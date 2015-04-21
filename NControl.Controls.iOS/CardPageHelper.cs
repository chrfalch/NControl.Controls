using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.iOS;
using UIKit;
using System.Threading.Tasks;
using CoreGraphics;
using System.Collections.Generic;

[assembly: Xamarin.Forms.Dependency (typeof (CardPageHelper))]
namespace NControl.Controls.iOS
{
	/// <summary>
	/// Card page renderer.
	/// </summary>
	public class CardPageHelper: ICardPageHelper
	{
		/// <summary>
		/// The presented controllers.
		/// </summary>
		private readonly Dictionary<CardPage, CardPageContext> _presentedCardPageContexts = 
			new Dictionary<CardPage, CardPageContext> ();

		#region ICardPageHelper implementation

		/// <summary>
		/// Shows the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="card">Card.</param>
		public async Task ShowAsync (CardPage card, Page mainPage)
		{			
			var window = UIApplication.SharedApplication.KeyWindow;

			if (!_presentedCardPageContexts.ContainsKey (card)) {

				_presentedCardPageContexts.Add (card, new CardPageContext{
					Controller = card.CreateViewController (),
					Overlay = new UIView {
						Alpha = 0.0f,
						BackgroundColor = UIColor.Black,
						Frame = window.Bounds,
					}
				});
			}
			
			var cardPageContext = _presentedCardPageContexts [card];

			// Presentation rect
			var rect = new CGRect (
				(nfloat)card.CardPadding.Left, 
				(nfloat)card.CardPadding.Top, 
				(nfloat)(window.Bounds.Width - (card.CardPadding.Right + (nfloat)card.CardPadding.Left)), 
				(nfloat)(window.Bounds.Height - (card.CardPadding.Bottom + (nfloat)card.CardPadding.Top)));
			
			cardPageContext.Controller.View.Frame = rect;
			cardPageContext.Controller.View.Layer.CornerRadius = 4;
			cardPageContext.Controller.View.Layer.MasksToBounds = true;
			cardPageContext.Controller.View.Transform = 
				CGAffineTransform.MakeTranslation (0, window.Bounds.Height);

			window.AddSubview (cardPageContext.Overlay);
			window.AddSubview (cardPageContext.Controller.View);

			// Animate
			await UIView.AnimateAsync (0.15f, () => cardPageContext.Overlay.Alpha = 0.2f);
			await UIView.AnimateAsync (0.2f, () => {
				cardPageContext.Controller.View.Transform = CGAffineTransform.MakeIdentity();
			});
		}

		/// <summary>
		/// Hides the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="card">Card.</param>
		public async Task HideAsync (CardPage card, Page mainPage)
		{
			var window = UIApplication.SharedApplication.KeyWindow;

			var cardPageContext = _presentedCardPageContexts [card];
			cardPageContext.Controller.View.Transform = CGAffineTransform.MakeIdentity ();
				
			// Animate
			await UIView.AnimateAsync (0.2f, () => {
				cardPageContext.Controller.View.Transform = 
					CGAffineTransform.MakeTranslation (0, window.Bounds.Height);
			});		

			await UIView.AnimateAsync (0.15f, () => cardPageContext.Overlay.Alpha = 0.0f);

			cardPageContext.Controller.View.RemoveFromSuperview ();
			cardPageContext.Overlay.RemoveFromSuperview ();
			cardPageContext.Controller.View.Transform = CGAffineTransform.MakeIdentity ();

		}
			
		/// <summary>
		/// Gets the size of the screen.
		/// </summary>
		/// <returns>The screen size.</returns>
		public Size GetScreenSize ()
		{
			return new Size (UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
		}
		#endregion
		
	}

	/// <summary>
	/// Card page context.
	/// </summary>
	public class CardPageContext
	{
		/// <summary>
		/// Gets or sets the controller.
		/// </summary>
		/// <value>The controller.</value>
		public UIViewController Controller { get; set; }

		/// <summary>
		/// Gets or sets the overlay.
		/// </summary>
		/// <value>The overlay.</value>
		public UIView Overlay { get; set; }
	}
}

