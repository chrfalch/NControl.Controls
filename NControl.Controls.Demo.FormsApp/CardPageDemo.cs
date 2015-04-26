using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class CardPageDemo: CardPage
	{
		public CardPageDemo ()
		{
			Title = "CardPage";
			RequestedHeight = 180;
			RequestedWidth = 250;

			var button = new Button{Text = "Close"};
			button.Clicked += async (sender, e) => await Navigation.PopModalAsync();

			var label = new Label {
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center 
			};

			label.SetBinding (Label.TextProperty, "Title");

			Content = new StackLayout {
				Padding = 22,
				Orientation = StackOrientation.Vertical,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					label,
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

