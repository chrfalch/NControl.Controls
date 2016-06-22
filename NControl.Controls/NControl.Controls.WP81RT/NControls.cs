using System;
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
            var r = Xamarin.Forms.Platform.WinRT.Platform.CreateRenderer(new Xamarin.Forms.Label());
            var assemblies = GetAssemblies();
            FontLoader.LoadFonts(assemblies, (fontName, s) =>
            {
                fontName = fontName.ToLowerInvariant();

                // Create folder
                StorageFolder folder = null;
                try
                {
                    folder = ApplicationData.Current.LocalFolder.GetFolderAsync("Fonts").GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    folder = ApplicationData.Current.LocalFolder.CreateFolderAsync("Fonts").GetAwaiter().GetResult();
                }

                var fontFilename = Path.ChangeExtension(fontName, ".ttf");

                // Save Font
                try
                {
                    using (var targetStream = folder.OpenStreamForWriteAsync(fontFilename, CreationCollisionOption.FailIfExists).Result)
                    {
                        s.CopyTo(targetStream);                        
                    }
                }
                catch (Exception ex)
                {
                    //var fontFamily = new Windows.UI.Xaml.Media.FontFamily(Path.Combine(folder.Path, fontFilename));
                    //Typefaces[fontName] = fontFamily;
                }

                var fontFamily = new Windows.UI.Xaml.Media.FontFamily("/Local/Fonts/" + fontFilename);
                Typefaces[fontName] = fontFamily;
            });
        }

        private static IEnumerable<Assembly> GetAssemblies()
        {
            var retVal = new List<Assembly>();
            var folder = Package.Current.InstalledLocation;
            var files = folder.GetFilesAsync();
            foreach (var file in files.GetAwaiter().GetResult())
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

