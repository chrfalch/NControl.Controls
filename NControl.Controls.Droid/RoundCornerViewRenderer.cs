using System;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Graphics;
using NControl.Droid;

[assembly: ExportRenderer (typeof (RoundCornerView), typeof (RoundCornerViewRenderer))]
namespace NControl.Controls.Droid
{	
	public class RoundCornerViewRenderer: NControlViewRenderer
	{
		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == RoundCornerView.BackgroundColorProperty.PropertyName ||
				e.PropertyName == RoundCornerView.BorderColorProperty.PropertyName ||
				e.PropertyName == RoundCornerView.BorderWidthProperty.PropertyName)
				Invalidate ();
		}

		/// <Docs>The Canvas to which the View is rendered.</Docs>
		/// <summary>
		/// Draw the specified canvas.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		public override void Draw (Canvas canvas)
		{			
			try
			{
				var element = Element as RoundCornerView;
				var cornerRadius = (float)element.CornerRadius*Resources.DisplayMetrics.Density;

				// Paint rounded rect itself
				canvas.Save();
				var paint = new Paint();
				paint.AntiAlias = true;
				paint.StrokeWidth = (((float)element.BorderWidth)*Resources.DisplayMetrics.Density);

				if(element.BackgroundColor != Xamarin.Forms.Color.Transparent)
				{
					paint.SetStyle(Paint.Style.Fill);
					paint.Color = element.BackgroundColor.ToAndroid();
					canvas.DrawRoundRect(new RectF(0, 0, Width, Height), cornerRadius, cornerRadius, paint);
				}

				if(element.BorderColor != Xamarin.Forms.Color.Transparent)
				{
					paint.SetStyle(Paint.Style.Stroke);
					paint.Color = element.BorderColor.ToAndroid();
					canvas.DrawRoundRect(new RectF(0, 0, Width, Height), cornerRadius, cornerRadius, paint);
				}

				//Properly dispose
				paint.Dispose();
				canvas.Restore();

				// Create clip path
				var path = new Path();
				path.AddRoundRect(new RectF(0.0f, 0.0f, Width, Height), 
					cornerRadius, cornerRadius, Path.Direction.Cw);
				
				canvas.Save();
				canvas.ClipPath(path);

				// Do base drawing
				for(var i=0; i<ChildCount; i++)
					GetChildAt(i).Draw(canvas);

				canvas.Restore();
				path.Dispose();
			}
			catch (Exception)
			{				
			}				
		}
	}
}

