using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.Droid;
using Android.App;
using System.IO;
using Android.Graphics;
using Android.Graphics.Drawables;

[assembly: ExportRenderer (typeof (CardPage), typeof (CardPageRenderer))]
namespace NControl.Controls.Droid
{
	/// <summary>
	/// Card page renderer.
	/// </summary>
	public class CardPageRenderer: PageRenderer
	{
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<Page> e)
		{
			base.OnElementChanged (e);

			if (e.OldElement != null)
				e.OldElement.Appearing -= HandleAppearing;
			
			if (e.NewElement != null) 
				e.NewElement.Appearing += HandleAppearing;			
		}

		void HandleAppearing (object sender, EventArgs e)
		{
			if (Element == null)
				return;

			SetBackgroundDrawable (null);
			return;

//			// Take screenshot
//			var activity = (Xamarin.Forms.Forms.Context as Activity);
//			var view = activity.Window.DecorView.RootView;
//			view.DrawingCacheEnabled = true;
//			var bitmap = view.GetDrawingCache (true);
//
//			using (var stream = new MemoryStream ()) {
//
//				bitmap.Compress (Bitmap.CompressFormat.Png, 0, stream);
//				var cardPage = Element as CardPage;
//				cardPage.BackgroundImageSource = ImageSource.FromStream (() => new MemoryStream (stream.GetBuffer ()));
//			}
//
//			view.DrawingCacheEnabled = false;			
		}				
	}
}

