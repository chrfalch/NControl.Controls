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
		/// Shows the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="card">Card.</param>
		public Task ShowAsync (CardPage card)
		{			
			return Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync (card, false);
		}

		/// <summary>
		/// Hides the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="card">Card.</param>
		public Task HideAsync (CardPage card)
		{
			return CloseAsync (card);
		}

		/// <summary>
		/// Disposes the card.
		/// </summary>
		/// <param name="card">Card.</param>
		public async Task CloseAsync (CardPage card)
		{
			await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync (false);
		}

		/// <summary>
		/// Shoulds the render chrome.
		/// </summary>
		/// <returns>true</returns>
		/// <c>false</c>
		public bool ShouldRenderChrome ()
		{
			return true;
		}

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
}

