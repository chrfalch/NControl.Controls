using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using NControl.Controls.iOS;

namespace NControl.Controls.Demo.FormsApp.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			NControls.Init ();

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

