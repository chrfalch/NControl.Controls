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
using Xamarin.Forms;
using NControl.Abstractions;
using NGraphics;

namespace NControl.Controls
{
    /// <summary>
    /// Rounded border control.
    /// </summary>
	public class RoundCornerView: NControlView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoundCornerView"/> class.
        /// </summary>
        public RoundCornerView()
        {
            base.BackgroundColor = Xamarin.Forms.Color.Transparent;
        }

        #region Properties

		/// <summary>
		/// The border width property.
		/// </summary>
		public static BindableProperty BorderWidthProperty = 
			BindableProperty.Create<RoundCornerView, double> (p => p.BorderWidth, 0, 
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RoundCornerView)bindable;
					ctrl.BorderWidth = newValue;
				});

		/// <summary>
		/// Gets or sets the border width
		/// </summary>
		/// <value>The corner radius.</value>
		public double BorderWidth 
		{
			get { return (double)GetValue(BorderWidthProperty); }
			set 
			{ 
				SetValue(BorderWidthProperty, value); 
				Invalidate ();
			}
		}

		/// <summary>
		/// The border color property.
		/// </summary>
		public static BindableProperty BorderColorProperty = 
			BindableProperty.Create<RoundCornerView, Xamarin.Forms.Color> (p => p.BorderColor, 
				Xamarin.Forms.Color.Transparent, 
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RoundCornerView)bindable;
					ctrl.BorderColor = newValue;
				});

		/// <summary>
		/// Gets or sets the border width
		/// </summary>
		/// <value>The corner radius.</value>
		public Xamarin.Forms.Color BorderColor 
		{
			get { return (Xamarin.Forms.Color)GetValue(BorderColorProperty); }
			set 
			{ 
				SetValue(BorderColorProperty, value); 
				Invalidate ();
			}
		}

        /// <summary>
        /// The corner radius property.
        /// </summary>
        public static BindableProperty CornerRadiusProperty = 
            BindableProperty.Create<RoundCornerView, int> (p => p.CornerRadius, 4, 
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RoundCornerView)bindable;
					ctrl.CornerRadius = newValue;
				});

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>The corner radius.</value>
        public int CornerRadius 
        {
            get { return (int)GetValue(CornerRadiusProperty); }
            set 
			{ 
				SetValue(CornerRadiusProperty, value); 
				Invalidate ();
			}
        }

        /// <summary>
        /// The border and fill color property.
        /// </summary>
        public static new BindableProperty BackgroundColorProperty = 
            BindableProperty.Create<RoundCornerView, Xamarin.Forms.Color> (
				p => p.BackgroundColor, Xamarin.Forms.Color.Transparent, propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RoundCornerView)bindable;
					ctrl.BackgroundColor = newValue;
				});

        /// <summary>
        /// Gets or sets the color which will fill the background of a VisualElement. This is a bindable property.
        /// </summary>
        /// <value>The color of the background.</value>
        public new Xamarin.Forms.Color BackgroundColor
        {
            get { return (Xamarin.Forms.Color)GetValue(BackgroundColorProperty); }
            set 
			{ 
				SetValue(BackgroundColorProperty, value); 
				Invalidate ();
			}
        }

        #endregion

        #region Drawing

//        /// <summary>
//        /// Draw the specified canvas.
//        /// </summary>
//        /// <param name="canvas">Canvas.</param>
//        /// <param name="rect">Rect.</param>
//        public override void Draw(NGraphics.ICanvas canvas, NGraphics.Rect rect)
//        {
//            base.Draw(canvas, rect);
//
//			var backgroundBrush = BackgroundColor == Xamarin.Forms.Color.Transparent ? 
//				null : new SolidBrush(BackgroundColor.ToNColor());
//			
//			var foregroundPen = BorderColor == Xamarin.Forms.Color.Transparent ? 
//				null : new Pen (BorderColor.ToNColor(), BorderWidth);
//            
//            var curveSize = CornerRadius;
//            var width = rect.Width;
//            var height = rect.Height;
//			var zero = 0.0;
//
//			if (BorderWidth > 0.0) {
//
//				canvas.DrawPath(new PathOp []{ 
//					new MoveTo(curveSize, zero),
//					// Top Right corner
//					new LineTo(width-curveSize, zero),
//					new CurveTo(
//						new NGraphics.Point(width-curveSize, zero),
//						new NGraphics.Point(width, zero),
//						new NGraphics.Point(width, curveSize)
//					),
//					new LineTo(width, height-curveSize),
//					// Bottom right corner
//					new CurveTo(
//						new NGraphics.Point(width, height-curveSize),
//						new NGraphics.Point(width, height),
//						new NGraphics.Point(width-curveSize, height)
//					),
//					new LineTo(curveSize, height),
//					// Bottom left corner
//					new CurveTo(
//						new NGraphics.Point(curveSize, height),
//						new NGraphics.Point(zero, height),
//						new NGraphics.Point(zero, height-curveSize)
//					),
//					new LineTo(zero, curveSize),
//					new CurveTo(
//						new NGraphics.Point(zero, curveSize),
//						new NGraphics.Point(zero, zero),
//						new NGraphics.Point(curveSize, zero)
//					),
//					new ClosePath()
//				}, foregroundPen, backgroundBrush);
//
//				width -= BorderWidth;
//				height -= BorderWidth;
//				zero += BorderWidth;
//			}
//
//            canvas.DrawPath(new PathOp []{ 
//				new MoveTo(curveSize, zero),
//				// Top Right corner
//				new LineTo(width-curveSize, zero),
//				new CurveTo(
//					new NGraphics.Point(width-curveSize, zero),
//					new NGraphics.Point(width, zero),
//					new NGraphics.Point(width, curveSize)
//				),
//				new LineTo(width, height-curveSize),
//				// Bottom right corner
//				new CurveTo(
//					new NGraphics.Point(width, height-curveSize),
//					new NGraphics.Point(width, height),
//					new NGraphics.Point(width-curveSize, height)
//				),
//				new LineTo(curveSize, height),
//				// Bottom left corner
//				new CurveTo(
//					new NGraphics.Point(curveSize, height),
//					new NGraphics.Point(zero, height),
//					new NGraphics.Point(zero, height-curveSize)
//				),
//				new LineTo(zero, curveSize),
//				new CurveTo(
//					new NGraphics.Point(zero, curveSize),
//					new NGraphics.Point(zero, zero),
//					new NGraphics.Point(curveSize, zero)
//				),
//				new ClosePath()
//			}, null, backgroundBrush);
//
//        }

        #endregion
    }
}

