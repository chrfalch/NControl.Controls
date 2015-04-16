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
using System.Windows.Input;
using System.Collections.Generic;

namespace NControl.Controls
{
	/// <summary>
	/// Implements a floating action button with a font awesome label as
	/// the icon. An action button is part of Google Material Design and 
	/// therefore has a few attributes that should be set correctly.
	/// 
	/// http://www.google.com/design/spec/components/buttons.html
	/// 
	/// 
	/// </summary>
	public class ActionButton: NControlView
	{
		#region Protected Members

		/// <summary>
		/// The button shadow element.
		/// </summary>
		protected readonly NControlView ButtonShadowElement;

		/// <summary>
		/// The button element.
		/// </summary>
		protected readonly NControlView ButtonElement;

		/// <summary>
		/// The button icon label.
		/// </summary>
		protected readonly FontAwesomeLabel ButtonIconLabel;

		#endregion

		#region Events

		/// <summary>
		/// Occurs when on clicked.
		/// </summary>
		public event EventHandler OnClicked;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.ActionButton"/> class.
		/// </summary>
		public ActionButton()
		{            
			var layout = new Grid{Padding = 0, ColumnSpacing = 0, RowSpacing = 0};

			ButtonShadowElement = new NControlView{ 
				DrawingFunction = (canvas, rect) => {

					// Draw shadow
					rect.Inflate(new NGraphics.Size(-4));
					rect.Y += 4;

					Device.OnPlatform(

						//iOS
						() => canvas.DrawEllipse (rect, null, new NGraphics.RadialGradientBrush (
							new NGraphics.Color(0, 0, 0, 200), NGraphics.Colors.Clear)),
						
						// Android
						() => canvas.DrawEllipse (rect, null, new NGraphics.RadialGradientBrush (
							new NGraphics.Point(rect.Width/2, (rect.Height/2)+2),
							new NGraphics.Size(rect.Width, rect.Height),
							new NGraphics.Color(0, 0, 0, 200), NGraphics.Colors.Clear)),
						
						// WP
						() => canvas.DrawEllipse (rect, null, new NGraphics.RadialGradientBrush (
							new NGraphics.Color(0, 0, 0, 200), NGraphics.Colors.Clear)),
						
						null
					);
				},
			};

			ButtonElement = new NControlView{ 
				DrawingFunction = (canvas, rect) => {

					// Draw button circle
					rect.Inflate (new NGraphics.Size (-8));
					canvas.DrawEllipse (rect, null, new NGraphics.SolidBrush(ButtonColor.ToNColor()));
				}
			};

			ButtonIconLabel = new FontAwesomeLabel{
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
				TextColor = Color.White,
				Text = FontAwesomeLabel.FAPlus,
				FontSize = 14,
			};

			layout.Children.Add (ButtonShadowElement);
			layout.Children.Add (ButtonElement);
			layout.Children.Add (ButtonIconLabel);

			Content = layout;
		}

		#region Properties

		/// <summary>
		/// The command property.
		/// </summary>
		public static BindableProperty CommandProperty = 
			BindableProperty.Create<ActionButton, ICommand> (p => p.Command, null,
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.Command = newValue;
				});

		/// <summary>
		/// Gets or sets the color of the buton.
		/// </summary>
		/// <value>The color of the buton.</value>
		public ICommand Command
		{
			get {  return (ICommand)GetValue (CommandProperty);}
			set {

				if (Command != null)
					Command.CanExecuteChanged -= HandleCanExecuteChanged;
				
				SetValue (CommandProperty, value);

				if (Command != null)
					Command.CanExecuteChanged += HandleCanExecuteChanged;
				
			}
		}

