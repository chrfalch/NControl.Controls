/************************************************************************
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2025 - Christian Falch
 * 
 * Permission is hereby granted, free of charge, to any person obtaining 
 * a copy of this software and associated documentation files (the 
 * "Software"), to deal in the Software without restriction, including 
 * without limitation the rights to use, copy, modify, merge, publish, 
 * distribute, sublicense, and/or sell copies of the Software, and to 
 * permit persons to whom the Software is furnished to do so, subject 
 * to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be 
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 ************************************************************************/

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;

namespace NControl.Controls
{
	/// <summary>
	/// Implements a simple cross-platform font loader with callbacks that can be used by platform implementations
	/// to register fonts on the device. Pass a list of assemblies to the LoadFonts method where fonts are embedded
	/// as resources and a callback for registering fonts.
	/// </summary>
	public static class FontLoader
	{
		/// <summary>
		/// The initialized flag
		/// </summary>
		private static bool _initialized = false;

		/// <summary>
		/// initializes
		/// </summary>
		public static void LoadFonts (IEnumerable<Assembly> assemblies, Action<string, Stream> registerFont)
		{
			if (_initialized)
				return;

			_initialized = true;

			foreach (var assembly in assemblies) {

				if (assembly.IsDynamic)
					continue;				
					
				// Find all resources ending with ttf 
				foreach (var name in assembly.GetManifestResourceNames()) {

					if (!name.ToLowerInvariant ().EndsWith (".ttf"))
						continue;

					var s = assembly.GetManifestResourceStream (name);
					var fontName = GetFontNameFromFontStream(s);
					s.Position = 0;
					registerFont (Path.GetFileName(fontName), s);
				}
			}
		}

		/// <summary>
		/// Gets the font names from font file.
		/// </summary>
		/// <returns>The font names from font file.</returns>
		/// <param name="s">S.</param>
		private static string GetFontNameFromFontStream(Stream s)
		{
			TT_OFFSET_TABLE ttOffsetTable;
			var br = new BinaryReader (s);
			ttOffsetTable.uMajorVersion = SwapWord(br.ReadUInt16 ());
			ttOffsetTable.uMinorVersion = SwapWord (br.ReadUInt16 ());
			ttOffsetTable.uNumOfTables = SwapWord (br.ReadUInt16 ());
			ttOffsetTable.uSearchRange = SwapWord (br.ReadUInt16 ());
			ttOffsetTable.uEntrySelector = SwapWord (br.ReadUInt16 ());
			ttOffsetTable.uRangeShift = SwapWord (br.ReadUInt16 ());

			//check is this is a true type font and the version is 1.0
			if(ttOffsetTable.uMajorVersion != 1 || ttOffsetTable.uMinorVersion != 0)
				return null;

			TT_TABLE_DIRECTORY tblDir;
			string csTemp;

			for(var i=0; i< ttOffsetTable.uNumOfTables; i++){

				tblDir.szTag1 = (char)br.ReadByte ();
				tblDir.szTag2 = (char)br.ReadByte ();
				tblDir.szTag3 = (char)br.ReadByte ();
				tblDir.szTag4 = (char)br.ReadByte ();
				tblDir.uCheckSum = SwapLong (br.ReadUInt32());
				tblDir.uOffset = SwapLong (br.ReadUInt32());
				tblDir.uLength = SwapLong (br.ReadUInt32());

				//table's tag cannot exceed 4 characters
				csTemp = tblDir.szTag1.ToString () + tblDir.szTag2.ToString () +
					tblDir.szTag3.ToString () + tblDir.szTag4.ToString ();
				
				if(csTemp.ToLowerInvariant().Equals("name")){

					// we found our table. Rearrange order and quit the loop
					//move to offset we got from Offsets Table
					s.Seek(tblDir.uOffset, SeekOrigin.Begin);
					TT_NAME_TABLE_HEADER ttNTHeader;

					ttNTHeader.uFSelector = SwapWord (br.ReadUInt16 ());
					ttNTHeader.uNRCount = SwapWord (br.ReadUInt16 ());
					ttNTHeader.uStorageOffset = SwapWord (br.ReadUInt16 ());

					//again, don't forget to swap bytes!
					TT_NAME_RECORD ttRecord;
					csTemp = string.Empty;

					for (var n = 0; n < ttNTHeader.uNRCount; n++) {

						ttRecord.uPlatformID = SwapWord (br.ReadUInt16 ());
						ttRecord.uEncodingID = SwapWord (br.ReadUInt16 ());
						ttRecord.uLanguageID = SwapWord (br.ReadUInt16 ());
						ttRecord.uNameID = SwapWord (br.ReadUInt16 ());
						ttRecord.uStringLength = SwapWord (br.ReadUInt16 ());
						ttRecord.uStringOffset = SwapWord (br.ReadUInt16 ());

						// 1 says that this is font name. 0 for example determines copyright info
						if (ttRecord.uNameID == 1) {

							// save file position, so we can return to continue with search
							var nPos = s.Position;
							s.Seek (tblDir.uOffset + ttRecord.uStringOffset +
								ttNTHeader.uStorageOffset, SeekOrigin.Begin);

							// read string
							var stringData = br.ReadBytes (ttRecord.uStringLength);
							if(ttRecord.uEncodingID == 0)
								csTemp = System.Text.Encoding.UTF8.GetString(stringData, 0, ttRecord.uStringLength);
							else
								csTemp = System.Text.Encoding.BigEndianUnicode.GetString (stringData, 0, ttRecord.uStringLength);

							// Try once more with bigendian unicode
							if(csTemp.Length == 0)
								csTemp = System.Text.Encoding.BigEndianUnicode.GetString (stringData, 0, ttRecord.uStringLength);

							// yes, still need to check if the font name is not empty
							// if it is, continue the search
							if (csTemp.Length > 0) {
								return csTemp;
							}
							s.Seek (nPos, SeekOrigin.Begin);
						}
					}
					break;
				}
			}

			return null;
		}

