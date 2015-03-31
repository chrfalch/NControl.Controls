using System;
using NControl.Abstractions;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace NControl.Controls
{
	/// <summary>
	/// Float label control.
	/// </summary>
	public class FloatingLabelControl: NControlView, IDemonstratableControl
	{
		#region Private Members

		/// <summary>
		/// The floating label.
		/// </summary>
		private Label _floatingLabel;

		/// <summary>
		/// The text entry.
		/// </summary>
		private Entry _textEntry;

        /// <summary>
        /// The post fix.
        /// </summary>
        private Label _postFix;

        /// <summary>
        /// The layout.
        /// </summary>
        private RelativeLayout _layout;

		#endregion

		#region IDemonstratableControl implementation

		/// <summary>
		/// Initialize this instance.
		/// </summary>
		public void Initialize ()
		{
			Placeholder = "My Placeholder";
		}

		#endregion

        #region Events

        /// <summary>
        /// Occurs when the element receives focus.
        /// </summary>
        /// <remarks>Focused event is raised whenever the VisualElement receives focus. This event is not bubbled through the
        /// Forms stack and is received directly from the native control. This event is emitted by the IsFocusedProperty setter.</remarks>
        /// <altmember cref="P:Xamarin.Forms.VisualElement.IsFocused"></altmember>
        /// <altmember cref="M:Xamarin.Forms.VisualElement.Focus()"></altmember>
        public new event System.EventHandler<FocusEventArgs> Focused;

        /// <summary>
        /// Occurs when the element loses focus.
        /// </summary>
        /// <remarks>Unfocused event is raised whenever the VisualElement loses focus. This event is not bubbled through the
        /// Forms stack and is received directly from the native control. This event is emitted by the IsFocusedProperty setter.</remarks>
        public new event System.EventHandler<FocusEventArgs> Unfocused;

        #endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="FloatingLabelDemo.FloatLabelControl"/> class.
		/// </summary>
		public FloatingLabelControl ()
		{
            HeightRequest = Device.OnPlatform<int>(50, 50, 85); ;

			// Create placeholder label
			_floatingLabel = new Label {
				BackgroundColor = Color.Transparent,
				Text = Placeholder,
				FontAttributes = FontAttributes.Bold,
				XAlign = TextAlignment.Start,
				YAlign = TextAlignment.Center,
				FontSize = 10,
				Opacity = 0.0,
				TextColor = Color.FromHex("#BBBBBB")
			};

			// Create textfield
			_textEntry = new ExtendedEntry { 
				Keyboard = this.Keyboard,
				BackgroundColor = Color.Transparent,
			};
				
            _textEntry.Focused += (object sender, FocusEventArgs e) =>
            {
                _floatingLabel.TextColor = PlaceholderColor;
                if(Focused != null)
                    Focused(this, e);
            };
                
            _textEntry.Unfocused += (object sender, FocusEventArgs e) =>
            {
                _floatingLabel.TextColor = Color.FromHex("#BBBBBB");
                if(Unfocused != null)
                    Unfocused(this, e);
            };

			_textEntry.TextChanged += TextEntry_TextChanged;

            // Postfix
            _postFix = new Label{
                BackgroundColor = Color.Transparent,
                Text = Postfix,
                XAlign = TextAlignment.End,
                YAlign = TextAlignment.Center,
                TextColor = Color.FromHex("#BBBBBB")
            };

			// Create layout
            _layout = new RelativeLayout { IsClippedToBounds = true };

            var postfixWidth = 40;

			// Position label
		 	_layout.Children.Add (_floatingLabel, () => new Rectangle(10, 14, _layout.Width-20, _layout.Height-14));

            // Position postfix
            _layout.Children.Add (_postFix, () => new Rectangle(_layout.Width-postfixWidth, 
                Device.OnPlatform<int>(12, 12, 12), postfixWidth, _layout.Height-12));

			// Position text field
			_layout.Children.Add (_textEntry, () => 
				new Rectangle(Device.OnPlatform<int>(10, -2, 10), 
					Device.OnPlatform<int>(12, 12, 12), _layout.Width-(10 +
						(string.IsNullOrWhiteSpace(_postFix.Text) ? 0 : postfixWidth)), 
					_layout.Height - Device.OnPlatform<int>(12, 12, 2)));

			Content = _layout;
		}

		#region Text Handling

		/// <summary>
		/// Texts the entry text changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void TextEntry_TextChanged (object sender, TextChangedEventArgs e)
		{
			double startY, stopY;
			double startAlpha, stopAlpha;

			if (string.IsNullOrWhiteSpace (e.OldTextValue) && !string.IsNullOrWhiteSpace (e.NewTextValue)) {
				// We have text - move label out
				startY = -12;
				stopY = -18;
				startAlpha = 0.0;
				stopAlpha = 1.0;

			} else if (!string.IsNullOrWhiteSpace (e.OldTextValue) && string.IsNullOrWhiteSpace (e.NewTextValue)) {
				// Text is empty, move label in
				startY = -18;
				stopY = -12;
				startAlpha = 1.0;
				stopAlpha = 0.0;
			} 
			else
				return;

			// Animate y position
			var yAnimation = new Animation ((d) => _floatingLabel.TranslationY = d,
				startY, stopY, Easing.CubicOut);

			var alphaAnimation = new Animation ((d) => _floatingLabel.Opacity = d,
				startAlpha, stopAlpha, Easing.CubicOut);
			
			yAnimation.WithConcurrent (alphaAnimation);
			yAnimation.Commit (_floatingLabel, "AnimateLabel");
		}

		#endregion

		#region Properties

		/// <summary>
		/// The keyboard property.
		/// </summary>
		public static BindableProperty KeyboardProperty = 
			BindableProperty.Create<FloatingLabelControl, Keyboard> (p => p.Keyboard, Xamarin.Forms.Keyboard.Default,
				BindingMode.Default, null, (bindable, oldValue, newValue) => 
				(bindable as FloatingLabelControl)._textEntry.Keyboard = newValue
			);

		/// <summary>
		/// Gets or sets the keyboard type.
		/// </summary>
		/// <value>The placeholder.</value>
		public Xamarin.Forms.Keyboard Keyboard
		{
			get { return (Xamarin.Forms.Keyboard)GetValue (KeyboardProperty); }
			set { SetValue (KeyboardProperty, value); }
		}

		/// <summary>
		/// The placeholder property.
		/// </summary>
		public static BindableProperty PlaceholderProperty = 
			BindableProperty.Create<FloatingLabelControl, string> (p => p.Placeholder, string.Empty,
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
                var ctrl = (FloatingLabelControl)bindable;
                ctrl.Placeholder = newValue;
            });

		/// <summary>
		/// Gets or sets the placeholder.
		/// </summary>
		/// <value>The placeholder.</value>
		public string Placeholder
		{
			get { return (string)GetValue (PlaceholderProperty); }
			set 
            { 
                SetValue (PlaceholderProperty, value); 
                _textEntry.Placeholder = value;
                _floatingLabel.Text = value;
            }
		}

		/// <summary>
		/// The text property.
		/// </summary>
		public static BindableProperty TextProperty = 
			BindableProperty.Create<FloatingLabelControl, string>(p => p.Text, string.Empty,
                propertyChanged: (bindable, oldValue, newValue) =>{
                var ctrl = (FloatingLabelControl)bindable;
                ctrl.Text = newValue;
            });
		

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
            get { return (string)GetValue (TextProperty); }
            set 
            { 
                SetValue (TextProperty, value); 
                _textEntry.Text = value;
            }
		}

        /// <summary>
        /// The postfix property.
        /// </summary>
        public static BindableProperty PostfixProperty = 
            BindableProperty.Create<FloatingLabelControl, string>(p => p.Postfix, string.Empty,
                propertyChanged: (bindable, oldValue, newValue) =>{
                var ctrl = (FloatingLabelControl)bindable;
                ctrl.Postfix = newValue;
            });


        /// <summary>
        /// Gets or sets the Postfix.
        /// </summary>
        /// <value>The text.</value>
        public string Postfix
        {
            get { return (string)GetValue (PostfixProperty); }
            set 
            { 
                SetValue (PostfixProperty, value); 
                _postFix.Text = value;
                _layout.ForceLayout();
            }
        }

		/// <summary>
		/// The placeholder color property.
		/// </summary>
		public static BindableProperty PlaceholderColorProperty = 
			BindableProperty.Create<FloatingLabelControl, Color>(p => p.PlaceholderColor, Color.Blue,
                BindingMode.Default, null, (bindable, oldValue, newValue) => {
                var ctrl = (FloatingLabelControl)bindable;
                ctrl.PlaceholderColor = newValue;	
            });


		/// <summary>
		/// Gets or sets the placeholder color.
		/// </summary>
		/// <value>The text.</value>
		public Color PlaceholderColor
		{
			get { return (Color)GetValue (PlaceholderColorProperty); }
			set
            { 
                SetValue(PlaceholderColorProperty, value); 
                _floatingLabel.TextColor = value;
            }
		}

		/// <summary>
		/// The is password property.
		/// </summary>
		public static readonly BindableProperty IsPasswordProperty = 
			BindableProperty.Create<FloatingLabelControl, bool> (p => p.IsPassword, false, 
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
				var ctrl = (FloatingLabelControl)bindable;
				ctrl.IsPassword = newValue;
			});

		//
		// Properties
		//
		public bool IsPassword 
		{
			get { return (bool)base.GetValue (Entry.IsPasswordProperty); }
			set 
			{
				base.SetValue (Entry.IsPasswordProperty, value);
				_textEntry.IsPassword = value;
			}
		}

		#endregion       
	}
}

