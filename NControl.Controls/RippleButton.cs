using System;
using Xamarin.Forms;
using NControl.Abstractions;
using System.Linq;
using System.Windows.Input;
using System.Threading.Tasks;

namespace NControl.Controls
{
	/// <summary>
	/// Ripple button.
	/// </summary>
	public class RippleButton: RoundCornerView
	{		
		protected readonly NControlView _ellipse;
		public readonly Label TextLabel;
		protected readonly FontAwesomeLabel _iconLabel;

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
				(layout.Width / 2) - (Math.Min(Math.Min (layout.Width, layout.Height), 44) / 2),
				(layout.Height / 2) - (Math.Min(Math.Min (layout.Width, layout.Height), 44) / 2),
				Math.Min(Math.Min (layout.Width, layout.Height), 44), 
				Math.Min(Math.Min (layout.Width, layout.Height), 44)));
		
			TextLabel = new Label{ 
				BackgroundColor = Color.Transparent,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
			};

			layout.Children.Add (TextLabel, ()=> layout.Bounds);

			_iconLabel = new FontAwesomeLabel {
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
				TextColor = IconColor,
			};

			layout.Children.Add (_iconLabel, () => new Rectangle (14, 0, 32, layout.Height));
		}		

		/// <summary>
		/// Ripple this instance.
		/// </summary>
		public async Task RippleAsync(double x, double y)
		{
			var position = new Point (x, y);

			_ellipse.Scale = 0.0;
			_ellipse.Opacity = 1.0;

				var layout = Content as RelativeLayout;
			_ellipse.TranslationX = -((layout.Width / 2) - position.X);
			_ellipse.TranslationY = -((layout.Height / 2) - position.Y);

			await _ellipse.ScaleTo (8, easing: Easing.CubicInOut);
			_ellipse.Opacity = 0;

		}

		/// <summary>
		/// Toucheses the began.
		/// </summary>
		/// <returns><c>true</c>, if began was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesBegan (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesBegan (points);

			Device.BeginInvokeOnMainThread (async () => {

				var firstPoint = points.FirstOrDefault ();
				await RippleAsync(firstPoint.X, firstPoint.Y);
			});

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

			// Execute command
			if (Command != null && Command.CanExecute (CommandParameter))
				Command.Execute (CommandParameter);
			
			return true;
		}

		/// <summary>
		/// Toucheses the ended.
		/// </summary>
		/// <returns><c>true</c>, if ended was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesEnded (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesEnded (points);

			// Execute command
			if (Command != null && Command.CanExecute (CommandParameter))
				Command.Execute (CommandParameter);

			return true;
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
				_iconLabel.Text = value;
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
				_iconLabel.TextColor = value;
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
				SetValue (CommandProperty, value);
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
	}
}

