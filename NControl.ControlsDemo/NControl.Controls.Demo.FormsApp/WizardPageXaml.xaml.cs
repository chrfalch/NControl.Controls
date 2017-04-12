using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public partial class WizardPageXaml : ContentPage
	{
		public WizardPageXaml()
		{
			InitializeComponent();
			Title = "WizardLayout XAML";		
		}

		void Handle_Clicked(object sender, System.EventArgs e)
		{
			this.wizard.Page++;
		}
	}
}