		/// <summary>
		/// The command parameter property.
		/// </summary>
		public static BindableProperty CommandParameterProperty = 
			BindableProperty.Create<ActionButton, object> (p => p.CommandParameter, null,
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.CommandParameter = newValue;
				});

		/// <summary>
		/// Gets or sets the color of the buton.
		/// </summary>
		/// <value>The color of the buton.</value>
		public object CommandParameter
		{
			get {  return GetValue (CommandParameterProperty);}
			set {
				SetValue (CommandParameterProperty, value);
			}
		}

		/// <summary>
		/// The button color property.
		/// </summary>
		public static BindableProperty ButtonColorProperty = 
			BindableProperty.Create<ActionButton, Color> (p => p.ButtonColor, Color.Gray,
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
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
				ButtonElement.Invalidate ();
			}
		}

		/// <summary>
		/// The button icon property.
		/// </summary>
		public static BindableProperty ButtonIconProperty = 
			BindableProperty.Create<ActionButton, string> (p => p.ButtonIcon, FontAwesomeLabel.FAPlus,
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.ButtonIcon = newValue;
				});

		/// <summary>
		/// Gets or sets the button icon.
		/// </summary>
		/// <value>The button icon.</value>
		public string ButtonIcon
		{
			get {  return (string)GetValue (ButtonIconProperty);}
			set {
				SetValue (ButtonIconProperty, value);
				ButtonIconLabel.Text = value;
			}
		}

		/// <summary>
		/// The button icon property.
		/// </summary>
		public static BindableProperty HasShadowProperty = 
			BindableProperty.Create<ActionButton, bool> (p => p.HasShadow, true,
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.HasShadow = newValue;
				});

		/// <summary>
		/// Gets or sets a value indicating whether this instance has shadow.
		/// </summary>
		/// <value><c>true</c> if this instance has shadow; otherwise, <c>false</c>.</value>
		public bool HasShadow
		{
			get {  return (bool)GetValue (HasShadowProperty);}
			set {
				SetValue (HasShadowProperty, value);

				if(value)
					ButtonShadowElement.FadeTo (1.0, 250);
				else
					ButtonShadowElement.FadeTo (0.0, 250);
			}
		}

		/// <summary>
		/// The button icon color property.
		/// </summary>
		public static BindableProperty ButtonIconColorProperty = 
			BindableProperty.Create<ActionButton, Color> (p => p.ButtonColor, Color.White,
				BindingMode.Default, null, (bindable, oldValue, newValue) => {
					var ctrl = (ActionButton)bindable;
					ctrl.ButtonIconColor = newValue;
				});

		/// <summary>
		/// Gets or sets the button icon color.
		/// </summary>
		/// <value>The button icon.</value>
		public Color ButtonIconColor
		{
			get {  return (Color)GetValue (ButtonIconColorProperty);}
			set {
				SetValue (ButtonIconColorProperty, value);
				ButtonIconLabel.TextColor = value;
			}
		}
			
		#endregion

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

		#region Touch Events

		/// <summary>
		/// Toucheses the began.
		/// </summary>
		/// <param name="points">Points.</param>
		public override bool TouchesBegan (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesBegan (points);

			if (!IsEnabled)
				return false;

			ButtonElement.ScaleTo (1.15, 140, Easing.CubicInOut);

			ButtonIconLabel.ScaleTo (1.2, 140, Easing.CubicInOut);

			ButtonShadowElement.TranslateTo (0.0, 3, 140, Easing.CubicInOut);
			ButtonShadowElement.ScaleTo (1.2, 140, Easing.CubicInOut);
			ButtonShadowElement.FadeTo (0.44, 140, Easing.CubicInOut);

			return true;
		}

		/// <summary>
		/// Toucheses the cancelled.
		/// </summary>
		/// <param name="points">Points.</param>
		public override bool TouchesCancelled (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesCancelled (points);

			if (!IsEnabled)
				return false;

			if (Command != null && Command.CanExecute (CommandParameter))
				Command.Execute (CommandParameter);			

			ButtonElement.ScaleTo (1.0, 140, Easing.CubicInOut);

			ButtonIconLabel.ScaleTo (1.0, 140, Easing.CubicInOut);

			ButtonShadowElement.TranslateTo (0.0, 0.0, 140, Easing.CubicInOut);
			ButtonShadowElement.ScaleTo (1.0, 140, Easing.CubicInOut);
			ButtonShadowElement.FadeTo (1.0, 140, Easing.CubicInOut);

			if (OnClicked != null)
				OnClicked (this, EventArgs.Empty);

			return true;
		}

		/// <summary>
		/// Toucheses the ended.
		/// </summary>
		/// <param name="points">Points.</param>
		public override bool TouchesEnded (System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesEnded (points);

			if (!IsEnabled)
				return false;

			if (Command != null && Command.CanExecute (CommandParameter))
				Command.Execute (CommandParameter);

			ButtonElement.ScaleTo (1.0, 140, Easing.CubicInOut);

			ButtonIconLabel.ScaleTo (1.0, 140, Easing.CubicInOut);

			ButtonShadowElement.TranslateTo (0.0, 0.0, 140, Easing.CubicInOut);
			ButtonShadowElement.ScaleTo (1.0, 140, Easing.CubicInOut);
			ButtonShadowElement.FadeTo (1.0, 140, Easing.CubicInOut);

			if (OnClicked != null)
				OnClicked (this, EventArgs.Empty);

			return true;
		}
		#endregion
	}
}

