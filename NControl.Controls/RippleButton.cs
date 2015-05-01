using System;
using Xamarin.Forms;
using NControl.Abstractions;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;

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
		private readonly Button _textButton;

		/// <summary>
		/// The icon label.
		/// </summary>
		private readonly Button _iconButton;

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
			_rippler = new RippleControl();
			_layout.Children.Add (_rippler, () => _layout.Bounds);

			// Add title and icon
			_textButton = new Button{ 
				BackgroundColor = Color.Transparent,
				BorderColor = Color.Transparent,
				TextColor = Color.Black,
				BorderRadius = 0,
				BorderWidth = 0,
			};
			_textButton.Clicked += (sender, e) => _rippler.RippleAsync(
				_layout.Width/2, _layout.Height/2, true);

			_layout.Children.Add (_textButton, ()=> GetTextRectangleForImagePosition(_layout));

			_iconButton = new Button{ 
				BackgroundColor = Color.Transparent,
				BorderColor = Color.Transparent,
				BorderRadius = 0,
				BorderWidth = 0,
				FontFamily = FontAwesomeLabel.FontAwesomeName,
				FontSize = 18,
				TextColor = (Color)IconColorProperty.DefaultValue,
			};
			_iconButton.Clicked += (sender, e) => _rippler.RippleAsync(
				_layout.Width/2, _layout.Height/2, true);

			_layout.Children.Add (_iconButton, () => GetIconRectangleForImagePosition(_layout));

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
				_iconButton.Text = value;
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
				_iconButton.TextColor = value;
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
				_textButton.Text = value;
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
				_textButton.FontFamily = value;
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
			set {
				SetValue (CommandProperty, value);

				_textButton.Command = value;
				_iconButton.Command = value;
			}
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
			set {
				SetValue (CommandParameterProperty, value);

				_textButton.CommandParameter = value;
				_iconButton.CommandParameter = value;
			}
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
				_textButton.TextColor = value;
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

				_textButton.FontSize = value;
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

				_iconButton.FontFamily = value;
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
			var position = new Point (x, y);

			_ellipse.Scale = 0.0;
			_ellipse.Opacity = 1.0;

			var layout = Content as RelativeLayout;
			_ellipse.TranslationX = -((layout.Width / 2) - position.X);
			_ellipse.TranslationY = -((layout.Height / 2) - position.Y);

			if (animate) {
				await _ellipse.ScaleTo (2.0, easing: Easing.CubicInOut);
				await _ellipse.FadeTo (0.0, easing: Easing.CubicInOut);
			}

					
		}

		/// <summary>
		/// Handles the end.
		/// </summary>
		/// <returns><c>true</c>, if end was handled, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		private bool HandleEnd(NGraphics.Point point, bool allowCancel)
		{
			_ellipse.FadeTo (0.0, 50);

			// Should we allow cancel?
//			if (allowCancel) {
//				var d = ((_touchStart.X - point.X) * (_touchStart.X - point.X) + (_touchStart.Y - point.Y) * (_touchStart.Y - point.Y));
//				if (d > 35) {
//					_ellipse.FadeTo (0.0, 50);
//					return false;
//				}
//			}

			// Execute command
//			if (Command != null && Command.CanExecute (CommandParameter))
//				Command.Execute (CommandParameter);
//

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

