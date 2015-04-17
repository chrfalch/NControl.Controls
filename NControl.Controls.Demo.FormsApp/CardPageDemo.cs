using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class CardPageDemo: CardPage
	{
		public CardPageDemo ()
		{
			Title = "CardPage";
			var button = new Button{Text = "Close"};
			button.Clicked += async (sender, e) => await HideCardPageAsync();

			Content = new StackLayout {
				Padding = 22,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new Label{ 
						Text = "This is a page",
						XAlign = TextAlignment.Center,
						YAlign = TextAlignment.Center 
					},

					button,
				}
			};					
					
		}
	}
}

