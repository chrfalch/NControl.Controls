using System;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
	public class FloatingLabelPage: ContentPage
	{
		public FloatingLabelPage ()
		{
			Title = "FloatingLabel";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			var entry1 = new FloatingLabelControl{ Placeholder = "Email", Keyboard = Keyboard.Email};
            var entry2 = new FloatingLabelControl { Placeholder = "First name", Keyboard = Keyboard.Text };
            var entry3 = new FloatingLabelControl { Placeholder = "Last name", Keyboard = Keyboard.Text };
			var entry4 = new FloatingLabelControl{ 
				Placeholder = "Price", 
				Keyboard = Keyboard.Numeric, 
				Postfix = "USD", 
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

			var entry5 = new FloatingLabelControl{ 
				Placeholder = "Distance", 
				Keyboard = Keyboard.Numeric, 
				Postfix = "Mi", 
				HorizontalOptions = LayoutOptions.FillAndExpand
			};

		    var entry6 = new FloatingLabelControl
		    {
                IsPassword = true,
                Placeholder = "Enter your secret",
		    };

			var entry7 = new FloatingLabelControl
			{				
				Placeholder = "Enter location",
				PostfixIcon = FontAwesomeLabel.FAMapMarker,
				PostfixColor = Color.FromHex("#0000FF"),
			};

			Content = new ScrollView{
				Content = new StackLayout {
					Padding = 10,
					HorizontalOptions = LayoutOptions.CenterAndExpand,
					VerticalOptions = LayoutOptions.StartAndExpand,
					Orientation = StackOrientation.Vertical,
					Children = {
                        //entry1, 
                        //entry2, 
                        //entry3, 
                        //new StackLayout{
							
                        //    Orientation = StackOrientation.Horizontal,
                        //    HorizontalOptions = LayoutOptions.FillAndExpand,
                        //    VerticalOptions = LayoutOptions.CenterAndExpand,
                        //    Children = {
                        //        entry4, entry5
                        //    }
                        //},
                        //entry6,
						entry7
					}
				}
			};
		}
	}
}

