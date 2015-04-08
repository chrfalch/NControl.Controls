using System;
using Xamarin.Forms;
using Foundation;
using CoreGraphics;
using CoreText;

namespace NControl.Controls.iOS
{
	/// <summary>
	/// NControls.
	/// </summary>
	public static class NControls
	{
		/// <summary>
		/// Init this instance to 
		/// </summary>
		public static void Init()
		{
			NControl.iOS.NControlViewRenderer.Init ();
			NControl.Controls.FontLoader.LoadFonts (AppDomain.CurrentDomain.GetAssemblies(), (fontName, s) => {

				// Copy bytes
				var data = new byte[s.Length];
				s.Read (data, 0, data.Length);

				NSError error;
				var dataProvider = new CGDataProvider (data, 0, data.Length);				
				var font = CGFont.CreateFromProvider(dataProvider);

				if (!CTFontManager.RegisterGraphicsFont(font, out error)) 
					throw new InvalidOperationException (error.Description);				
			});
		}
	}
}

