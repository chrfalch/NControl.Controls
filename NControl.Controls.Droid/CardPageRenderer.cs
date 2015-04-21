using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.Droid;

[assembly: ExportRenderer(typeof(CardPage), typeof(CardPageRenderer))]
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

			SetBackgroundDrawable (null);
			SetBackgroundColor (Android.Graphics.Color.Transparent);
		}
	}
}

