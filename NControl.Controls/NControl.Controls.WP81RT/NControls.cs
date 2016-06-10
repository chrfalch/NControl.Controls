using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Windows.ApplicationModel;
using Windows.Storage;

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
        public static readonly Dictionary<string, Windows.UI.Xaml.Media.FontFamily> Typefaces = 
            new Dictionary<string, Windows.UI.Xaml.Media.FontFamily>();

        /// <summary>
        /// Init this instance to 
        /// </summary>
        public static void Init()
        {
            var assemblies = GetAssemblies();
            FontLoader.LoadFonts(assemblies, (fontName, s) =>
            {
                fontName = fontName.ToLowerInvariant();

                // Create folder
                var folder = Package.Current.InstalledLocation.GetFolderAsync("Fonts").GetResults();
                if (folder == null)
                    folder = Package.Current.InstalledLocation.CreateFolderAsync("Fonts").GetResults();

                // Save Font
                if (folder.GetFileAsync(fontName) == null)
                    using (var targetStream = folder.OpenStreamForWriteAsync(fontName, CreationCollisionOption.FailIfExists).Result)
                    {
                        s.CopyTo(targetStream);
                        var fontFamily = new Windows.UI.Xaml.Media.FontFamily(Path.Combine(folder.Path, fontName));
                        Typefaces[fontName] = fontFamily;
                    }
            });
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            var retVal = new List<Assembly>();
            var folder = Package.Current.InstalledLocation;
            foreach (var file in folder.GetFilesAsync().GetResults())
            {
                if (file.FileType == ".dll")
                {
                    var assemblyName = new AssemblyName(file.DisplayName);
                    var assembly = Assembly.Load(assemblyName);
                    retVal.Add(assembly);
                }
            }

            return retVal;
        }
    }
}

