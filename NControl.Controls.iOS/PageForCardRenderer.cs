using System;
using Xamarin.Forms;
using NControl.Controls.iOS;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using UIKit;

[assembly: ExportRenderer(typeof(Page), typeof(PageForCardRenderer))]
namespace NControl.Controls.iOS
{
	/// <summary>
	/// Renders a card page - ie a view with transparent background
	/// </summary>
	public class PageForCardRenderer: PageRenderer
	{
		public PageForCardRenderer ()
		{
		}
			

	}
}

