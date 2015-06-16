using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class CardPageDemo: ContentPage
	{
		public CardPageDemo ()
		{
			Title = "CardPage";

			Content = new StackLayout{
				Padding = 24,
				Children = {
					new Button {
						BackgroundColor = Color.Transparent,
						Text = "Show",
						Command = new Command(async (obj)=> {

							var button = new Button
							{
								Text = "Close"
							};

							var label = new Label {
								XAlign = TextAlignment.Center,
								YAlign = TextAlignment.Center 
							};

							label.SetBinding (Label.TextProperty, "Title");

							var page = new CardPage{

								RequestedHeight = 180,
								RequestedWidth = 250,

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
								}
							};

							page.BindingContext = this.BindingContext;
							var currentApp = Xamarin.Forms.Application.Current;
							button.Clicked += async (sender, e) => {
								await page.CloseAsync();
								if(currentApp != Xamarin.Forms.Application.Current)
									throw new InvalidOperationException("Application.Current changed");
							};
								
							await page.ShowAsync();
						})		
					}
				}
			};				
		}
	}
}

