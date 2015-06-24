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
						Command = ShowCardPageCommand
					},
					new Button {
						Text = "Show from Modal Page",
						Command = new Command(async () => {
							
							await Application.Current.MainPage.Navigation.PushModalAsync(new ContentPage{
								Content = new StackLayout{
									Children = {
										new Button{
											VerticalOptions = LayoutOptions.Center,
											HorizontalOptions = LayoutOptions.Center,
											HeightRequest = 44,
											Text = "Show Card",
											Command = ShowCardPageCommand
										},
										new Button{
											VerticalOptions = LayoutOptions.Center,
											HorizontalOptions = LayoutOptions.Center,
											HeightRequest = 44,
											Text = "Close",
											Command = new Command(async () => {
												await Application.Current.MainPage.Navigation.PopModalAsync();
											})
										}
									}
								}
							});

						})
					}
				}
			};				
		}

		/// <summary>
		/// Gets the show card page command.
		/// </summary>
		/// <value>The show card page command.</value>
		private Command ShowCardPageCommand {
			get{
				return new Command (async (obj) => {
					var showModalViewButton = new Button {
						Text = "Show Modal View"
					};

					var closeCardButton = new Button {
						Text = "Close"
					};

					var label = new Label {
						XAlign = TextAlignment.Center,
						YAlign = TextAlignment.Center 
					};

					label.SetBinding (Label.TextProperty, "Title");

					var page = new CardPage {

						RequestedHeight = 240,
						RequestedWidth = 250,

						Content = new StackLayout {
							Padding = 22,
							BackgroundColor = Color.Yellow,
							Orientation = StackOrientation.Vertical,
							HorizontalOptions = LayoutOptions.FillAndExpand,
							VerticalOptions = LayoutOptions.CenterAndExpand,
							Children = {
								label,
								new Label { 
									Text = "This is a page",
									XAlign = TextAlignment.Center,
									YAlign = TextAlignment.Center 
								},
								showModalViewButton,
								closeCardButton,
							}
						}
					};

					page.BindingContext = this.BindingContext;
					var currentApp = Xamarin.Forms.Application.Current;
					closeCardButton.Clicked += async (sender, e) => {
						await page.CloseAsync ();
						if (currentApp != Xamarin.Forms.Application.Current)
							throw new InvalidOperationException ("Application.Current changed");
					};

					showModalViewButton.Clicked += async (sender, e) => {

						// Push modal 
						await Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync (new ContentPage {
							Content = new StackLayout {
								Children = {
									new Label {
										Text = "Hello",
									},
									new Button {
										Text = "Close",
										Command = new Command (async () => 
											await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync ())
									}
								}									
							}
						});	
					};

					await page.ShowAsync ();
				});					
			}
		}
	}
}

