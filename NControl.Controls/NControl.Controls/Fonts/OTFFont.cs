using System;
using System.IO;
using System.Runtime.InteropServices;

namespace NControl.Controls.Fonts
{
	/// <summary>
	/// OTF font.
	/// </summary>
	public class OTFFont: BaseFont
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <returns>The name.</returns>
		/// <param name="s">S.</param>
		public static string GetName(Stream s)
		{
			TTCHeader ttcHeader;

			var br = new BinaryReader (s);
			ttcHeader.versiona = (br.ReadUInt16 ());
			ttcHeader.versionb = (br.ReadUInt16 ());
			ttcHeader.numTables = SwapWord (br.ReadUInt16 ());
			ttcHeader.searchRange = SwapWord (br.ReadUInt16 ());
			ttcHeader.entrySelector = SwapWord (br.ReadUInt16 ());
			ttcHeader.rangeShift = SwapWord (br.ReadUInt16 ());

			//check is this is a valid version
			if(ttcHeader.versiona != 0x0001 && ttcHeader.versionb != 0x0000)
				return null;

			return null;
		}
	}

	/// <summary>
	/// Name record
	/// </summary>
	[StructLayout(LayoutKind.Sequential, Pack = 0x1)]
	struct TTCHeader
	{
		public ushort versiona;
		public ushort versionb;
		public ushort numTables;
		public ushort searchRange;
		public ushort entrySelector;
		public ushort rangeShift;
	}
}

