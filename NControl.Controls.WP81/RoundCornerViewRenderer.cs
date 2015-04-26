using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using NControl.Abstractions;
using NControl.Controls;
using NControl.Controls.WP81;
using NControl.WP81;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

//[assembly: ExportRenderer(typeof(RoundCornerView), typeof(RoundCornerViewRenderer))]
namespace NControl.Controls.WP81
{
    public class RoundCornerViewRenderer: NControlViewRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<NControlView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                var element = (e.NewElement as RoundCornerView);
                UpdateElement(element);
            }
        }
                
        /// <summary>
        /// Raises the element property changed event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == RoundCornerView.BackgroundColorProperty.PropertyName ||
                e.PropertyName == RoundCornerView.BorderColorProperty.PropertyName ||
                e.PropertyName == RoundCornerView.BorderWidthProperty.PropertyName)
                UpdateElement(Element as RoundCornerView);
        }

        private Canvas c;
        /// <summary>
        /// Updates the element.
        /// </summary>
        /// <param name="element">Element.</param>
        private void UpdateElement(RoundCornerView element)
        {
            if (c == null)
            {
                c = new Canvas();
                Canvas.Children.Insert(0, c);
            }

            if (element.BackgroundColor == Xamarin.Forms.Color.Transparent)
                c.Background = new SolidColorBrush(Colors.Transparent);
            else
                c.Background =
                    (Brush)new ColorConverter().Convert(element.BackgroundColor, null, null, null);

            Canvas.Clip = new EllipseGeometry {
                RadiusX = (Element as RoundCornerView).CornerRadius,
                RadiusY = (Element as RoundCornerView).CornerRadius,                
            };
            
        }
    }
}
