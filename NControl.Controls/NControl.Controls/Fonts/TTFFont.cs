using System;
using System.Runtime.InteropServices;
using System.IO;

namespace NControl.Controls.Fonts
{
	/// <summary>
	/// TTF font.
	/// </summary>
	public class TTFFont: BaseFont
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <returns>The name.</returns>
		/// <param name="s">S.</param>
		public static string GetName(Stream s)
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
							if(csTemp[0] ==  '\0')
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

