using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class CardPageDemo: CardPage
	{
		public CardPageDemo ()
		{
			Title = "CardPage";
			Content = new Label{ 
				Text = "This is a page",
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center 
			};
		}
	}
}

