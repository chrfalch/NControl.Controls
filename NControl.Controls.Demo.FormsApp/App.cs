using System;

using Xamarin.Forms;
using System.Reflection;
using System.Linq;

namespace NControl.Controls.Demo.FormsApp
{
	public class App : Application
	{
		public App ()
		{
			var ControlList = typeof(RoundCornerView).GetTypeInfo ().Assembly.DefinedTypes				
				.Where(t => !t.IsAbstract)
				.Select (t => t.Name).ToArray ();

			var listView = new ListView {
				ItemsSource = ControlList,
			};

			var startPage = new ContentPage {
				Title = "NControl.Controls",
				Content = listView
			};

			listView.ItemSelected += async (sender, e) => {

				if(listView.SelectedItem == null)
					return;
				
				// Create control
				var type = typeof(RoundCornerView).GetTypeInfo().Assembly.DefinedTypes.FirstOrDefault(
					t => t.Name.EndsWith((string)listView.SelectedItem)).AsType();
				
				var control = (View)Activator.CreateInstance(type);
				var demonstratable = control as IDemonstratableControl;
				if(demonstratable != null)
					demonstratable.Initialize();

				await startPage.Navigation.PushAsync(
					new ContentPage{
						Title = (string)listView.SelectedItem,
						Padding = 25,
						Content = control
					});							
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

