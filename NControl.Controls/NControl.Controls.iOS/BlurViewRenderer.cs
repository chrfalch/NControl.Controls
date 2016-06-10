using System;
using NControl.Controls.iOS;
using NControl.Controls;
using Xamarin.Forms;
using NControl.iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer (typeof (BlurImageView), typeof (BlurViewRenderer))]
namespace NControl.Controls.iOS
{
	public class BlurViewRenderer: ImageRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged (e);
		
			if (Control != null) {
				
				var blurOverlay = new UIVisualEffectView (UIBlurEffect.FromStyle (UIBlurEffectStyle.Light));
				blurOverlay.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				Control.AddSubview(blurOverlay);
			}
		}
	}
}

