using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class CustomFontPage: ContentPage
	{
		public CustomFontPage ()
		{
			Title = "Custom Font";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			Content = new StackLayout {
				Children = {
					new Label () {
						HeightRequest = 155,
						Text = "Custom Font",
						FontFamily = "Clink Clank",
						BackgroundColor = Xamarin.Forms.Color.White,
						HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center,
                        VerticalTextAlignment = Xamarin.Forms.TextAlignment.Center,
						TextColor = Xamarin.Forms.Color.Blue,
						FontSize = 24,
					},
					new Label() {
						HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center,
                        VerticalTextAlignment  = Xamarin.Forms.TextAlignment.Center,
						FontSize = 10,
						Text = "This font is added to the demo project as " + 
							"an embedded resource. The font loader in " + 
							"NControl.Controls detects all font resources and " + 
							"makes them available without any additional work!"
					}
				}
			};
		}
	}
}

