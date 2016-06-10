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
            IsClippedToBounds = true;  
        }

		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren (x, y, width, height);
		}

        #region Properties

		/// <summary>
		/// The border width property.
		/// </summary>
		public static BindableProperty BorderWidthProperty = 
			BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(RoundCornerView), 0, 
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RoundCornerView)bindable;
					ctrl.BorderWidth = (double)newValue;
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
			BindableProperty.Create(nameof(BorderColor), typeof(Xamarin.Forms.Color), typeof(RoundCornerView), 
				Xamarin.Forms.Color.Transparent, 
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RoundCornerView)bindable;
					ctrl.BorderColor = (Xamarin.Forms.Color)newValue;
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
            BindableProperty.Create(nameof(CornerRadius), typeof(int), typeof(RoundCornerView), 4, 				
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (RoundCornerView)bindable;
					ctrl.CornerRadius = (int)newValue;
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

        #endregion

    }
}

