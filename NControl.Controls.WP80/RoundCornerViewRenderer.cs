using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using NControl.Abstractions;
using NControl.Controls;
using NControl.Controls.WP80;
using NControl.WP80;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: ExportRenderer(typeof(RoundCornerView), typeof(RoundCornerViewRenderer))]
namespace NControl.Controls.WP80
{
    public class RoundCornerViewRenderer : NControlViewRenderer
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
                e.PropertyName == RoundCornerView.CornerRadiusProperty.PropertyName ||
                e.PropertyName == RoundCornerView.BorderWidthProperty.PropertyName)
                UpdateElement(Element as RoundCornerView);            
        }

        /// <summary>
        /// Updates the clip rectangle
        /// </summary>
        protected override void UpdateClip()
        {
            base.UpdateClip();

            if (Control.ActualWidth == 0 || Control.ActualHeight == 0)
                return;

            if (Element.IsClippedToBounds)
            {
                var width = Control.ActualWidth - ((Element as RoundCornerView).BorderWidth * 4);
                var height = Control.ActualHeight - ((Element as RoundCornerView).BorderWidth * 4);
                if (width > 0 && height > 0) {

                    Clip = new RectangleGeometry
                    {
                        Rect = new Rect(0, 0, Control.ActualWidth, Control.ActualHeight),
                        RadiusX = (Element as RoundCornerView).CornerRadius,
                        RadiusY = (Element as RoundCornerView).CornerRadius,
                        
                    };

                    if (Control.Child != null)
                    {
                        var el = Element as RoundCornerView;
                    
                        Control.Child.Clip = new RectangleGeometry
                        {
                            Rect = new Rect((Element as RoundCornerView).BorderWidth,
                                (Element as RoundCornerView).BorderWidth, width, height),
                            RadiusX = (Element as RoundCornerView).CornerRadius,
                            RadiusY = (Element as RoundCornerView).CornerRadius,
                        };
                    }
                }
            }
            else
            {
                if (Control.Child != null)
                    Control.Child.Clip = null;

                Clip = null;
            }
        }

        /// <summary>
        /// Updates the element.
        /// </summary>
        /// <param name="element">Element.</param>
        private void UpdateElement(RoundCornerView element)
        {
            var colorConverter = new ColorConverter();

            Control.Background = (Brush)colorConverter.Convert(element.BackgroundColor, null, null, null);
            Control.BorderBrush = (Brush)colorConverter.Convert(element.BorderColor, null, null, null);
            Control.BorderThickness = new System.Windows.Thickness(element.BorderWidth);
            Control.CornerRadius = new CornerRadius(element.CornerRadius);

            UpdateClip();
        }
    }
}
