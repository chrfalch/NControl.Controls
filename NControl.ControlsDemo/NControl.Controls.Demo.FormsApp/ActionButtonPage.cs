using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class ActionButtonPage: ContentPage
	{
		private Command _command;

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
			abtgl.SetBinding (IsToggledProperty, "IsToggled");
			layout.Children.Add(abtgl, () => new Rectangle((layout.Width/2)-(56/2), (layout.Height/2)-(56/2), 56, 56));

			_command = new Command ((obj) => {}, (obj) => abtgl.IsToggled);

			var playButton = new ActionButton{ 
				ButtonColor = Color.FromHex ("#2196F3"), 
				ButtonIcon = FontAwesomeLabel.FAPlay,
				Command = _command,
			};

			var abex = new ExpandableActionButton {
				ButtonColor = Color.FromHex("#FF9800"),
				Buttons = {
					playButton,
					new ActionButton{ ButtonColor = Color.FromHex("#009688"), ButtonIcon = FontAwesomeLabel.FATag},
					new ActionButton{ ButtonColor = Color.FromHex("#CDDC39"), ButtonIcon = FontAwesomeLabel.FARoad},
				}
			};
			layout.Children.Add(abex, () => new Rectangle(((layout.Width/4)*3)-(56/2), (layout.Height/2)-(200), 56, 250));
		}

		/// <summary>
		/// The IsToggled property.
		/// </summary>
		public static BindableProperty IsToggledProperty = 
			BindableProperty.Create<ActionButtonPage, bool> (p => p.IsToggled, false,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (ActionButtonPage)bindable;
					ctrl.IsToggled = newValue;
				});

		/// <summary>
		/// Gets or sets the IsToggled of the ActionButtonPage instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public bool IsToggled {
			get{ return (bool)GetValue (IsToggledProperty); }
			set {
				SetValue (IsToggledProperty, value);
				_command.ChangeCanExecute ();
			}
		}

	}
}

