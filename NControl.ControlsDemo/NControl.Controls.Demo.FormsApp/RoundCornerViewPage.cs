using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class RoundCornerViewPage: ContentPage
	{
		public RoundCornerViewPage ()
		{
			Title = "RoundCornerView";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
		
			var layout = new RelativeLayout ();

			var border1 = new RoundCornerView {
				BackgroundColor = Color.Red,
				CornerRadius = 34,
                BorderColor = Color.Yellow,
                BorderWidth = 18,
            };

			var border2 = new RoundCornerView {
				BackgroundColor = Color.Blue,
				CornerRadius = 14,
			};

            var border3 = new RoundCornerView {
				BackgroundColor = Color.Green,
				CornerRadius = 12,
                BorderColor = Color.Red,
                BorderWidth = 4,                
			};

			var border4 = new RoundCornerView {
				BackgroundColor = Color.Yellow,
				CornerRadius = 42,
			};

            var shape = new BoxView { BackgroundColor = Color.Aqua };
            var shapeLayout = new RelativeLayout();
            shapeLayout.Children.Add(shape, () => new Rectangle(-50, -30, 100, 100));
            border1.Content = shapeLayout;

            layout.Children.Add (border1, () => new Rectangle (10, 10, (layout.Width / 2) - 20, (layout.Height / 2) - 20));
			layout.Children.Add (border2, () => new Rectangle ((layout.Width / 2) + 10, 10, (layout.Width / 2) - 20, (layout.Height / 2) - 20));
			layout.Children.Add (border3, () => new Rectangle (10, (layout.Height / 2) + 10, (layout.Width / 2) - 20, (layout.Height / 2) - 20));
			layout.Children.Add (border4, () => new Rectangle ((layout.Width / 2) + 10, (layout.Height / 2) + 10, (layout.Width / 2) - 20, (layout.Height / 2) - 20));

			Content = layout;
		}
	}
}

