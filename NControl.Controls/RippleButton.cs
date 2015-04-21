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
		private readonly NControlView _ellipse;
		private readonly Label _label;

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
		
			_label = new Label{ 
				BackgroundColor = Color.Transparent,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
			};

			layout.Children.Add (_label, ()=> layout.Bounds);
		}		

		/// <summary>
		/// Toucheses the began.
		/// </summary>
		/// <returns><c>true</c>, if began was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesBegan (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			_ellipse.Scale = 0.0;
			_ellipse.Opacity = 1.0;

			var firstPoint = points.FirstOrDefault ();

			var layout = Content as RelativeLayout;
			_ellipse.TranslationX = -((layout.Width/2)-firstPoint.X);
			_ellipse.TranslationY = -((layout.Height/2)-firstPoint.Y);
		
			Device.BeginInvokeOnMainThread(async () => {				
				await _ellipse.ScaleTo (8, easing:Easing.CubicInOut);
				_ellipse.Opacity = 0;
			});

			// Execute command
			if (Command != null && Command.CanExecute (CommandParameter))
				Command.Execute (CommandParameter);
			
			return base.TouchesBegan (points);
		}

		/// <summary>
		/// The RippleColor property.
		/// </summary>
		public static BindableProperty RippleColorProperty = 
			BindableProperty.Create<RippleButton, Color> (p => p.RippleColor, Color.FromHex("#CCCCCC"),
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
				_label.Text = value;
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
				_label.FontFamily = value;
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
				_label.TextColor = value;
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
				_label.FontSize = value;
			}
		}
	}
}

