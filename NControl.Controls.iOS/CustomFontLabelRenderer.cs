using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.iOS;
using UIKit;
using Foundation;
using CoreGraphics;
using CoreText;

[assembly: ExportRenderer (typeof (CustomFontLabel), typeof (CustomFontLabelRenderer))]
namespace NControl.Controls.iOS
{
	/// <summary>
	/// Custom font label renderer.
	/// </summary>
	public class CustomFontLabelRenderer: LabelRenderer
	{		
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Label> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null) 
			{
				// Register font 
				RegisterFont(e.NewElement);
			}
		}

		/// <summary>
		/// Registers the font.
		/// </summary>
		private void RegisterFont (Label element)
		{
			if (UIFont.FromName (element.FontFamily, (nfloat)element.FontSize) != null)
				return;

			var fontName = element.FontFamily;
			element.FontFamily = "Arial";

			var label = (CustomFontLabel)element;
			var fontData = label.FontData;

			NSError error;
			var dataProvider = new CGDataProvider (fontData, 0, fontData.Length);				
			var font = CGFont.CreateFromProvider(dataProvider);

			if (!CTFontManager.RegisterGraphicsFont(font, out error)) 
			{				
				throw new InvalidOperationException (error.Description);
			}

			element.FontFamily = fontName;
		}
	}
}

