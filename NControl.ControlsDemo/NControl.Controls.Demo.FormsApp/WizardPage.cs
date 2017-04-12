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

			WizardLayout wizard = null;
			wizard = new WizardLayout {
				Pages = {
				new Button {
					Command = new Command((obj)=>wizard.Page++),
					Text = "Page 1",
				},

				new Button {
					Command = new Command((obj)=>wizard.Page++),
					Text = "Page 2",
				},

				new Button {
					Command = new Command((obj)=>wizard.Page++),
					Text = "Page 3",
				},

				new Button {
					Text = "Done",
				}
				}
			};

			Content = wizard;
		}
	}
}

