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
				new CustomFontPage(),
				new RoundCornerViewPage(),
				new FontAwesomeLabelPage(),
				new FloatingLabelPage(),
				new ActionButtonPage(),
				new CardPageDemo{BindingContext = new ViewModel("CardPage")},
				new BlurViewPage(),
				new GalleryPage(),
				new PagingViewPage(),
				new RippleButtonPage(),
				new MaterialDesignIconsPage(),
			};

			BindingContext = new ViewModel("Should say CardPage");

			var listView = new ListView {
				ItemsSource = demoPageList,
			};

			listView.ItemTemplate = new DataTemplate (typeof(TextCell));
			listView.ItemTemplate.SetBinding (TextCell.TextProperty, "Title");

			var contentLayout = new RelativeLayout ();
			contentLayout.Children.Add (listView, () => contentLayout.Bounds);

			var startPage = new ContentPage {
				Title = "NControl.Controls",
				Content = contentLayout              
			};

			listView.ItemSelected += async (sender, e) => {

				if(listView.SelectedItem == null)
					return;
				
				// Show page
				await startPage.Navigation.PushAsync(listView.SelectedItem as Page);					
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

	public class ViewModel
	{
		public ViewModel(string title)
		{
			Title = title;
		}

		public string Title {
			get;
			private set;
		}

		public override string ToString ()
		{
			return string.Format ("[ViewModel: Title={0}]", Title);
		}
	}
}

