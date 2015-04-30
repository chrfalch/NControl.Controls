using System;
using NControl.Controls.Droid;
using NControl.Controls;
using Xamarin.Forms;
using NControl.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Views;
using Android.Graphics.Drawables;
using Android.Graphics;
using Android.Renderscripts;
using Android.Widget;
using Android.Content;
using System.ComponentModel;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

[assembly: ExportRenderer (typeof (BlurImageView), typeof (BlurImageViewRenderer))]
namespace NControl.Controls.Droid
{
	/// <summary>
	/// Blur image view renderer.
	/// </summary>
	public class BlurImageViewRenderer: ImageRenderer
	{
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<Image> e)
		{
			base.OnElementChanged (e);

			// Update bitmap
			//UpdateBitmap();

			if (e.OldElement != null)
				e.OldElement.MeasureInvalidated -= HandleUpdateBitmap;

			if(e.NewElement != null)
				e.NewElement.MeasureInvalidated += HandleUpdateBitmap;
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged (object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == Image.SourceProperty.PropertyName) {
				// Update bitmap
				HandleUpdateBitmap(this, EventArgs.Empty);
				return;
			}
		}

		/// <summary>
		/// Updates the bitmap.
		/// </summary>
		private void HandleUpdateBitmap(object sender, EventArgs e)
		{
			if (Control == null)
				return;
			
			// Get bitmap from control
			var d = Control.Drawable;
			if (d != null) {
				if (d is BitmapDrawable) {
					var bmp = (d as BitmapDrawable).Bitmap;
					if (bmp == null)
						return;
					
					this.Control.SetImageBitmap(CreateBlurredImage(25, bmp));
					this.Control.Invalidate ();
				}
			}

		}

		/// <summary>
		/// Creates the blurred image.
		/// </summary>
		/// <returns>The blurred image.</returns>
		/// <param name="radius">Radius.</param>
		/// <param name="originalBitmap">Original bitmap.</param>
		private Bitmap CreateBlurredImage (int radius, Bitmap originalBitmap)
		{
			// Create another bitmap that will hold the results of the filter.
			Bitmap blurredBitmap;
			blurredBitmap = Bitmap.CreateBitmap (originalBitmap);

			// Create the Renderscript instance that will do the work.
			var rs = RenderScript.Create (Forms.Context);

			// Allocate memory for Renderscript to work with
			var input = Allocation.CreateFromBitmap (rs, originalBitmap, Allocation.MipmapControl.MipmapFull, AllocationUsage.Script);
			var output = Allocation.CreateTyped (rs, input.Type);

			// Load up an instance of the specific script that we want to use.
			var script = ScriptIntrinsicBlur.Create (rs, Android.Renderscripts.Element.U8_4 (rs));
			script.SetInput (input);

			// Set the blur radius
			script.SetRadius (radius);

			// Start the ScriptIntrinisicBlur
			script.ForEach (output);

			// Copy the output to the blurred bitmap
			output.CopyTo (blurredBitmap);

			return blurredBitmap;
		}
	}
}

