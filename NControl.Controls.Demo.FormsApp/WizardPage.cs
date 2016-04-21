using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class WizardPage: ContentPage
	{
		public WizardPage ()
		{
			Title = "WizardLayout";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			var wizard = new WizardLayout ();
			wizard.Pages = new View[]{
				new Button {
					Command = new Command((obj)=>wizard.Page++),
					Text = "Page 2",
				},

				new Button {
					Command = new Command((obj)=>wizard.Page++),
					Text = "Page 3",
				},

				new Button {
					Command = new Command((obj)=>wizard.Page++),
					Text = "Page 4",
				},

				new Button {
					Text = "Done",
				}
			};

			Content = wizard;
		}
	}
}

