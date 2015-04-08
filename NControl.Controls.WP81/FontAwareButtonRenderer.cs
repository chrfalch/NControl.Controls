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

[assembly: ExportRenderer(typeof(Label), typeof(FontAwareLabelRenderer))]
namespace NControl.Controls.WP81
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
			{
				var label = e.NewElement;
				var fontName = label.FontFamily;
				if (string.IsNullOrWhiteSpace (fontName))
					return;

				fontName = fontName.ToLowerInvariant ();
			    if (NControls.Typefaces.ContainsKey(fontName))
			    {
                    Control.FontSource = NControls.Typefaces[fontName];
                    Control.FontFamily = new FontFamily(fontName);
			    }
			}
		}

	}
}