		/// <summary>
		/// Swaps the word.
		/// </summary>
		/// <returns>The word.</returns>
		/// <param name="value">Value.</param>
		private static UInt16 SwapWord(UInt16 value) 
		{
			return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
		}

		/// <summary>
		/// Swaps bytes in a long.
		/// </summary>
		/// <returns>The word.</returns>
		/// <param name="value">Value.</param>
		private static UInt32 SwapLong(UInt32 value)
		{
			return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
				(value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
		}

		/// <summary>
		/// Reverses the bytes.
		/// </summary>
		/// <returns>The bytes.</returns>
		/// <param name="value">Value.</param>
		private static UInt64 ReverseBytes(UInt64 value)
		{
			return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
				(value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
				(value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
				(value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
		}
	}

 	/// <summary>
	/// TTF file header
 	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 0x1)]
	struct TT_OFFSET_TABLE
	{
		public ushort uMajorVersion;
		public ushort uMinorVersion;
		public ushort uNumOfTables;
		public ushort uSearchRange;
		public ushort uEntrySelector;
		public ushort uRangeShift;

	}

	/// <summary>
	/// Directory
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 0x1)]
	struct TT_TABLE_DIRECTORY
	{            
		public char szTag1;
		public char szTag2;
		public char szTag3;
		public char szTag4;            
		public uint uCheckSum; //Check sum
		public uint uOffset; //Offset from beginning of file
		public uint uLength; //length of the table in bytes
	}

	/// <summary>
	/// Name Table header
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 0x1)]
	struct TT_NAME_TABLE_HEADER
	{
		public ushort uFSelector;
		public ushort uNRCount;
		public ushort uStorageOffset;
	}

	/// <summary>
	/// Name record
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 0x1)]
	struct TT_NAME_RECORD
	{
		public ushort uPlatformID;
		public ushort uEncodingID;
		public ushort uLanguageID;
		public ushort uNameID;
		public ushort uStringLength;
		public ushort uStringOffset;
	}
}

