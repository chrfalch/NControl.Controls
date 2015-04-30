using System;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.iOS;

[assembly: ExportRenderer (typeof (ExtendedEntry), typeof (ExtendedEntryRenderer))]
namespace NControl.Controls.iOS
{
	public class ExtendedEntryRenderer: EntryRenderer
	{
		/// <summary>
		/// Raises the element changed event.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnElementChanged (ElementChangedEventArgs<Xamarin.Forms.Entry> e)
		{
			base.OnElementChanged (e);

			var textfield = Control as UITextField;
			textfield.BorderStyle = UITextBorderStyle.None;
			textfield.TextAlignment = (e.NewElement as ExtendedEntry).XAlign.ToUITextAlignment ();
		}
	}
}

