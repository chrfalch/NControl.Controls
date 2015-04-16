using System;
using Xamarin.Forms;

namespace NControl.Controls
{
	/// <summary>
	/// NGraphics extensions.
	/// </summary>
	public static class NGraphicsExtensions
	{
		/// <summary>
		/// Converts the Xamarin.Forms Color to an NGraphics Color
		/// </summary>
		/// <returns>The N color.</returns>
		/// <param name="color">Color.</param>
		public static NGraphics.Color ToNColor (this Color color)
		{
			return new NGraphics.Color (color.R, color.G, color.B, color.A);
		}

		/// <summary>
		/// Converts a Xamarin Rectangle to an NGraphics rectangle
		/// </summary>
		/// <returns>The N rect.</returns>
		/// <param name="rect">Rect.</param>
		public static NGraphics.Rect ToNRect(this Rectangle rect)
		{
			return new NGraphics.Rect (rect.X, rect.Y, rect.Width, rect.Height);
		}
	}
}

