using System;
using Xamarin.Forms;
using Foundation;
using CoreGraphics;
using CoreText;
using System.IO;
using UIKit;

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

//				var folder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
//				var filename = Path.Combine (folder, fontName + ".ttf");
//				NSError error;
//
//				if(!File.Exists(filename))
//				{
//					using(var fs = new FileStream(filename, FileMode.CreateNew))
//						s.CopyTo(fs);					
//
//					s.Position = 0;
//				var data = new byte[s.Length];
//				s.Read (data, 0, data.Length);
//
//				var dataProvider = new CGDataProvider (data, 0, data.Length);				
//				var font = CGFont.CreateFromProvider(dataProvider);
//
//				NSError error;
//				if (!CTFontManager.RegisterGraphicsFont(font, out error)) 
//					throw new InvalidOperationException (error.Description);
//				}

//				error = CTFontManager.RegisterFontsForUrl(NSUrl.FromFilename(filename), 
//					CTFontManagerScope.Session);
//				
//				if(error != null)
//					throw new InvalidOperationException (error.Description);

//				var indexOf = Array.IndexOf(UIFont.FamilyNames, fontName);
//				if(indexOf > -1)
//					return;
			});
		}
	}
}

