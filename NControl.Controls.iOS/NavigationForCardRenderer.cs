using System;
using Xamarin.Forms;
using NControl.Controls.iOS;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using UIKit;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationForCardRenderer))]
namespace NControl.Controls.iOS
{
	/// <summary>
	/// Renders a card page - ie a view with transparent background
	/// </summary>
	public class NavigationForCardRenderer: NavigationRenderer
	{
		#region Private Members

		/// <summary>
		/// The will present card page.
		/// </summary>
		private bool _willPresentCardPage = false;

		private UIViewController _presentedController = null;

		#endregion

		/// <summary>
		/// Raises the push async event.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		protected override Task<bool> OnPushAsync (Page page, bool animated)
		{
			if (page is CardPage)
				_willPresentCardPage = true;
			else
				_willPresentCardPage = false;

			return base.OnPushAsync (page, animated);
		}

		/// <summary>
		/// Raises the pop view async event.
		/// </summary>
		/// <param name="page">Page.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		protected override Task<bool> OnPopViewAsync (Page page, bool animated)
		{
			if (page is CardPage) {
				var tcs = new TaskCompletionSource<bool> ();
				_presentedController.DismissViewController (animated, () => tcs.SetResult (true));
				_presentedController = null;
				return tcs.Task;
			}

			return base.OnPopViewAsync (page, animated);
		}
			
		/// <summary>
		/// Pushs the view controller.
		/// </summary>
		/// <param name="viewController">View controller.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		public override void PushViewController (UIKit.UIViewController viewController, bool animated)
		{
			if (_willPresentCardPage) {
				viewController.TransitioningDelegate = new CardPageTransitioningDelegate ();
				viewController.ModalPresentationStyle = UIModalPresentationStyle.Custom;
				_presentedController = viewController;
				PresentViewController (viewController, animated, null);
			}
			else
				base.PushViewController (viewController, animated);
		}
	}

	/// <summary>
	/// Card page transitioning delegate.
	/// </summary>
	public class CardPageTransitioningDelegate: UIViewControllerTransitioningDelegate
	{
		/// <summary>
		/// Gets the animation controller for presented controller.
		/// </summary>
		/// <returns>The animation controller for presented controller.</returns>
		/// <param name="presented">Presented.</param>
		/// <param name="presenting">Presenting.</param>
		/// <param name="source">Source.</param>
		public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForPresentedController(
			UIViewController presented, UIViewController presenting, UIViewController source)
		{
			var animator = new CardPageAnimatedTransitioning ();
			animator.IsPresenting = true;
			return animator;
		}

		/// <summary>
		/// Gets the animation controller for dismissed controller.
		/// </summary>
		/// <returns>The animation controller for dismissed controller.</returns>
		/// <param name="dismissed">Dismissed.</param>
		public override IUIViewControllerAnimatedTransitioning GetAnimationControllerForDismissedController (
			UIViewController dismissed)
		{
			var animator = new CardPageAnimatedTransitioning ();
			animator.IsPresenting = false;
			return animator;
		}
	}

	public class CardPageAnimatedTransitioning: UIViewControllerAnimatedTransitioning
	{
		/// <summary>
		/// Gets or sets a value indicating whether this instance is presented.
		/// </summary>
		/// <value><c>true</c> if this instance is presented; otherwise, <c>false</c>.</value>
		public bool IsPresenting {get;set;}

		#region implemented abstract members of UIViewControllerAnimatedTransitioning

		/// <summary>
		/// Animates the transition.
		/// </summary>
		/// <param name="transitionContext">Transition context.</param>
		public override void AnimateTransition (IUIViewControllerContextTransitioning transitionContext)
		{
			var fromViewController = transitionContext.GetViewControllerForKey (UITransitionContext.FromViewControllerKey);
			var toViewController = transitionContext.GetViewControllerForKey (UITransitionContext.ToViewControllerKey);
			var containerView = transitionContext.ContainerView;

			// Set our ending frame. We'll modify this later if we have to
			var endFrame = containerView.Bounds;

			if (IsPresenting) 
			{
				fromViewController.View.UserInteractionEnabled = false;

				//containerView.AddSubview(fromViewController.View);
				containerView.AddSubview (new UIView{ 
					Frame = containerView.Bounds,
					BackgroundColor= Color.Black.ToUIColor(),
					Alpha = 0.1F
				});
				containerView.AddSubview(toViewController.View);

				var startFrame = endFrame;
				startFrame.Y += 500;

				toViewController.View.Frame = startFrame;

				UIView.Animate (TransitionDuration (transitionContext), () => {
					// Animation
					fromViewController.View.TintAdjustmentMode = UIViewTintAdjustmentMode.Dimmed;
					toViewController.View.Frame = endFrame;

				}, ()=> {
					// Completed
					transitionContext.CompleteTransition(true);

				});

			}
			else {
				toViewController.View.UserInteractionEnabled = true;

				//containerView.AddSubview(toViewController.View);
				containerView.AddSubview(fromViewController.View);

				endFrame.Y += 500;

				UIView.Animate (TransitionDuration (transitionContext), () => {
					// Animation
					toViewController.View.TintAdjustmentMode = UIViewTintAdjustmentMode.Normal;
					fromViewController.View.Frame = endFrame;

				}, ()=> {
					// Completed
					transitionContext.CompleteTransition(true);

				});
			}
		}

		/// <summary>
		/// Transitions the duration.
		/// </summary>
		/// <returns>The duration.</returns>
		/// <param name="transitionContext">Transition context.</param>
		public override double TransitionDuration (IUIViewControllerContextTransitioning transitionContext)
		{
			return 0.5;
		}
		#endregion
	}
}

