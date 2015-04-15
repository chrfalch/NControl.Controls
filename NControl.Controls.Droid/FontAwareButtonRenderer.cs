using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.Droid;
using Android.Graphics;
using System.IO;

[assembly: ExportRenderer (typeof (Button), typeof (FontAwareButtonRenderer))]
namespace NControl.Controls.Droid
{
	/// <summary>
	/// Custom font label renderer.
	/// </summary>
	public class FontAwareButtonRenderer: ButtonRenderer
	{		
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Button> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null) 
				UpdateFont();
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if(e.PropertyName == Button.FontFamilyProperty.PropertyName ||
				e.PropertyName == Button.FontSizeProperty.PropertyName ||
				e.PropertyName == Button.FontAttributesProperty.PropertyName ||
				e.PropertyName == Button.TextProperty.PropertyName)
				UpdateFont();
		}

		private void UpdateFont()
		{
			var fontName = Element.FontFamily;
			if (string.IsNullOrWhiteSpace (fontName))
				return;

			fontName = fontName.ToLowerInvariant ();
			if(NControls.Typefaces.ContainsKey(fontName))
				Control.SetTypeface(NControls.Typefaces[fontName], TypefaceStyle.Normal);	
		}
	}
}

