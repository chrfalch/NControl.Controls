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
		private readonly RelativeLayout _buttonsLayout;

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
			Content = new RelativeLayout();

			// Main button
			_mainButton = new ToggleActionButton {
				ButtonIcon = FontAwesomeLabel.FAEllipsisV,
			};
				
			// Create buttons layout
			_buttonsLayout = new RelativeLayout ();
			(Content as RelativeLayout).Children.Add (_buttonsLayout, () => (Content as RelativeLayout).Bounds);

			AddButtonToLayout (_mainButton, Content as RelativeLayout);

			_mainButton.PropertyChanged += async (object sender, System.ComponentModel.PropertyChangedEventArgs e) => {
				if(e.PropertyName == ToggleActionButton.IsToggledProperty.PropertyName)
				{
					// Show/Hide buttons
					if(_mainButton.IsToggled)
						await ShowButtonsAsync();
					else
						await HideButtonsAsync();
				}
			};
		}

		#region Private Members

		/// <summary>
		/// Adds buttons.
		/// </summary>
		private void AddButtons()
		{
			// Update buttons
			foreach(var button in Buttons)
			{
				if (!button.IsEnabled)
					continue;

				button.Opacity = 0.0;
				button.OnClicked += HandleButtonClicked;
				AddButtonToLayout (button, _buttonsLayout);
			}				

			_buttonsLayout.ForceLayout ();
		}

		/// <summary>
		/// Removes buttons.
		/// </summary>
		private void RemoveButtons()
		{
			// Update buttons
			var toRemove = new List<ActionButton>();
			foreach(var button in _buttonsLayout.Children)
			{				
				var ac = button as ActionButton;
				ac.OnClicked -= HandleButtonClicked;
				toRemove.Add(ac);
			}

			foreach(var button in toRemove)
				_buttonsLayout.Children.Remove(button);

			_buttonsLayout.ForceLayout ();
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
		private async Task HideButtonsAsync ()
		{
			var tasks = new List<Task>();
			foreach (var button in Buttons) {						
				button.HasShadow = false;
				tasks.Add(button.TranslateTo (0.0, 0.0, easing: Easing.CubicInOut));
			}

			await Task.WhenAll(tasks);

			tasks.Clear ();
			foreach (var button in Buttons) {						
				tasks.Add (button.FadeTo (0.0, 350, Easing.CubicInOut));
			}

			await Task.WhenAll(tasks);

			RemoveButtons ();
		}

		/// <summary>
		/// Shows the buttons.
		/// </summary>
		private async Task ShowButtonsAsync ()
		{
			AddButtons ();

			var tasks = new List<Task>();

			var c = 1;

			foreach (var button in Buttons) {
				if (!button.IsEnabled)
					continue;	
				
				button.HasShadow = true;
				tasks.Add (button.FadeTo (1.0, 50));
				tasks.Add(button.TranslateTo (0.0, -(16+40) * c++, easing: Easing.SpringIn));
			}			

			await Task.WhenAll(tasks);
		}

		/// <summary>
		/// Handles the button clicked.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void HandleButtonClicked(object sender, EventArgs args)
		{
			_mainButton.IsToggled = false;
			HideButtonsAsync ();
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

