using System;
using Xamarin.Forms.Platform.Android;
using NControl.Controls;
using Xamarin.Forms;
using NControl.Controls.Droid;
using Android.Views;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]
namespace NControl.Controls.Droid
{
	public class ExtendedEntryRenderer: EntryRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			Control.SetBackgroundColor (Android.Graphics.Color.Transparent);
			Control.SetPadding (10, 0, 0, 0);

			UpdateGravity ();
		}
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);
			if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
				UpdateGravity ();
			else if (e.PropertyName == Entry.FontFamilyProperty.PropertyName)
				UpdateFont ();
		}

		/// <summary>
		/// Updates the font.
		/// </summary>
		private void UpdateFont ()
		{
			
		}

		/// <summary>
		/// Updates the text alignment.
		/// </summary>
		private void UpdateGravity ()
		{
			var element = (ExtendedEntry)Element;

			GravityFlags gravityFlags = GravityFlags.AxisSpecified;
			if (element.HorizontalTextAlignment == Xamarin.Forms.TextAlignment.Start) {
				gravityFlags = GravityFlags.Left;
			}
			else {
				if (element.HorizontalTextAlignment == Xamarin.Forms.TextAlignment.End) {
					gravityFlags = GravityFlags.Right;
				}
			}

			Control.Gravity = (gravityFlags);
		}
	}
}

