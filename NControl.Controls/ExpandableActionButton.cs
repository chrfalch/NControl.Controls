using System;
using NControl.Abstractions;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NControl.Controls
{
	/// <summary>
	/// Expand direction.
	/// </summary>
	public enum ExpandDirection
	{
		Up,
		Down,
		Left,
		Right
	}

	/// <summary>
	/// Expandable action button.
	/// </summary>
	public class ExpandableActionButton: NControlView
	{
		#region Private Members

		/// <summary>
		/// The main button.
		/// </summary>
		private readonly ToggleActionButton _mainButton;

		/// <summary>
		/// The layout.
		/// </summary>
		private readonly RelativeLayout _layout;

		/// <summary>
		/// The buttons.
		/// </summary>
		private readonly ObservableCollection<ActionButton> _buttons = 
			new ObservableCollection<ActionButton>();

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.ExpandableActionButton"/> class.
		/// </summary>
		public ExpandableActionButton ()
		{
			// Set direction
			Direction = ExpandDirection.Up;

			// Layout
			_layout = new RelativeLayout ();
			Content = _layout;

			// Main button
			_mainButton = new ToggleActionButton {
				ButtonIcon = FontAwesomeLabel.FAEllipsisV,
			};

			AddButtonToLayout (_mainButton, _layout);

			// List of buttons
			_buttons.CollectionChanged += (sender, e) => UpdateButtons();

			_mainButton.PropertyChanged += (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if(e.PropertyName == ToggleActionButton.IsToggledProperty.PropertyName)
				{
					// Show/Hide buttons
					if(_mainButton.IsToggled)
						ShowButtons();
					else
						HideButtons();
				}
			};
		}

		#region Private Members

		/// <summary>
		/// Updates the buttons.
		/// </summary>
		private void UpdateButtons()
		{
			// Update buttons
			var toRemove = new List<ActionButton>();
			foreach(var button in _layout.Children)
			{				
				if (button == _mainButton)
					continue;
				
				var ac = button as ActionButton;
				ac.OnClicked -= HandleButtonClicked;
				toRemove.Add(ac);
			}

			foreach(var button in toRemove)
				_layout.Children.Remove(button);

			foreach(var button in Buttons)
			{
				if (!button.IsEnabled)
					continue;
				
				button.Opacity = 0.0;
				button.OnClicked += HandleButtonClicked;
				AddButtonToLayout (button, _layout);
			}				
		}

		/// <summary>
		/// Adds the main button to layout.
		/// </summary>
		/// <param name="layout">Layout.</param>
		private void AddButtonToLayout (ActionButton button, RelativeLayout layout)
		{			
			layout.Children.Add (button, () => 
				
				Direction == ExpandDirection.Up ? 
				// Up	
				new Rectangle(0, layout.Height-layout.Width, layout.Width, layout.Width) : 
				// Down
				Direction == ExpandDirection.Down ? 
					new Rectangle(0, 0, layout.Width, layout.Width) :
				// Left
				Direction == ExpandDirection.Left ? 
					new Rectangle(layout.Width-layout.Height, 0, layout.Height, layout.Height) :
				// Right
				new Rectangle(0, 0, layout.Height, layout.Height)

			);
				
		}

		/// <summary>
		/// Hides the buttons.
		/// </summary>
		private void HideButtons ()
		{
			Task.Run (() => 
				Device.BeginInvokeOnMainThread(async () => {

					var tasks = new List<Task>();
					foreach (var button in Buttons) {						
						button.HasShadow = false;
						tasks.Add(button.TranslateTo (0.0, 0.0, easing: Easing.CubicInOut));
					}

					await Task.WhenAll(tasks);

					foreach (var button in Buttons) {						
						button.FadeTo (0.0, 150,Easing.CubicInOut);
					}
				})
			);				
		}

		/// <summary>
		/// Shows the buttons.
		/// </summary>
		private void ShowButtons ()
		{
			UpdateButtons ();

			var c = 1;

			foreach (var button in Buttons) {
				if (!button.IsEnabled)
					continue;	
				
				button.HasShadow = true;
				button.FadeTo(1.0, 100);
				button.TranslateTo (0.0, -(16+40) * c++, easing: Easing.SpringIn);
			}
		}

		/// <summary>
		/// Handles the button clicked.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void HandleButtonClicked(object sender, EventArgs args)
		{
			_mainButton.IsToggled = false;
			HideButtons ();
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the direction of expansion
		/// </summary>
		/// <value>The direction.</value>
		public ExpandDirection Direction {
			get;
			set;
		}

		/// <summary>
		/// The button color property.
		/// </summary>
		public static BindableProperty ButtonColorProperty = 
			BindableProperty.Create<ExpandableActionButton, Color> (p => p.ButtonColor, Color.Gray,
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
					var ctrl = (ExpandableActionButton)bindable;
					ctrl.ButtonColor = newValue;
				});

		/// <summary>
		/// Gets or sets the color of the buton.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color ButtonColor
		{
			get {  return (Color)GetValue (ButtonColorProperty);}
			set {
				SetValue (ButtonColorProperty, value);
				_mainButton.ButtonColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the color of the buton.
		/// </summary>
		/// <value>The color of the buton.</value>
		public ObservableCollection<ActionButton> Buttons
		{
			get {  return _buttons; }
		}
			
		#endregion
	}
}

