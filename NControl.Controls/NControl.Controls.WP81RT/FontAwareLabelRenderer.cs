using System;
using System.ComponentModel;
using NControl.Controls.WP80;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(Label), typeof(FontAwareLabelRenderer))]
namespace NControl.Controls.WP80
{
    /// <summary>
    /// Custom font label renderer.
    /// </summary>
    public class FontAwareLabelRenderer : LabelRenderer
    {
        /// <summary>
        /// Raises the element changed event.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Label> e)
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
            if (NControls.Typefaces.ContainsKey(fontName))
                Control.FontSource = NControls.Typefaces[fontName];
        }

    }
}

