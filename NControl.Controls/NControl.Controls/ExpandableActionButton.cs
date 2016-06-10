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
	public class ExpandableActionButton: ContentView
	{
		#region Private Members

		/// <summary>
		/// The buttons added.
		/// </summary>
		private bool _buttonsAdded = false;

        /// <summary>
        /// Flag showing submenu buttons
        /// </summary>
	    private bool _isShowingSubmenu = false;

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
			if (_buttonsAdded)
				return;

			_buttonsAdded = true;

			// Update buttons
			foreach(var button in Buttons)
			{
				button.Opacity = 0.0;
                button.Command = new Command(async () => {

                    if (!_isShowingSubmenu)
				        return;

                    _mainButton.IsToggled = false;
					await HideButtonsAsync ();
                });
                
				AddButtonToLayout (button, _buttonsLayout);
			}				

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
		    if (!_isShowingSubmenu)
		        return;

		    _isShowingSubmenu = false;

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
		}

		/// <summary>
		/// Shows the buttons.
		/// </summary>
		private async Task ShowButtonsAsync ()
		{
			AddButtons ();

		    if (_isShowingSubmenu)
		        return;

			var tasks = new List<Task>();

			var c = 1;

			foreach (var button in Buttons) 
			{				
				button.Opacity = 0.0;

				if ((button.Command != null &&
				    button.Command.CanExecute (button.CommandParameter) == false)) 
					continue;
				
				button.HasShadow = true;
				tasks.Add (button.FadeTo (1.0, 50));
				tasks.Add(button.TranslateTo (0.0, -(ButtonPadding+_mainButton.Height) * c++, easing: Easing.SpringIn));
			}			

			await Task.WhenAll(tasks);

            _isShowingSubmenu = true;
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
		/// The ButtonPadding property.
		/// </summary>
		public static BindableProperty ButtonPaddingProperty = 
			BindableProperty.Create(nameof(ButtonPadding), typeof(double), typeof(ExpandableActionButton), 0,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (ExpandableActionButton)bindable;
					ctrl.ButtonPadding = (double)newValue;
				});

		/// <summary>
		/// Gets or sets the ButtonPadding of the ExpandableActionButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public double ButtonPadding {
			get{ return (double)GetValue (ButtonPaddingProperty); }
			set {
				SetValue (ButtonPaddingProperty, value);
			}
		}
		/// <summary>
		/// The button color property.
		/// </summary>
		public static BindableProperty ButtonColorProperty = 
			BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(ExpandableActionButton), Color.Gray,
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
					var ctrl = (ExpandableActionButton)bindable;
					ctrl.ButtonColor = (Color)newValue;
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

