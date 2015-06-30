using System;
using NControl.Abstractions;
using Xamarin.Forms;

namespace NControl.Controls
{
	/// <summary>
	/// Paging view.
	/// </summary>
	public class PagingView: NControlView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.PagingView"/> class.
		/// </summary>
		public PagingView()
		{			
			HeightRequest = 22;			
		}

		/// <summary>
		/// Draw the specified canvas and rect.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="rect">Rect.</param>
		public override void Draw (NGraphics.ICanvas canvas, NGraphics.Rect rect)
		{
			base.Draw (canvas, rect);

			if (PageCount <= 0)
				return;
			
			var dotWidth = rect.Height - 16;
			var spacing = dotWidth;
			var totalWidthNeeded = (PageCount * (dotWidth + spacing)) - dotWidth;

			// Draw dots
			double midx = rect.Width / 2;
			double midy = rect.Height / 2;

			var x = midx - (totalWidthNeeded / 2);
			var pen = new NGraphics.Pen (IndicatorColor.ToNColor (), 0.5);
			var brush = new NGraphics.SolidBrush (IndicatorColor.ToNColor ()); 
			for (int i = 0; i < PageCount; i++) {

				canvas.DrawEllipse (new NGraphics.Rect (x, midy - (dotWidth / 2), 
					dotWidth, dotWidth), pen, i==Page ? brush : null);

				x += dotWidth + spacing;
			}

		}

		/// <summary>
		/// The PageCount property.
		/// </summary>
		public static BindableProperty PageCountProperty = 
			BindableProperty.Create<PagingView, int> (p => p.PageCount, 0,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (PagingView)bindable;
					ctrl.PageCount = newValue;
				});

		/// <summary>
		/// Gets or sets the PageCount of the PagingView instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public int PageCount {
			get{ return (int)GetValue (PageCountProperty); }
			set {
				SetValue (PageCountProperty, value);
				Invalidate ();
			}
		}

		/// <summary>
		/// The Page property.
		/// </summary>
		public static BindableProperty PageProperty = 
			BindableProperty.Create<PagingView, int> (p => p.Page, 0, BindingMode.TwoWay,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (PagingView)bindable;
					ctrl.Page = newValue;
				});

		/// <summary>
		/// Gets or sets the Page of the PagingView instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public int Page {
			get{ return (int)GetValue (PageProperty); }
			set {
				SetValue (PageProperty, value);
				Invalidate ();
			}
		}

		/// <summary>
		/// The IndicatorColor property.
		/// </summary>
		public static BindableProperty IndicatorColorProperty = 
			BindableProperty.Create<PagingView, Color> (p => p.IndicatorColor, Color.Gray,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (PagingView)bindable;
					ctrl.IndicatorColor = newValue;
				});

		/// <summary>
		/// Gets or sets the IndicatorColor of the PagingView instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color IndicatorColor {
			get{ return (Color)GetValue (IndicatorColorProperty); }
			set {
				SetValue (IndicatorColorProperty, value);
				Invalidate ();
			}
		}
	}
}

