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
            HeightRequest = Device.OnPlatform<int>(50, 50, 75); ;

			// Create placeholder label
			_floatingLabel = new Label {
				BackgroundColor = Color.Transparent,
				Text = Placeholder,
				FontAttributes = FontAttributes.Bold,
				XAlign = TextAlignment.Start,
				YAlign = TextAlignment.Center,
                FontSize = LabelFontSize,
				Opacity = 0.0,
                TextColor = PlaceholderColor
			};

			// Create textfield
			_textEntry = new ExtendedEntry { 
				Keyboard = this.Keyboard,
				BackgroundColor = Color.Transparent,
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
				XAlign = TextAlignment.Center,
                YAlign = TextAlignment.Center,                
				TextColor = PostfixColor
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

		#region Properties

		/// <summary>
		/// The LabelFontSize property.
		/// </summary>
		public static BindableProperty LabelFontSizeProperty = 
			BindableProperty.Create<FloatingLabelControl, int> (p => p.LabelFontSize, Device.OnPlatform<int>(10, 10, 12),
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (FloatingLabelControl)bindable;
					ctrl.LabelFontSize = newValue;
				});

		/// <summary>
		/// Gets or sets the LabelFontSize of the FloatingLabelControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public int LabelFontSize {
			get{ return (int)GetValue (LabelFontSizeProperty); }
			set {
				SetValue (LabelFontSizeProperty, value);
			}
		}
		/// <summary>
		/// The keyboard property.
		/// </summary>
		public static BindableProperty KeyboardProperty = 
			BindableProperty.Create<FloatingLabelControl, Keyboard> (p => p.Keyboard, Xamarin.Forms.Keyboard.Default,
                BindingMode.Default, null, (bindable, oldValue, newValue) => {
                    var ctrl = (FloatingLabelControl)bindable;
                    ctrl._textEntry.Keyboard = newValue;
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
		public static BindableProperty XAlignProperty = 
			BindableProperty.Create<FloatingLabelControl, TextAlignment> (p => p.XAlign, 
				TextAlignment.Start, BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (FloatingLabelControl)bindable;
					ctrl.XAlign = newValue;
				});

		/// <summary>
		/// Gets or sets the XAlign of the FloatingLabelControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public TextAlignment XAlign {
			get{ return (TextAlignment)GetValue (XAlignProperty); }
			set {
				SetValue (XAlignProperty, value);
				_textEntry.XAlign = value;
			}
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
				BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) =>{
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
            set { 
                SetValue (TextProperty, value);                 
            }
		}

        /// <summary>
        /// The postfix color property.
        /// </summary>
		public static BindableProperty PostfixColorProperty = 
			BindableProperty.Create<FloatingLabelControl, Color>(p => p.PostfixColor, Color.FromHex("#CCCCCC"),
                propertyChanged: (bindable, oldValue, newValue) =>{
                var ctrl = (FloatingLabelControl)bindable;
                ctrl.PostfixColor = newValue;
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
                _entryAndPostfixLayout.ForceLayout();
			}
		}

		/// <summary>
		/// The postfix property.
		/// </summary>
		public static BindableProperty PostfixIconProperty = 
			BindableProperty.Create<FloatingLabelControl, string>(p => p.PostfixIcon, string.Empty,
				propertyChanged: (bindable, oldValue, newValue) =>{
					var ctrl = (FloatingLabelControl)bindable;
					ctrl.PostfixIcon = newValue;
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
            BindableProperty.Create<FloatingLabelControl, Color>(p => p.PlaceholderFocusedColor, Color.Accent,
                BindingMode.Default, null, (bindable, oldValue, newValue) => {
                var ctrl = (FloatingLabelControl)bindable;
                ctrl.PlaceholderFocusedColor = newValue;	
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
            BindableProperty.Create<FloatingLabelControl, Color>(p => p.PlaceholderColor, Color.FromHex("#CCCCCC"),
                BindingMode.Default, null, (bindable, oldValue, newValue) =>
                {
                    var ctrl = (FloatingLabelControl)bindable;
                    ctrl.PlaceholderColor = newValue;
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

		#region Drawing

		/// <summary>
		/// Draw the specified canvas and rect.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="rect">Rect.</param>
		public override void Draw (NGraphics.ICanvas canvas, NGraphics.Rect rect)
		{
			base.Draw (canvas, rect);

			var bottom = Device.OnPlatform<double> (rect.Height-4, 
				rect.Height-4, Bounds.Height -4);

			canvas.DrawPath (new NGraphics.PathOp[]{ 
				new NGraphics.MoveTo(_textEntry.Bounds.Left-1, bottom),
				new NGraphics.LineTo(rect.Width, bottom)
			}, new NGraphics.Pen(GetCurrentPlaceholderColor(), 0.5));
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
		private NGraphics.Color GetCurrentPlaceholderColor()
		{
			return _textEntry.IsFocused && !string.IsNullOrWhiteSpace(_textEntry.Text) ? 
				new NGraphics.Color (PlaceholderFocusedColor.R, PlaceholderFocusedColor.G, PlaceholderFocusedColor.B) :
				new NGraphics.Color (PlaceholderColor.R, PlaceholderColor.G, PlaceholderColor.B);
		}

        /// <summary>
        /// Updates the placeholder color
        /// </summary>
	    private void UpdatePlaceholderColor()
        {
            _floatingLabel.TextColor = _textEntry.IsFocused ? PlaceholderFocusedColor : PlaceholderColor;
			Invalidate ();
        }

	    #endregion
    }
}

