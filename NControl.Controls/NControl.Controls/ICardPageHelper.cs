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
		/// Gets the size of the screen.
		/// </summary>
		/// <returns>The screen size.</returns>
		Size GetScreenSize();

		/// <summary>
		/// Shows the card async.
		/// </summary>
		/// <returns>The async.</returns>
		Task ShowAsync(CardPage page);

		/// <summary>
		/// Closes the card async
		/// </summary>
		/// <returns>The aync.</returns>
		Task CloseAsync (CardPage page);

		/// <summary>
		/// Gets a value indicating whether this <see cref="NControl.Controls.ICardPageHelper"/> control animates itself.
		/// </summary>
		/// <value><c>true</c> if control animates itself; otherwise, <c>false</c>.</value>
		bool ControlAnimatesItself { get; }
	}		
}

