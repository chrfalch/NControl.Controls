using System;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.Droid;
using Xamarin.Forms.Platform.Android.AppCompat;
using Android.Views;
using System.Threading.Tasks;

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

			Background = null;
			SetBackgroundColor (Android.Graphics.Color.Transparent);

			var parent = (Parent as Android.Views.ViewGroup);
			if (parent != null) {
				for (var i = 0; i < parent.ChildCount; i++)
					parent.GetChildAt (i).SetBackgroundColor (Android.Graphics.Color.Transparent);
			}
		}	
	}
}

