using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class MaterialDesignIconsPage: ContentPage
	{
		public MaterialDesignIconsPage ()
		{
			Title = "Material Design Icons";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			Content = new Grid {
				Padding = 16,
				Children = {
					new FontMaterialDesignLabel {
						Text = 
							FontMaterialDesignLabel.MD3dRotation + " " + 
							FontMaterialDesignLabel.MDAccountBox + " " + 
							FontMaterialDesignLabel.MDGasStation + " " + 
							FontMaterialDesignLabel.MDMenu
					}
				}
			};
		}
	}
}

