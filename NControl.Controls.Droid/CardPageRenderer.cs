using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.Droid;

[assembly: ExportRenderer(typeof(CardPage), typeof(CardPageRenderer))]
[assembly: ExportRenderer(typeof(DroidCardPageNavigationPage), typeof(DroidNavigationPageRenderer))]
namespace NControl.Controls.Droid
{
	/// <summary>
	/// Card page renderer.
	/// </summary>
	public class CardPageRenderer: PageRenderer
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.Droid.CardPageRenderer"/> class.
		/// </summary>
		public CardPageRenderer()
		{
			
		}

		/// <Docs>This is called when the view is attached to a window.</Docs>
		/// <summary>
		/// Raises the attached to window event.
		/// </summary>
		protected override void OnAttachedToWindow ()
		{
			base.OnAttachedToWindow ();

			Background = null;
			SetBackgroundColor (Android.Graphics.Color.Transparent);
		}
	}

	public class DroidNavigationPageRenderer: NavigationRenderer
	{
		/// <Docs>This is called when the view is attached to a window.</Docs>
		/// <summary>
		/// Raises the attached to window event.
		/// </summary>
		protected override void OnAttachedToWindow ()
		{
			base.OnAttachedToWindow ();

			Background = null;
			ViewGroup.Background = null;
			SetBackgroundColor (Android.Graphics.Color.Transparent);
			ViewGroup.SetBackgroundColor (Android.Graphics.Color.Transparent);
			ViewGroup.GetChildAt (0).Background = null;
			ViewGroup.GetChildAt (0).SetBackgroundColor(Android.Graphics.Color.Transparent);
		}
	}
}

