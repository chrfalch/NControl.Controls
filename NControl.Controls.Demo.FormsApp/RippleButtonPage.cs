using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	
	public class RippleButtonPage: ContentPage
	{
		public RippleButtonPage ()
		{
			Title = "RippleButton";
		    BackgroundColor = Color.White;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			var label = new Label{ 
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,			
			};

			var i = 0;
			var incCommand = new Command ((obj) => {
				label.Text = "Count number: " + (i++).ToString();
			});

			var button = new RippleButton {
				Text = "Click me!",
				BorderColor = Color.Gray,
				BorderWidth = 2.0,
				CornerRadius = 22,
				Icon = FontAwesomeLabel.FACar,
				Command = incCommand
			};

			var button2 = new RippleButton {
				Text = "And then click me!",
				BorderColor = Color.Red,
				BackgroundColor = Color.Aqua,
				BorderWidth = 4.0,
				CornerRadius = 4,
				RippleColor = Color.Blue,
				Command = incCommand
			};

			var button3= new RippleButton {
				Text = "What about me?",
				Icon = FontAwesomeLabel.FATerminal,
				ImagePosition = ImagePosition.Top,
				BorderColor = Color.Gray,
				BorderWidth = 2.0,
				CornerRadius = 22,
				Command = incCommand
			};

			var button4= new RippleButton {
				Text = "Yup?",
				Icon = FontAwesomeLabel.FAHome,
				ImagePosition = ImagePosition.Top,
				BorderColor = Color.Gray,
				BorderWidth = 2.0,
				CornerRadius = 22,
				Command = incCommand
			};

			var layout = new RelativeLayout ();
			layout.Children.Add (button, () => new Rectangle (50, 100, layout.Width - 100, 44));
			layout.Children.Add (button2, () => new Rectangle (50, 160, layout.Width - 100, 44));
			layout.Children.Add (button3, () => new Rectangle (20, 220, layout.Width - 200, 150));
			layout.Children.Add (button4, () => new Rectangle (210, 220, 120, 75));
			layout.Children.Add (label, () => new Rectangle (10, 400, layout.Width - 20, 40));

			Content = layout;
		}
	}
}

