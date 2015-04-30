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
		/// The height of the image.
		/// </summary>
		private const double ImageHeight = 34;

		#endregion

		#region Private Members

		/// <summary>
		/// The color of the orginal text.
		/// </summary>
		private Color _orginalTextColor;

		/// <summary>
		/// The touch start.
		/// </summary>
		private NGraphics.Point _touchStart;

		/// <summary>
		/// The ellipse.
		/// </summary>
		protected readonly NControlView _ellipse;

		/// <summary>
		/// The text label.
		/// </summary>
		public readonly Label TextLabel;

		/// <summary>
		/// The icon label.
		/// </summary>
		public readonly FontAwesomeLabel IconLabel;


		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.RippleButton"/> class.
		/// </summary>
		public RippleButton ()
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

		
			TextLabel = new Label{ 
				BackgroundColor = Color.Transparent,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
			};
			_orginalTextColor = TextLabel.TextColor;

			layout.Children.Add (TextLabel, ()=> GetTextRectangleForImagePosition(layout));

			IconLabel = new FontAwesomeLabel {
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
				TextColor = IconColor,
			};

			layout.Children.Add (IconLabel, () => GetIconRectangleForImagePosition(layout));

		}	

		/// <summary>
		/// Gets the text layout for image position.
		/// </summary>
		/// <returns>The text layout for image position.</returns>
		/// <param name="layout">Layout.</param>
		private Rectangle GetTextRectangleForImagePosition (RelativeLayout layout)
		{
			switch (ImagePosition) {

			case ImagePosition.Top:
				return new Rectangle (0, ImageHeight, layout.Width, layout.Height - ImageHeight);

			case ImagePosition.Bottom:
				return new Rectangle (0, 0, layout.Width, layout.Height - ImageHeight);

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
				return new Rectangle (layout.Width-(ImageHeight+8), 0, layout.Height, layout.Height);

			case ImagePosition.Top:
				return new Rectangle (0, 0, layout.Width, ImageHeight);

			case ImagePosition.Bottom:
				return new Rectangle (0, layout.Height-ImageHeight, ImageHeight, ImageHeight);

			case ImagePosition.Left:
			default:				
				return new Rectangle (8, 0, layout.Height, layout.Height);
			}
		}

		/// <summary>
		/// Ripple this instance.
		/// </summary>
		public async Task RippleAsync(double x, double y, bool animate)
		{
			await InternalRippleAsync (x, y, animate);

			if (animate)
				await _ellipse.FadeTo (0.0, 50);
			else
				_ellipse.Opacity = 0.0;
				
		}

		/// <summary>
		/// Internals the ripple async.
		/// </summary>
		/// <returns>The ripple async.</returns>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		private async Task InternalRippleAsync(double x, double y, bool animate)
		{
			var position = new Point (x, y);

			_ellipse.Scale = 0.0;
			_ellipse.Opacity = 1.0;

			var layout = Content as RelativeLayout;
			_ellipse.TranslationX = -((layout.Width / 2) - position.X);
			_ellipse.TranslationY = -((layout.Height / 2) - position.Y);

			if (animate)
				await _ellipse.ScaleTo (1.2, easing: Easing.CubicInOut);
			else
				return;
		}

		/// <summary>
		/// Toucheses the began.
		/// </summary>
		/// <returns><c>true</c>, if began was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesBegan (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesBegan (points);

			if (!IsEnabled)
				return false;

			var firstPoint = points.FirstOrDefault ();
			_touchStart = firstPoint;

			Device.BeginInvokeOnMainThread (async () =>  await InternalRippleAsync(firstPoint.X, firstPoint.Y, true));

			return true;
		}

		/// <summary>
		/// Toucheses the cancelled.
		/// </summary>
		/// <returns><c>true</c>, if cancelled was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesCancelled (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesCancelled (points);
			return HandleEnd (points.First (), true);
		}

		/// <summary>
		/// Toucheses the ended.
		/// </summary>
		/// <returns><c>true</c>, if ended was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesEnded (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesEnded (points);		
			return HandleEnd (points.First (), true);
		}

		/// <summary>
		/// Handles the end.
		/// </summary>
		/// <returns><c>true</c>, if end was handled, <c>false</c> otherwise.</returns>
		/// <param name="point">Point.</param>
		private bool HandleEnd(NGraphics.Point point, bool allowCancel)
		{
			if (!IsEnabled)
				return false;
			
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
			_ellipse.FadeTo (0.0, 50);

			return true;
		}

		/// <summary>
		/// The ShouldRipple property.
		/// </summary>
		public static BindableProperty ShouldRippleProperty = 
			BindableProperty.Create<RippleButton, bool> (p => p.ShouldRipple, true,
				BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
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
				(Content as RelativeLayout).ForceLayout ();
			}
		}
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
				IconLabel.Text = value;
			}
		}

		/// <summary>
		/// The IconColor property.
		/// </summary>
		public static BindableProperty IconColorProperty = 
			BindableProperty.Create<RippleButton, Color> (p => p.IconColor, Color.FromHex("#CECECE"),
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
				IconLabel.TextColor = value;
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
				_ellipse.BackgroundColor = value;
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
				if (Command != null)
					Command.CanExecuteChanged -= HandleCanExecuteChanged;

				SetValue (CommandProperty, value);
				IsEnabled = Command.CanExecute (CommandParameter);

				if (value != null)
					value.CanExecuteChanged += HandleCanExecuteChanged;
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
				TextLabel.Text = value;
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
				TextLabel.FontFamily = value;
			}
		}

		/// <summary>
		/// The IsEnabled property.
		/// </summary>
		public static new BindableProperty IsEnabledProperty = 
			BindableProperty.Create<RippleButton, bool> (p => p.IsEnabled, true,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RippleButton)bindable;
					ctrl.IsEnabled = newValue;
				});

		/// <summary>
		/// Gets or sets the IsEnabled of the RippleButton instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public new bool IsEnabled 
		{
			get{ return (bool)GetValue (IsEnabledProperty); }
			set {

				SetValue (IsEnabledProperty, value);

				if (value)
					TextLabel.TextColor = _orginalTextColor;
				else {
					_orginalTextColor = TextLabel.TextColor;
					TextLabel.TextColor = Color.FromHex ("#CCCCCC");
				}
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
				TextLabel.TextColor = value;
				_orginalTextColor = value;
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
				TextLabel.FontSize = value;
			}
		}

		#region Private Members

		/// <summary>
		/// Handles the can execute changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		private void HandleCanExecuteChanged(object sender, EventArgs args)
		{
			IsEnabled = Command.CanExecute (CommandParameter);
		}
		#endregion
	}
}

