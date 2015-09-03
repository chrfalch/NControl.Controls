using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp
{
    class CalendarPage : ContentPage
    {
        public CalendarPage()
        {
            Title = "Calendar";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var calendar = new CalendarView {
                HeightRequest = 350,
            };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Padding = 10,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    VerticalOptions = LayoutOptions.StartAndExpand,
                    Orientation = StackOrientation.Vertical,
                    Children = {
                        calendar
                    }
                }
            };
        }
    }
}
