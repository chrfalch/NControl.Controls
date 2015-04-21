using System;
using Xamarin.Forms;
using NControl.Controls.iOS;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using UIKit;
using NControl.Controls;

[assembly: ExportRenderer(typeof(CardPage), typeof(CardPageRenderer))]
namespace NControl.Controls.iOS
{
	/// <summary>
	/// Renders a card page - ie a view with transparent background
	/// </summary>
	public class CardPageRenderer: PageRenderer
	{
		private UIImageView _backgroundScreen;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			View.BackgroundColor = UIColor.Clear;
		}

		public override void DidMoveToParentViewController (UIViewController parent)
		{
			base.DidMoveToParentViewController (parent);

			UIGraphics.BeginImageContextWithOptions (UIScreen.MainScreen.Bounds.Size, false, UIScreen.MainScreen.Scale);
			UIApplication.SharedApplication.KeyWindow.Layer.RenderInContext (UIGraphics.GetCurrentContext ());
			var image = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();

			_backgroundScreen = new UIImageView { 
				ContentMode = UIViewContentMode.ScaleAspectFill,
				Image = image,
				Frame = View.Bounds,
			};

			View.Superview.InsertSubviewBelow (_backgroundScreen, View);
		}			

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			_backgroundScreen.RemoveFromSuperview ();
		}
	}
}

