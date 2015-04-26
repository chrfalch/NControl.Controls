using System.ComponentModel;
using NControl.Controls.WP80;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(Button), typeof(FontAwareButtonRenderer))]
namespace NControl.Controls.WP80
{
    /// <summary>
    /// Custom font label renderer.
    /// </summary>
    public class FontAwareButtonRenderer : ButtonRenderer
    {
        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Button> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
                UpdateFont();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Label.FontFamilyProperty.PropertyName)
                UpdateFont();
        }

        private void UpdateFont()
        {
            var fontName = Element.FontFamily;
            if (string.IsNullOrWhiteSpace(fontName))
                return;

            fontName = fontName.ToLowerInvariant();
            // TODO: Fix
            //if (NControls.Typefaces.ContainsKey(fontName))
            //    Control.ApplyFont(NControls.Typefaces[fontName]);
        }

    }
}

