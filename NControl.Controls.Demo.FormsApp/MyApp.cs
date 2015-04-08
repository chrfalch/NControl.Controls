using System;
using NControl.Abstractions;
using NGraphics;
using Xamarin.Forms;
using System.Reflection;
using System.Linq;

namespace NControl.Controls.Demo.FormsApp
{
	public class MyApp : Application
	{
		public MyApp ()
		{
			var demoPageList = new ContentPage[] {
				new RoundCornerViewPage(),
				new FontAwesomeLabelPage(),
				new FloatingLabelPage(),
			};

			var listView = new ListView {
				ItemsSource = demoPageList,
			};

			listView.ItemTemplate = new DataTemplate (typeof(TextCell));
			listView.ItemTemplate.SetBinding (TextCell.TextProperty, "Title");

			var startPage = new ContentPage {
				Title = "NControl.Controls",
                Content = new StackLayout
                {
                    Children =
                    {
						new Label()
                        {
                            HeightRequest = 55,
							Text = "Custom Font",
							FontFamily = "Clink Clank",
							BackgroundColor = Xamarin.Forms.Color.White,
							XAlign = Xamarin.Forms.TextAlignment.Center,
							YAlign = Xamarin.Forms.TextAlignment.Center,
							TextColor = Xamarin.Forms.Color.Blue,
							FontSize = 24,
                        },
                        listView
                    }
                }
			};

			listView.ItemSelected += async (sender, e) => {

				if(listView.SelectedItem == null)
					return;
				
				// Show page
				await startPage.Navigation.PushAsync(listView.SelectedItem as ContentPage);
					
			};

			listView.ItemTapped += (sender, e) => listView.SelectedItem = null;
				
			// The root page of your application
			MainPage = new NavigationPage(startPage);
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

