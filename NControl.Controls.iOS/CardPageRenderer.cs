using System;
using Xamarin.Forms;
using NControl.Controls.iOS;
using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using UIKit;
using NControl.Controls;

[assembly: ExportRenderer(typeof(CardPage), typeof(CardPageRenderer))]
namespace NControl.Controls.iOS
{
	/// <summary>
	/// Renders a card page - ie a view with transparent background
	/// </summary>
	public class CardPageRenderer: PageRenderer
	{
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
		}			
	}
}

