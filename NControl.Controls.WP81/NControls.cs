using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;

namespace NControl.Controls.WP81
{
    /// <summary>
    /// NControls.
    /// </summary>
    public static class NControls
    {
		/// <summary>
		/// The typefaces.
		/// </summary>
		public static readonly Dictionary<string, FontSource> Typefaces = new Dictionary<string, FontSource>();

		/// <summary>
        /// Init this instance to 
        /// </summary>
        public static void Init()
        {            
			NControl.WP81.NControlViewRenderer.Init();
		    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
			NControl.Controls.FontLoader.LoadFonts (assemblies, (fontName, s) => {

				fontName = fontName.ToLowerInvariant();
				Typefaces[fontName] = new System.Windows.Documents.FontSource(s);
			});
        }
    }
}

