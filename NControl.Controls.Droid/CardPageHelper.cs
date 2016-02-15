using System;
using System.Reflection;
using System.Linq;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.Droid;
using System.Threading.Tasks;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Content;

[assembly: Xamarin.Forms.Dependency (typeof (CardPageHelper))]
namespace NControl.Controls.Droid
{
	/// <summary>
	/// Card page renderer.
	/// </summary>
	public class CardPageHelper: ICardPageHelper
	{
		#region ICardPageHelper implementation

		/// <summary>
		/// Gets the size of the screen.
		/// </summary>
		/// <returns>The screen size.</returns>
		public Size GetScreenSize ()
		{			
			var metrics = Forms.Context.Resources.DisplayMetrics;
			var widthInDp = ConvertPixelsToDp(metrics.WidthPixels);
			var heightInDp = ConvertPixelsToDp(metrics.HeightPixels);

			return new Size (widthInDp, heightInDp);
		}

		/// <summary>
		/// Shows the card async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="page">Page.</param>
		public Task ShowAsync (CardPage page)
		{			
			return Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync (new DroidCardPageNavigationPage(page), false);
		}

		/// <summary>
		/// Closes the card async
		/// </summary>
		/// <returns>The aync.</returns>
		/// <param name="page">Page.</param>
		public Task CloseAsync (CardPage page)
		{
			return Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync (false);
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

		#region Private Members

		/// <summary>
		/// Converts the pixels to dp.
		/// </summary>
		/// <returns>The pixels to dp.</returns>
		/// <param name="pixelValue">Pixel value.</param>
		private int ConvertPixelsToDp(float pixelValue)
		{
			var dp = (int) ((pixelValue)/Forms.Context.Resources.DisplayMetrics.Density);
			return dp;
		}

		#endregion
	}

	/// <summary>
	/// Droid card page navigation page.
	/// </summary>
	public class DroidCardPageNavigationPage : NavigationPage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.Droid.DroidCardPageNavigationPage"/> class.
		/// </summary>
		public DroidCardPageNavigationPage ()
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.Droid.DroidCardPageNavigationPage"/> class.
		/// </summary>
		/// <param name="rootPage">Root page.</param>
		public DroidCardPageNavigationPage (Page rootPage): base(rootPage)
		{
			
		}
	}
}

