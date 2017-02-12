/************************************************************************
 * 
 * The MIT License (MIT)
 * 
 * Copyright (c) 2025 - Christian Falch
 * 
 * Permission is hereby granted, free of charge, to any person obtaining 
 * a copy of this software and associated documentation files (the 
 * "Software"), to deal in the Software without restriction, including 
 * without limitation the rights to use, copy, modify, merge, publish, 
 * distribute, sublicense, and/or sell copies of the Software, and to 
 * permit persons to whom the Software is furnished to do so, subject 
 * to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be 
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
 * IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
 * CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
 * TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 * 
 ************************************************************************/

using System;
using NControl.Abstractions;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace NControl.Controls
{
	/// <summary>
	/// Float label control.
	/// </summary>
	public class FloatingLabelControl: NControlView
	{
		#region Private Members

		/// <summary>
		/// The underline control.
		/// </summary>
		private readonly BoxView _underlineControl;

		/// <summary>
		/// The floating label.
		/// </summary>
		private readonly Label _floatingLabel;

		/// <summary>
		/// The text entry.
		/// </summary>
		private readonly ExtendedEntry _textEntry;

        /// <summary>
        /// The post fix.
        /// </summary>
        private readonly Label _postFix;

        /// <summary>
        /// The layout.
        /// </summary>
        private readonly RelativeLayout _layout;

        /// <summary>
        /// Entry and postfix layout
        /// </summary>
	    private readonly StackLayout _entryAndPostfixLayout;

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
            HeightRequest = Device.OnPlatform<int>(50, 50, 75);
            InputTransparent = false;

			// Create placeholder label
			_floatingLabel = new Label {
				BackgroundColor = Color.Transparent,
				Text = Placeholder,
				FontAttributes = FontAttributes.Bold,
				HorizontalTextAlignment= TextAlignment.Start,
                VerticalTextAlignment = TextAlignment.Center,
                FontSize = LabelFontSize,
				Opacity = 0.0,
                TextColor = PlaceholderColor,
                InputTransparent = true
			};

			// Create textfield
			_textEntry = new ExtendedEntry { 
				Keyboard = this.Keyboard,
                BackgroundColor = Color.Transparent,
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
				
			_textEntry.SetBinding (Entry.TextProperty, TextProperty.PropertyName, BindingMode.TwoWay);
			_textEntry.BindingContext = this;
            _textEntry.Focused += (object sender, FocusEventArgs e) =>
            {
                UpdatePlaceholderColor();
             
                if(Focused != null)
                    Focused(this, e);
            };

			_textEntry.Unfocused += (object sender, FocusEventArgs e) =>
            {
                UpdatePlaceholderColor();

                if(Unfocused != null)
                    Unfocused(this, e);
            };

			_textEntry.TextChanged += TextEntry_TextChanged;

            // Postfix
            _postFix = new Label{
				BackgroundColor = Color.Transparent,                
				HorizontalTextAlignment= TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,                
				TextColor = PostfixColor,
                InputTransparent = true
            };

		    UpdatePostFix();

			// create stacklayout for entry and postfix
            _entryAndPostfixLayout = new StackLayout
            {
				BackgroundColor = Color.Transparent,
                Padding = 0,                
				Orientation = StackOrientation.Horizontal,
				Children = {
					_textEntry, _postFix,
				}
			};

			// Create layout
            _layout = new RelativeLayout { IsClippedToBounds = true };

            // Position label
		 	_layout.Children.Add (_floatingLabel, () => new Rectangle(
				Device.OnPlatform<int>(0, 0, 0), 
                14, 
                _layout.Width-20, 
                _layout.Height-14));
			            
			// Position entry/postfix
            _layout.Children.Add(_entryAndPostfixLayout, () => 
				new Rectangle(
					Device.OnPlatform<int>(0, -4, -15), 
					Device.OnPlatform<int>(12, 12, 14), 
					_layout.Width - Device.OnPlatform<int>(0, -4, (string.IsNullOrWhiteSpace(Postfix) ? -34 : -15)), 
					_layout.Height - Device.OnPlatform<int>(12, 0, 2)));

			// underline
			_underlineControl = new BoxView {				
				BackgroundColor = GetCurrentPlaceholderColor(),
                InputTransparent = true
			};

			_layout.Children.Add (_underlineControl, () => new Rectangle (0, 
				Device.OnPlatform<double> (_layout.Height - 4, _layout.Height - 4, Bounds.Height - 4), 
				_layout.Width, 0.5));

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

			UpdatePlaceholderColor ();

		}

		#endregion

		#region Public Members

		/// <summary>
		/// Focus this instance.
		/// </summary>
		public new void Focus()
		{
			_textEntry.Focus();
		}

		/// <summary>
		/// Unfocus this instance.
		/// </summary>
		public new void Unfocus()
		{
			_textEntry.Unfocus();
		}

		#endregion

		#region Properties
		/// <summary>
		/// The TextColor property.
		/// </summary>
		public static BindableProperty TextColorProperty =
            BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FloatingLabelControl), Color.Black, propertyChanged:
                (bindable, oldValue, newValue) =>
                {
                    var ctrl = (FloatingLabelControl)bindable;
                    ctrl.TextColor = (Color)newValue;
                });

		/// <summary>
		/// Gets or Sets the TextColor property.
		/// </summary>
        public Color TextColor
        {
            get { return (Color)GetValue(TextColorProperty); }
            set
            {
                SetValue(TextColorProperty, value);
                _textEntry.TextColor = value;
            }
        }
		
		/// <summary>
		/// The LabelFontSize property.
		/// </summary>
		public static BindableProperty LabelFontSizeProperty = 
			BindableProperty.Create(nameof(LabelFontSize), typeof(int), typeof(FloatingLabelControl), Device.OnPlatform<int>(10, 10, 12),
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (FloatingLabelControl)bindable;
					ctrl.LabelFontSize = (int)newValue;
				});

		/// <summary>
		/// Gets or sets the LabelFontSize of the FloatingLabelControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public int LabelFontSize {
			get{ return (int)GetValue (LabelFontSizeProperty); }
			set {
				SetValue (LabelFontSizeProperty, value);
				_floatingLabel.FontSize = value;
			}
		}

		/// <summary>
		/// The LabelFontFamily property.
		/// </summary>
		public static BindableProperty LabelFontFamilyProperty = 
			BindableProperty.Create(nameof(LabelFontFamily), typeof(string), typeof(FloatingLabelControl), null,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (FloatingLabelControl)bindable;
					ctrl.LabelFontFamily = (string)newValue;
				});

		/// <summary>
		/// Gets or sets the LabelFontFamily of the FloatingLabelControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public string LabelFontFamily {
			get{ return (string)GetValue (LabelFontFamilyProperty); }
			set {
				SetValue (LabelFontFamilyProperty, value);
				_floatingLabel.FontFamily = value;
			}
		}

		/// <summary>
		/// The FontFamily property.
		/// </summary>
		public static BindableProperty FontFamilyProperty = 
			BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(FloatingLabelControl), null,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (FloatingLabelControl)bindable;
					ctrl.FontFamily = (string)newValue;
				});

		/// <summary>
		/// Gets or sets the FontFamily of the FloatingLabelControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public string FontFamily {
			get{ return (string)GetValue (FontFamilyProperty); }
			set {
				SetValue (FontFamilyProperty, value);
				_textEntry.FontFamily = value;
			}
		}

		/// <summary>
		/// The PostfixFontFamily property.
		/// </summary>
		public static BindableProperty PostfixFontFamilyProperty = 
			BindableProperty.Create(nameof(PostfixFontFamily), typeof(string), typeof(FloatingLabelControl), null,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (FloatingLabelControl)bindable;
					ctrl.PostfixFontFamily = (string)newValue;
				});

		/// <summary>
		/// Gets or sets the PostfixFontFamily of the FloatingLabelControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public string PostfixFontFamily {
			get{ return (string)GetValue (PostfixFontFamilyProperty); }
			set {
				SetValue (PostfixFontFamilyProperty, value);
				_postFix.FontFamily = value;
			}
		}

		/// <summary>
		/// The keyboard property.
		/// </summary>
		public static BindableProperty KeyboardProperty = 
			BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(FloatingLabelControl), Xamarin.Forms.Keyboard.Default,
                BindingMode.Default, null, (bindable, oldValue, newValue) => {
                    var ctrl = (FloatingLabelControl)bindable;
                    ctrl._textEntry.Keyboard = (Keyboard)newValue;
                });

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
		/// The XAlign property.
		/// </summary>
		public static BindableProperty HorizontalTextAlignment = 
			BindableProperty.Create(nameof(XAlign), typeof(TextAlignment), typeof(FloatingLabelControl), TextAlignment.Start, BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (FloatingLabelControl)bindable;
					ctrl.XAlign = (TextAlignment)newValue;
				});

		/// <summary>
		/// Gets or sets the XAlign of the FloatingLabelControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public TextAlignment XAlign {
			get{ return (TextAlignment)GetValue (HorizontalTextAlignment); }
			set {
				SetValue (HorizontalTextAlignment, value);
				_textEntry.HorizontalTextAlignment = value;
			}
		}

		/// <summary>
		/// The placeholder property.
		/// </summary>
		public static BindableProperty PlaceholderProperty = 
			BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FloatingLabelControl), string.Empty,
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
                var ctrl = (FloatingLabelControl)bindable;
                ctrl.Placeholder = (string)newValue;
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
			BindableProperty.Create(nameof(Text), typeof(string), typeof(FloatingLabelControl), string.Empty,
				BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>{
                var ctrl = (FloatingLabelControl)bindable;
                ctrl.Text = (string)newValue;
            });
		

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
            get { return (string)GetValue (TextProperty); }
            set { 
                SetValue (TextProperty, value);                 
            }
		}

        /// <summary>
        /// The postfix color property.
        /// </summary>
		public static BindableProperty PostfixColorProperty = 
			BindableProperty.Create(nameof(PostfixColor), typeof(Color), typeof(FloatingLabelControl), Color.FromHex("#CCCCCC"),
                propertyChanged: (bindable, oldValue, newValue) =>{
                var ctrl = (FloatingLabelControl)bindable;
                ctrl.PostfixColor = (Color)newValue;
            });


        /// <summary>
        /// Gets or sets the Postfix color.
        /// </summary>
        /// <value>The text.</value>
        public Color PostfixColor
        {
			get { return (Color)GetValue (PostfixColorProperty); }
            set 
            { 
				SetValue (PostfixColorProperty, value); 
				_postFix.TextColor = value;                
            }
        }

		/// <summary>
		/// The postfix property.
		/// </summary>
		public static BindableProperty PostfixProperty = 
			BindableProperty.Create(nameof(Postfix), typeof(string), typeof(FloatingLabelControl), string.Empty,
				propertyChanged: (bindable, oldValue, newValue) =>{
					var ctrl = (FloatingLabelControl)bindable;
					ctrl.Postfix = (string)newValue;
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
                _entryAndPostfixLayout.ForceLayout();
			}
		}

		/// <summary>
		/// The postfix property.
		/// </summary>
		public static BindableProperty PostfixIconProperty = 
			BindableProperty.Create(nameof(PostfixIcon), typeof(string), typeof(FloatingLabelControl), string.Empty,
				propertyChanged: (bindable, oldValue, newValue) =>{
					var ctrl = (FloatingLabelControl)bindable;
					ctrl.PostfixIcon = (string)newValue;
				});


		/// <summary>
		/// Gets or sets the Postfix.
		/// </summary>
		/// <value>The text.</value>
		public string PostfixIcon
		{
			get { return (string)GetValue (PostfixIconProperty); }
			set 
			{ 
				SetValue (PostfixIconProperty, value);
                UpdatePostFix();
			}
		}

		/// <summary>
		/// The placeholder color property.
		/// </summary>
        public static BindableProperty PlaceholderFocusedColorProperty =
            BindableProperty.Create(nameof(PlaceholderFocusedColor), typeof(Color), typeof(FloatingLabelControl), Color.Accent,
                BindingMode.Default, null, (bindable, oldValue, newValue) => {
                var ctrl = (FloatingLabelControl)bindable;
                ctrl.PlaceholderFocusedColor = (Color)newValue;	
            });


		/// <summary>
		/// Gets or sets the placeholder color.
		/// </summary>
		/// <value>The text.</value>
		public Color PlaceholderFocusedColor
		{
			get { return (Color)GetValue (PlaceholderFocusedColorProperty); }
			set
            {
                SetValue(PlaceholderFocusedColorProperty, value);
                UpdatePlaceholderColor();
            }
		}

        /// <summary>
        /// The placeholder color property.
        /// </summary>
        public static BindableProperty PlaceholderColorProperty =
            BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(FloatingLabelControl), Color.FromHex("#CCCCCC"),
                BindingMode.Default, null, (bindable, oldValue, newValue) =>
                {
                    var ctrl = (FloatingLabelControl)bindable;
                    ctrl.PlaceholderColor = (Color)newValue;
                });


        /// <summary>
        /// Gets or sets the placeholder color.
        /// </summary>
        /// <value>The text.</value>
        public Color PlaceholderColor
        {
            get { return (Color)GetValue(PlaceholderColorProperty); }
            set
            {
                SetValue(PlaceholderColorProperty, value);
                UpdatePlaceholderColor();
            }
        }

		/// <summary>
		/// The is password property.
		/// </summary>
		public static readonly BindableProperty IsPasswordProperty = 
			BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(FloatingLabelControl), false, 
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
				var ctrl = (FloatingLabelControl)bindable;
				ctrl.IsPassword = (bool)newValue;
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

        #region Private Members

        /// <summary>
        /// Update postfix label
        /// </summary>
        private void UpdatePostFix()
        {
            _postFix.Text = string.IsNullOrWhiteSpace(PostfixIcon) ? Postfix : PostfixIcon;
			_postFix.FontFamily = string.IsNullOrWhiteSpace(PostfixIcon) ? null : FontAwesomeLabel.FontAwesomeName;
            //_postFix.FontSize = string.IsNullOrWhiteSpace(PostfixIcon)
            //    ? Device.OnPlatform<int>(14, 14, 14)
            //    : Device.OnPlatform<int>(18, 18, 18);

            if (_layout == null)
                return;

            _layout.ForceLayout();
            _entryAndPostfixLayout.ForceLayout();
        }

		/// <summary>
		/// Gets the color of the current placeholder.
		/// </summary>
		/// <returns>The current placeholder color.</returns>
		private Color GetCurrentPlaceholderColor()
		{
			return _textEntry.IsFocused && !string.IsNullOrWhiteSpace(_textEntry.Text) ? 
				PlaceholderFocusedColor :
				PlaceholderColor;
		}

        /// <summary>
        /// Updates the placeholder color
        /// </summary>
	    private void UpdatePlaceholderColor()
        {
            _floatingLabel.TextColor = _textEntry.IsFocused ? PlaceholderFocusedColor : PlaceholderColor;
			_underlineControl.BackgroundColor = GetCurrentPlaceholderColor ();
        }

	    #endregion

    }
}

