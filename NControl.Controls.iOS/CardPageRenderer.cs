using System;
using Xamarin.Forms;
using NControl.Controls.iOS;
using NControl.Controls;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using CoreGraphics;

[assembly: ExportRenderer (typeof (CardPage), typeof (CardPageRenderer))]
namespace NControl.Controls.iOS
{
	/// <summary>
	/// Renders a card page - ie a view with transparent background
	/// </summary>
	public class CardPageRenderer: PageRenderer
	{
		/// <summary>
		/// Render screenshot on display
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (VisualElementChangedEventArgs e)
		{
			base.OnElementChanged (e);


		}	
			
		/// <summary>
		/// Handles the appearing.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void TakeScreenshot ()
		{
			if (Element == null)
				return;
			
			// Take screenshot
			UIGraphics.BeginImageContextWithOptions (UIScreen.MainScreen.Bounds.Size, false, 
				UIScreen.MainScreen.Scale);

			UIApplication.SharedApplication.KeyWindow.Layer.RenderInContext (UIGraphics.GetCurrentContext ());
			var screenshot = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();

			(Element as CardPage).BackgroundImageSource = ImageSource.FromStream (() => screenshot.AsPNG ().AsStream ());
		}

		public override void ViewWillAppear (bool animated)
		{
			TakeScreenshot ();
			base.ViewWillAppear (animated);
		}
	}
}

