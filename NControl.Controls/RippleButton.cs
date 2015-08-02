using System;
using Xamarin.Forms;
using NControl.Abstractions;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace NControl.Controls
{
	/// <summary>
	/// Image position.
	/// </summary>
	public enum ImagePosition
	{
		Left,
		Right,
		Top,
		Bottom
	}

	/// <summary>
	/// Ripple button.
	/// </summary>
	public class RippleButton: RoundCornerView
	{
		#region Constants

		/// <summary>
		/// The width of the image.
		/// </summary>
		private const double ImageWidth = 44;

		#endregion

		#region Private Members

		/// <summary>
		/// The text label.
		/// </summary>
        private readonly Label _labelText;

		/// <summary>
		/// The icon label.
		/// </summary>
        private readonly FontAwesomeLabel _iconLabel;

		/// <summary>
		/// The rippler.
		/// </summary>
		private readonly RippleControl _rippler;

		/// <summary>
		/// The layout.
		/// </summary>
		private readonly RelativeLayout _layout;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.RippleButton"/> class.
		/// </summary>
		public RippleButton()
		{            
            _layout = new RelativeLayout {IsClippedToBounds = true};

			// Add ripple thing
			_rippler = new RippleControl{
				InputTransparent = true,
			};
			_layout.Children.Add (_rippler, () => _layout.Bounds);

			// Add title and icon
            _labelText = new Label{ 
				BackgroundColor = Color.Transparent,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
				TextColor = Color.Black,				
				InputTransparent = true,
			};
			
			_layout.Children.Add (_labelText, ()=> GetTextRectangleForImagePosition(_layout));

            _iconLabel = new FontAwesomeLabel{ 
				BackgroundColor = Color.Transparent,
				FontSize = 18,
				TextColor = (Color)IconColorProperty.DefaultValue,
				InputTransparent = true,
			};
			
			_layout.Children.Add (_iconLabel, () => GetIconRectangleForImagePosition(_layout));

			Content = _layout;
		}

		#region Properties

		/// <summary>
		/// The ImagePosition property.
		/// </summary>
		public static BindableProperty ImagePositionProperty = 
			BindableProperty.Create<RippleButton, ImagePosition> (p => p.ImagePosition, 
				ImagePosition.Left, BindingMode.TwoWay, propertyChanged: 
				(bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.ImagePosition = newValue;
				});

		/// <summary>
		/// Gets or sets the ImagePosition of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public ImagePosition ImagePosition {
			get{ return (ImagePosition)GetValue (ImagePositionProperty); }
			set {
				SetValue (ImagePositionProperty, value);
				_layout.ForceLayout ();
			}
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Gets the text layout for image position.
		/// </summary>
		/// <returns>The text layout for image position.</returns>
		/// <param name="layout">Layout.</param>
		private Rectangle GetTextRectangleForImagePosition (RelativeLayout layout)
		{
			if (string.IsNullOrEmpty (_iconLabel.Text))
				return layout.Bounds;
			
			switch (ImagePosition) {

			case ImagePosition.Top:
				return new Rectangle (0, layout.Height/2, layout.Width, layout.Height/2);

			case ImagePosition.Bottom:
				return new Rectangle (0, 0, layout.Width, layout.Height/2);

			default:
				return layout.Bounds;
			}
		}

		/// <summary>
		/// Gets the icon layout for image position.
		/// </summary>
		/// <returns>The icon layout for image position.</returns>
		/// <param name="layout">Layout.</param>
		private Rectangle GetIconRectangleForImagePosition(RelativeLayout layout)
		{
			switch (ImagePosition) {

			case ImagePosition.Right:
				return new Rectangle (layout.Width-ImageWidth, 0, ImageWidth, layout.Height);

			case ImagePosition.Top:
				return new Rectangle (0, 0, layout.Width, layout.Height/2);

			case ImagePosition.Bottom:
				return new Rectangle (0, layout.Height/2, layout.Width, layout.Height/2);

			case ImagePosition.Left:
			default:				
				return new Rectangle (0, 0, ImageWidth, layout.Height);
			}
		}
		#endregion

		#region Commands

		/// <summary>
		/// The Icon property.
		/// </summary>
		public static BindableProperty IconProperty = 
			BindableProperty.Create<RippleButton, string> (p => p.Icon, string.Empty,
				BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.Icon = newValue;
				});

		/// <summary>
		/// Gets or sets the Icon of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public string Icon {
			get{ return (string)GetValue (IconProperty); }
			set {
				SetValue (IconProperty, value);
				_iconLabel.Text = value;
			}
		}

		/// <summary>
		/// The IconColor property.
		/// </summary>
		public static BindableProperty IconColorProperty = 
			BindableProperty.Create<RippleButton, Color> (p => p.IconColor, Color.FromHex("#BBBBBB"),
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.IconColor = newValue;
				});

		/// <summary>
		/// Gets or sets the IconColor of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color IconColor {
			get{ return (Color)GetValue (IconColorProperty); }
			set {
				SetValue (IconColorProperty, value);
				_iconLabel.TextColor = value;
			}
		}


		/// <summary>
		/// The Text property.
		/// </summary>
		public static BindableProperty TextProperty = 
			BindableProperty.Create<RippleButton, string> (p => p.Text, string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.Text = newValue;
				});

		/// <summary>
		/// Gets or sets the Text of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public string Text {
			get{ return (string)GetValue(TextProperty); }
			set
			{
				SetValue(TextProperty, value);
				_labelText.Text = value;
			}
		}

		/// <summary>
		/// The FontFamily property.
		/// </summary>
		public static BindableProperty FontFamilyProperty = 
			BindableProperty.Create<RippleButton, string> (p => p.FontFamily, null,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.FontFamily = newValue;
				});

		/// <summary>
		/// Gets or sets the FontFamily of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public string FontFamily {
			get{ return (string)GetValue (FontFamilyProperty); }
			set {
				SetValue (FontFamilyProperty, value);
				_labelText.FontFamily = value;
			}
		}

		/// <summary>
		/// The Command property.
		/// </summary>
		public static BindableProperty CommandProperty = 
			BindableProperty.Create<RippleButton, ICommand> (p => p.Command, null,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.Command = newValue;
				});

		/// <summary>
		/// Gets or sets the Command of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public ICommand Command {
			get{ return (ICommand)GetValue (CommandProperty); }
			set { SetValue (CommandProperty, value); }
		}

		/// <summary>
		/// The CommandParameter property.
		/// </summary>
		public static BindableProperty CommandParameterProperty = 
			BindableProperty.Create<RippleButton, object> (p => p.CommandParameter, null,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.CommandParameter = newValue;
				});

		/// <summary>
		/// Gets or sets the CommandParameter of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public object CommandParameter {
			get{ return (object)GetValue (CommandParameterProperty); }
			set { SetValue (CommandParameterProperty, value); }
		}

		/// <summary>
		/// The TextColor property.
		/// </summary>
		public static BindableProperty TextColorProperty = 
			BindableProperty.Create<RippleButton, Color> (p => p.TextColor, Color.Black,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.TextColor = newValue;
				});
					
		/// <summary>
		/// Gets or sets the TextColor of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color TextColor {
			get{ return (Color)GetValue (TextColorProperty); }
			set {
				SetValue (TextColorProperty, value);
				_labelText.TextColor = value;
			}
		}

		/// <summary>
		/// The FontSize property.
		/// </summary>
		public static BindableProperty FontSizeProperty = 
			BindableProperty.Create<RippleButton, double> (p => p.FontSize, Font.Default.FontSize,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.FontSize = newValue;
				});

		/// <summary>
		/// Gets or sets the FontSize of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public double FontSize {
			get{ return (double)GetValue (FontSizeProperty); }
			set {
				SetValue (FontSizeProperty, value);

				_labelText.FontSize = value;
			}
		}

		/// <summary>
		/// The IconFontFamily property.
		/// </summary>
		public static BindableProperty IconFontFamilyProperty = 
			BindableProperty.Create<RippleButton, string> (p => p.IconFontFamily, null,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.IconFontFamily = newValue;
				});

		/// <summary>
		/// Gets or sets the IconFontFamily of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public string IconFontFamily {
			get{ return (string)GetValue (IconFontFamilyProperty); }
			set {
				SetValue (IconFontFamilyProperty, value);

				_iconLabel.FontFamily = value;
			}
		}

		/// <summary>
		/// The RippleColor property.
		/// </summary>
		public static BindableProperty RippleColorProperty = 
			BindableProperty.Create<RippleButton, Color> (p => p.RippleColor, Color.FromHex("#DDDDDD"),
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.RippleColor = newValue;
				});

		/// <summary>
		/// Gets or sets the RippleColor of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color RippleColor {
			get{ return (Color)GetValue (RippleColorProperty); }
			set {
				SetValue (RippleColorProperty, value);

				_rippler.RippleColor = value;
			}
		}
		#endregion

		#region Sizing

		/// <param name="widthConstraint">The available width that a parent element can allocated to a child. Value will be between 0 and double.PositiveInfinity.</param>
		/// <param name="heightConstraint">The available height that a parent element can allocated to a child. Value will be between 0 and double.PositiveInfinity.</param>
		/// <summary>
		/// Gets the size request.
		/// </summary>
		/// <returns>The size request.</returns>
		protected override SizeRequest OnSizeRequest (double widthConstraint, double heightConstraint)
		{							
			var retVal = new Button().GetSizeRequest (widthConstraint, heightConstraint);
            var labelSize = _labelText.GetSizeRequest(widthConstraint, heightConstraint);

            return new SizeRequest (new Size (labelSize.Request.Width + 8, retVal.Request.Height), 
				retVal.Minimum);
		}

		#endregion

		#region Touches

		/// <summary>
		/// Toucheses the began.
		/// </summary>
		/// <returns><c>true</c>, if began was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
        public override bool TouchesBegan(System.Collections.Generic.IEnumerable<NGraphics.Point> points)
        {
			base.TouchesBegan(points);

			var point = points.FirstOrDefault();
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _rippler.RippleAsync(point.X, point.Y, true);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _labelText.TextColor = Color.Accent;
            
			return true;
        }

		/// <summary>
		/// Toucheses the ended.
		/// </summary>
		/// <returns><c>true</c>, if ended was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
        public override bool TouchesEnded(System.Collections.Generic.IEnumerable<NGraphics.Point> points)
        {
            base.TouchesEnded(points);
            _labelText.TextColor = TextColor;
            if (Command != null && Command.CanExecute(CommandParameter))
                Command.Execute(CommandParameter);
            
            return true;
        }

        /// <summary>
        /// Toucheses the cancelled.
        /// </summary>
        /// <returns><c>true</c>, if cancelled was touchesed, <c>false</c> otherwise.</returns>
        /// <param name="points">Points.</param>
        public override bool TouchesCancelled(IEnumerable<NGraphics.Point> points)
        {
            base.TouchesCancelled(points);
            _labelText.TextColor = TextColor;
            return true;
        }

		#endregion
	}

	/// <summary>
	/// Ripple button.
	/// </summary>
	public class RippleControl: RoundCornerView
	{				
		#region Private Members

		/// <summary>
		/// The ellipse.
		/// </summary>
		protected readonly NControlView _ellipse;

		/// <summary>
		/// The animation lock.
		/// </summary>
		private bool _animationLock = false;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.RippleButton"/> class.
		/// </summary>
		public RippleControl ()
		{			
			HeightRequest = 44;

			var layout = new RelativeLayout ();
			Content = layout;
			IsClippedToBounds = true;

			_ellipse = new NControlView {
				BackgroundColor = Color.Transparent,
				DrawingFunction = (canvas, rect) =>{
					canvas.DrawEllipse(rect, null, new NGraphics.SolidBrush(RippleColor.ToNColor()));
				},
				Scale = 0.0,
			};				

			layout.Children.Add (_ellipse, () => new Rectangle (
				(layout.Width / 2) - (Math.Max(layout.Width, layout.Height) / 2),
				(layout.Height / 2) - (Math.Max(layout.Width, layout.Height) / 2),
				Math.Max(layout.Width, layout.Height), 
				Math.Max(layout.Width, layout.Height)));
			
		}	

		/// <summary>
		/// Ripple this instance.
		/// </summary>
		public async Task RippleAsync(double x, double y, bool animate)
		{
			if (_animationLock)
				return;

			_animationLock = true;
			var position = new Point (x, y);

			_ellipse.Scale = 0.0;
			_ellipse.Opacity = 1.0;

			var layout = Content as RelativeLayout;
			_ellipse.TranslationX = -((layout.Width / 2) - position.X);
			_ellipse.TranslationY = -((layout.Height / 2) - position.Y);

			if (animate) {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                _ellipse.ScaleTo (2.0, easing: Easing.CubicInOut);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                await _ellipse.FadeTo (0.5, 150, easing: Easing.CubicInOut);
				await _ellipse.FadeTo (0.0, 250, easing: Easing.CubicInOut);
			}		

			_animationLock = false;
		}

		/// <summary>
		/// Handles the end.
		/// </summary>
		/// <returns><c>true</c>, if end was handled, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		private bool HandleEnd(NGraphics.Point point, bool allowCancel)
		{
			_ellipse.FadeTo (0.0, 50);

			return true;
		}

		/// <summary>
		/// The ShouldRipple property.
		/// </summary>
		public static BindableProperty ShouldRippleProperty = 
			BindableProperty.Create<RippleControl, bool> (p => p.ShouldRipple, true,
				BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleControl)bindable;
					ctrl.ShouldRipple = newValue;
				});

		/// <summary>
		/// Gets or sets the ShouldRipple of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public bool ShouldRipple {
			get{ return (bool)GetValue (ShouldRippleProperty); }
			set {
				SetValue (ShouldRippleProperty, value);
			}
		}

		/// <summary>
		/// The RippleColor property.
		/// </summary>
		public static BindableProperty RippleColorProperty = 
			BindableProperty.Create<RippleControl, Color> (p => p.RippleColor, Color.FromHex("#DDDDDD"),
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleControl)bindable;
					ctrl.RippleColor = newValue;
				});

		/// <summary>
		/// Gets or sets the RippleColor of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color RippleColor {
			get{ return (Color)GetValue (RippleColorProperty); }
			set {
				SetValue (RippleColorProperty, value);
				_ellipse.BackgroundColor = value;
			}
		}			
	}
}

