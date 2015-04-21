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
	}
}

