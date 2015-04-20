using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NControl.Controls
{
	/// <summary>
	/// I card page helper.
	/// </summary>
	public interface ICardPageHelper
	{
		/// <summary>
		/// Shows the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="card">Card.</param>
		Task ShowAsync(CardPage card);

		/// <summary>
		/// Hides the async. Use if you will be displaying again
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="card">Card.</param>
		Task HideAsync(CardPage card);

		/// <summary>
		/// Closes the card.
		/// </summary>
		Task CloseAsync(CardPage card);

		/// <summary>
		/// Shoulds the render chrome.
		/// </summary>
		/// <returns><c>true</c>, if render chrome was shoulded, <c>false</c> otherwise.</returns>
		bool ShouldRenderChrome();

		/// <summary>
		/// Gets the size of the screen.
		/// </summary>
		/// <returns>The screen size.</returns>
		Size GetScreenSize();
	}
}

