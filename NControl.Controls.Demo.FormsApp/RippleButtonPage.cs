using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	
	public class RippleButtonPage: ContentPage
	{
		public RippleButtonPage ()
		{
			Title = "RippleButton";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			var button = new RippleButton {
				Text = "Click me!",
				BorderColor = Color.Gray,
				BorderWidth = 2.0,
				CornerRadius = 22,
				Icon = FontAwesomeLabel.FACar,
			};

			var button2 = new RippleButton {
				Text = "And then click me!",
				BorderColor = Color.Red,
				BackgroundColor = Color.Aqua,
				BorderWidth = 4.0,
				CornerRadius = 4,
				RippleColor = Color.Blue,
			};

			var button3= new RippleButton {
				Text = "What about me?",
				BorderColor = Color.Gray,
				BorderWidth = 2.0,
				CornerRadius = 22,
			};

			var layout = new RelativeLayout ();
			layout.Children.Add (button, () => new Rectangle (50, 100, layout.Width - 100, 44));
			layout.Children.Add (button2, () => new Rectangle (50, 160, layout.Width - 100, 44));
			layout.Children.Add (button3, () => new Rectangle (100, 220, layout.Width - 200, 150));

			Content = layout;
		}
	}
}

