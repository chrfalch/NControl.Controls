using System;
using Xamarin.Forms;
using NControl.Controls.iOS;
using UIKit;
using System.Threading.Tasks;
using CoreGraphics;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms.Platform.iOS;
using Foundation;

[assembly: Dependency (typeof (CardPageHelper))]
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
//		private readonly Dictionary<CardPage, CardPageContext> _presentedCardPageContexts = 
//			new Dictionary<CardPage, CardPageContext> ();

		#region ICardPageHelper implementation

		/// <summary>
		/// Shows the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="card">Card.</param>
		public async Task ShowAsync (CardPage card)
		{	
			// If not showing any modals at the moment:
//			if (Application.Current.MainPage.Navigation.ModalStack.Count == 0) {
//
//				// Find platform renderer uiviewcontroller
//				var currentController = UIApplication.SharedApplication.KeyWindow.RootViewController;
//				currentController.TransitioningDelegate = this;
//				currentController.ModalPresentationStyle = UIModalPresentationStyle.Custom;
//			}

			await Application.Current.MainPage.Navigation.PushModalAsync (card, false);

//			var window = UIApplication.SharedApplication.KeyWindow;
//
//			// Create card context
//			var context = new CardPageContext {
////				Controller = card.CreateViewController(), 
////				Controller = new CardPageRenderer (),
//				Controller = (UIViewController)RendererFactory.GetRenderer(card),
////				Overlay = new UIView {
////					Alpha = 0.0f,
////					BackgroundColor = UIColor.Black,
////					Frame = window.Bounds,
////				}
//			};
//
//			_presentedCardPageContexts.Add (card, context);	
//
//			// Set element
//			var renderer = (context.Controller as CardPageRenderer);
//			if(renderer != null)
//				renderer.SetElement(card);
//
//			// controller containment
//			window.RootViewController.AddChildViewController (context.Controller);
//			window.RootViewController.View.AddSubview (context.Controller.View);
//			context.Controller.DidMoveToParentViewController (window.RootViewController);
//
//			return Task.FromResult (true);
		}

		/// <summary>
		/// Hides the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="card">Card.</param>
		public async Task CloseAsync (CardPage card)
		{
			await Application.Current.MainPage.Navigation.PopModalAsync ();

//			var window = UIApplication.SharedApplication.KeyWindow;
//
//			var cardPageContext = _presentedCardPageContexts [card];
//			cardPageContext.Controller.View.Transform = CGAffineTransform.MakeIdentity ();
//				
//			// Controller containment
//			cardPageContext.Controller.WillMoveToParentViewController (null);
//			cardPageContext.Controller.View.RemoveFromSuperview ();
//			cardPageContext.Controller.RemoveFromParentViewController ();
//
//			// Clean up
//			cardPageContext.Controller.Dispose ();
//		    cardPageContext.Controller = null;
//
//			return Task.FromResult (true);
		}
			
		/// <summary>
		/// Gets the size of the screen.
		/// </summary>
		/// <returns>The screen size.</returns>
		public Size GetScreenSize ()
		{
			return new Size (UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="NControl.Controls.iOS.CardPageHelper"/> control animates itself.
		/// </summary>
		/// <value><c>true</c> if control animates itself; otherwise, <c>false</c>.</value>
		public bool ControlAnimatesItself {
			get {
				return true;
			}
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
	}
}

