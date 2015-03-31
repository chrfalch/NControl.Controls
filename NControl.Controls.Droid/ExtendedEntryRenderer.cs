using System;
using Xamarin.Forms.Platform.Android;
using NControl.Controls;
using Xamarin.Forms;
using NControl.Controls.Droid;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]

namespace NControl.Controls.Droid
{
	public class ExtendedEntryRenderer: EntryRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged (e);

			this.Control.SetBackgroundDrawable(null);
		}
	}
}

