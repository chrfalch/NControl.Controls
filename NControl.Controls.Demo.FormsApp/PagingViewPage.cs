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

			var pager = new PagingView {
				PageCount = 10,			
			};
			pager.BindingContext = this;
			pager.SetBinding (PagingView.PageProperty, "Page");

			var btn = new Button{ 
				Text = "Count",
				Command = new Command((obj) => {
					Page++;
				}),
			};

			Content = new StackLayout {
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					pager,
					btn,
					new PagingView {
						PageCount = 5,
						Page = 3,
						IndicatorColor = Color.Blue,
					}
				}
			};
		}

		/// <summary>
		/// The Page property.
		/// </summary>
		public static BindableProperty PageProperty = 
			BindableProperty.Create<PagingViewPage, int> (p => p.Page, 0,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (PagingViewPage)bindable;
					ctrl.Page = newValue;
				});

		/// <summary>
		/// Gets or sets the Page of the PagingViewPage instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public int Page {
			get{ return (int)GetValue (PageProperty); }
			set { SetValue (PageProperty, value); }
		}
	}
}

