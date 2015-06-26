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
		#region ICardPageHelper implementation

		/// <summary>
		/// Shows the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="card">Card.</param>
		public Task ShowAsync (CardPage card)
		{	
			return Application.Current.MainPage.Navigation.PushModalAsync (card, false);
		}

		/// <summary>
		/// Hides the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="card">Card.</param>
		public Task CloseAsync (CardPage card)
		{
			return Application.Current.MainPage.Navigation.PopModalAsync ();
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
		public bool ControlAnimatesItself { get { return true; } }

		#endregion	
	}
}

