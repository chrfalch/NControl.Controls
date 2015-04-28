using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class TabStripPage: ContentPage
	{
		public TabStripPage ()
		{
			Title = "TabStrip";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			Content = new TabStripControl {
				Children = {
					new TabItem ("test 1", GetTabPage (Color.Green)),
					new TabItem ("test 2", GetTabPage (Color.Red)),
					new TabItem ("test 3", GetTabPage (Color.Blue))
				},
			};
		}

		View GetTabPage (Color clr)
		{
			var retVal = new StackLayout {
				BackgroundColor = clr,
				Children = {
						new RippleButton{
						Text = "Tastelisten 12 234 345 ",
						BorderWidth = 4,
						BorderColor = Color.Aqua,
						CornerRadius = 4,
						HeightRequest = 55,
					}
				}
			};

			return retVal;
		}
	}
}

