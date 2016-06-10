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
		/// <summary>
		/// The parent.
		/// </summary>
		private UIViewController _parentModalViewController;

        /// <summary>
		/// Dids the move to parent view controller.
		/// </summary>
		/// <param name="parent">Parent.</param>
		public override void DidMoveToParentViewController (UIViewController parent)
		{
			base.DidMoveToParentViewController (parent);

			// Save modal wrapper from Xamarin.Forms
			_parentModalViewController = parent;

			// Set custom to be able to set clear background!
			parent.ModalPresentationStyle = UIModalPresentationStyle.Custom;
		}

        /// <summary>
        /// Views the will appear.
        /// </summary>
        /// <param name="animated">If set to <c>true</c> animated.</param>
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(false);

            // Clear background on parent modal wrapper!!
            _parentModalViewController.View.BackgroundColor = UIColor.Clear;
            View.BackgroundColor = UIColor.Clear;
        }

		/// <summary>
		/// Views the did appear.
		/// </summary>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (false);

			// Clear background on parent modal wrapper!!
            _parentModalViewController.View.BackgroundColor = UIColor.Clear;
            View.BackgroundColor = UIColor.Clear;
		}
	}
}

