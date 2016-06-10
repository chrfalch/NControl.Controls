using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Documents;

namespace NControl.Controls.WP81RT
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
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            FontLoader.LoadFonts(assemblies, (fontName, s) =>
            {
                fontName = fontName.ToLowerInvariant();
                var copyStream = new MemoryStream();
                s.CopyTo(copyStream);
                Typefaces[fontName] = new FontSource(copyStream);
            });
        }
    }
}

