using NControl.Abstractions;
using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class CardPageDemo: ContentPage
	{
        public CardPageDemo()
        {
            Title = "CardPage";

            var raiseExceptionButton = new NControlView
            {
                BackgroundColor = Color.Red,
                HeightRequest = 300,
                Content = new Label { Text = "Click to raise exception" },
            };

            raiseExceptionButton.OnTouchesBegan += (s, e) => { throw new InvalidOperationException("Whoopps"); };

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
							
							await Application.Current.MainPage.Navigation.PushModalAsync(
								new NavigationPage(
									new ContentPage{
										Title = "Navigation",
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
									})
							);

						})
					},
                    raiseExceptionButton
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

                    var showAlert = new Button {
                        Text = "Show Alert",
                        Command = new Command(()=> DisplayAlert("NControl.Controls", "Welcome to the popup!", "Cancel"))
                    };

					var closeCardButton = new RippleButton {
						Text = "Close",
                        BorderColor = Color.Blue,
                        BorderWidth = 1,
                        CornerRadius = 4,
                        HeightRequest = 44,
					};

					var label = new Label {
						XAlign = TextAlignment.Center,
						YAlign = TextAlignment.Center 
					};

                    label.SetBinding (Label.TextProperty, "Title");

                    var touchCount = 1;
                    var touchLabel = new Label { Text = "Click me: " + touchCount.ToString() };
                    var touchButton = new NControlView { HeightRequest = 65, Content = touchLabel };
                    touchButton.OnTouchesBegan += (s, e) => {
                        touchCount++;
                        touchLabel.Text = "Click me: " + touchCount.ToString();
                    };

                    var page = new CardPage {

						RequestedHeight = 320,
						RequestedWidth = 250,

						Content = new StackLayout {
							Padding = 22,
							BackgroundColor = Color.Aqua,
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
                                touchButton,
                                closeCardButton,
							}
						}
					};

					page.BindingContext = this.BindingContext;
					var currentApp = Xamarin.Forms.Application.Current;
					closeCardButton.Command = new Command( async () => {
						await page.CloseAsync ();
						if (currentApp != Xamarin.Forms.Application.Current)
							throw new InvalidOperationException ("Application.Current changed");
					});

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

