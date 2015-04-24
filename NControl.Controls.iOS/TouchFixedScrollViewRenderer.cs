using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using NControl.Controls.iOS;
using UIKit;

[assembly: ExportRenderer (typeof (ScrollView), typeof (TouchFixedScrollViewRenderer))]
namespace NControl.Controls.iOS
{
	/// <summary>
	/// Touch fixed scroll view renderer.
	/// </summary>
	public class TouchFixedScrollViewRenderer: ScrollViewRenderer
	{
		protected override void OnElementChanged (VisualElementChangedEventArgs e)
		{
			base.OnElementChanged (e);

			if (e.OldElement == null)
				((UIScrollView)NativeView).DelaysContentTouches = false;
		}
	}
}

