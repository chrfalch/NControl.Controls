using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class PagingViewPage: ContentPage
	{
		public PagingViewPage ()
		{
			Title = "Pager";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			Content = new StackLayout {
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new PagingView {
						PageCount = 10,
						Page = 1,
					},
					new PagingView {
						PageCount = 5,
						Page = 3,
						IndicatorColor = Color.Blue,
					}
				}
			};
		}
	}
}

