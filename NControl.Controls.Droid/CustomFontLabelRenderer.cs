using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.Droid;
using Android.Graphics;
using System.IO;

[assembly: ExportRenderer (typeof (CustomFontLabel), typeof (CustomFontLabelRenderer))]
namespace NControl.Controls.Droid
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
				var label = (CustomFontLabel)e.NewElement;
				var fontName = label.FontFamily;

				var fontFilename = System.IO.Path.Combine (System.Environment.GetFolderPath (
					Environment.SpecialFolder.Personal), fontName);
				
				if (!File.Exists (fontFilename)) {
					using (var fs = new FileStream (fontFilename, FileMode.CreateNew)) {
						var fontData = label.FontData;
						fs.Write (fontData, 0, fontData.Length);
					}
				}

				var typeface = Typeface.CreateFromFile(fontFilename);
				Control.SetTypeface(typeface, TypefaceStyle.Normal);
			}
		}
	}
}

