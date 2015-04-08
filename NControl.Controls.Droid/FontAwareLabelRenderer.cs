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
			{
				var label = e.NewElement;
				var fontName = label.FontFamily;
				if (string.IsNullOrWhiteSpace (fontName))
					return;

				fontName = fontName.ToLowerInvariant ();
				if(NControls.Typefaces.ContainsKey(fontName))
					Control.SetTypeface(NControls.Typefaces[fontName], TypefaceStyle.Normal);
			}
		}
	}
}

