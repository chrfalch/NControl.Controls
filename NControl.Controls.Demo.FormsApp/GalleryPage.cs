using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class GalleryPage: ContentPage
	{
		public GalleryPage ()
		{
			Title = "GalleryView";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			var galleryView = new GalleryView {
				Padding = 10,
				BackgroundColor = Color.Gray,
				Children = {
					new Label {
						BackgroundColor = Color.Red, 
						Text = "First",
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment  = TextAlignment.Center
					}
					,
					new Label {
						BackgroundColor = Color.Green, 
						Text = "Second",
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment  = TextAlignment.Center 
					},
					new Label {
						BackgroundColor = Color.Aqua, 
						Text = "Third",
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment  = TextAlignment.Center 
					},
					new Label {
						BackgroundColor = Color.Blue, 
						Text = "Last",
                        HorizontalTextAlignment = TextAlignment.Center,
                        VerticalTextAlignment  = TextAlignment.Center 
					},
				}
			};

			galleryView.ActivatePage (galleryView.Children [1], false);

			Content = new StackLayout{
				Padding = 8,
				HeightRequest = 150,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Children = {
					galleryView
				}
			};
		}
	}
}

