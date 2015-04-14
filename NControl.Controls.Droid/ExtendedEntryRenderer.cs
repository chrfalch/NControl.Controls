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

			Control.SetBackgroundDrawable(null);
			Control.SetPadding (10, 0, 0, 0);	
		}
	}
}

