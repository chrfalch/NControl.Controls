using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class FontAwesomeLabelPage: ContentPage
	{
		public FontAwesomeLabelPage ()
		{
			Title = "FontAwesomeLabel";	
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

		    const int count = 0xf2e0 - 0xf000;
            const int colCount = 10;

		    var rowLayout = new StackLayout() { 			                
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
			};

			var c = 0xf000;
			for (var row = 0; row < count / colCount; row++)
			{
			    var colLayout = new StackLayout
			    {
			        Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.StartAndExpand,                    
			    };

				for (var col = 0; col < colCount; col++) {
					var label = new FontAwesomeLabel
					{
					    Text = ((char) c++).ToString(),
                        HorizontalOptions = LayoutOptions.CenterAndExpand,
					};
                    colLayout.Children.Add(label);
				}
                rowLayout.Children.Add(colLayout);
			}

            Content = new ScrollView { Margin = 14, Content = rowLayout };
		}
	}
}

