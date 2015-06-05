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
using NControl.Controls.Fonts;

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

					if (name.ToLowerInvariant ().EndsWith (".ttf")) 
					{
						using (var s = assembly.GetManifestResourceStream (name)) {
							var fontName = TTFFont.GetName (s);					
							s.Position = 0;
							registerFont (Path.GetFileName (fontName), s);
						}
					}
				}
			}
		}
	}
}

