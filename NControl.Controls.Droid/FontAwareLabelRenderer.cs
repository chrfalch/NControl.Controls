using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.Droid;
using Android.Graphics;
using System.IO;

[assembly: ExportRenderer (typeof (Label), typeof (FontAwareLabelRenderer))]
namespace NControl.Controls.Droid
{
	/// <summary>
	/// Custom font label renderer.
	/// </summary>
	public class FontAwareLabelRenderer: LabelRenderer
	{		
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Label> e)
		{
			base.OnElementChanged (e);

			if (e.NewElement != null) 
				UpdateFont();
		}

		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
		
			if(e.PropertyName == Label.FontFamilyProperty.PropertyName)
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

