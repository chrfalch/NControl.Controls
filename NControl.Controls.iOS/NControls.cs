using System;
using Xamarin.Forms;
using Foundation;
using CoreGraphics;
using CoreText;
using System.IO;
using UIKit;
using System.Collections.Generic;

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
			NControl.Controls.FontLoader.LoadFonts (AppDomain.CurrentDomain.GetAssemblies(), (fontName, s) => {

				var data = new byte[s.Length];
				s.Read (data, 0, data.Length);

				var dataProvider = new CGDataProvider (data, 0, data.Length);
				var familyNames = UIFont.FamilyNames; //fixes app freeze in some cases: https://alpha.app.net/jaredsinclair/post/18555292
				var cgFont = CGFont.CreateFromProvider (dataProvider);
				NSError error;

				var registered = CTFontManager.RegisterGraphicsFont(cgFont, out error);
				if (!registered)
				{
					// If the error code is 105 then the font we are trying to register is already registered
					// We will not report this as an error.
					if (error.Code != 105)
						throw new ArgumentException("Error registering: " + fontName + 
							" (" + error.LocalizedDescription + ")");
				}
			});
		}
	}
}

