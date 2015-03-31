using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using Windows.ApplicationModel.Activation;
using Windows.Storage;
using NControl.Controls;
using NControl.Controls.WP81;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(CustomFontLabel), typeof(CustomFontLabelRenderer))]
namespace NControl.Controls.WP81
{
    /// <summary>
    /// Custom font label renderer.
    /// </summary>
    public class CustomFontLabelRenderer : LabelRenderer
    {
        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                // Load font 
                var label = (CustomFontLabel)e.NewElement;
                var fontName = label.FontFamily;
                var fontData = label.FontData;
                var ms = new MemoryStream(fontData);
                
                Control.FontSource = new System.Windows.Documents.FontSource(ms);
                Control.FontFamily = new FontFamily(fontName);
            }
        }

    }
}

