using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class ActionButtonPage: ContentPage
	{
		public ActionButtonPage ()
		{
			Title = "ActionButton";
			BackgroundColor = Color.White;
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			var layout = new RelativeLayout ();
			Content = layout;

			var ab = new ActionButton {
				ButtonColor = Color.FromHex("#E91E63"),
				ButtonIcon = FontAwesomeLabel.FAThumbsUp,
			};
			layout.Children.Add(ab, () => new Rectangle((layout.Width/4)-(56/2), (layout.Height/2)-(56/2), 56, 56));

			var abtgl = new ToggleActionButton {
				ButtonColor = Color.FromHex("#FF5722"),
				ButtonIcon = FontAwesomeLabel.FAPlus,
			};
			layout.Children.Add(abtgl, () => new Rectangle((layout.Width/2)-(56/2), (layout.Height/2)-(56/2), 56, 56));

			var abex = new ExpandableActionButton {
				ButtonColor = Color.FromHex("#FF9800"),
				Buttons = {
					new ActionButton{ ButtonColor = Color.FromHex("#2196F3"), ButtonIcon = FontAwesomeLabel.FAPlay},
					new ActionButton{ ButtonColor = Color.FromHex("#009688"), ButtonIcon = FontAwesomeLabel.FATag},
					new ActionButton{ ButtonColor = Color.FromHex("#CDDC39"), ButtonIcon = FontAwesomeLabel.FARoad},
				}
			};
			layout.Children.Add(abex, () => new Rectangle(((layout.Width/4)*3)-(56/2), (layout.Height/2)-(56/2), 56, (layout.Height/4)-48));
		}
	}
}

