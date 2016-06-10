using System;
using Xamarin.Forms;
using NControl.Controls;
using NControl.Controls.iOS;
using Xamarin.Forms.Platform.iOS;
using NControl.iOS;

[assembly: ExportRenderer (typeof (RoundCornerView), typeof (RoundCornerViewRenderer))]
namespace NControl.Controls.iOS
{	
	public class RoundCornerViewRenderer: NControlViewRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<NControl.Abstractions.NControlView> e)
		{
			base.OnElementChanged (e);
		
			if (e.NewElement != null) {
				
				Layer.MasksToBounds = true;
				var element = (e.NewElement as RoundCornerView);

				UpdateElement (element);
			}
		}

		/// <summary>
		/// Raises the element property changed event.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		protected override void OnElementPropertyChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged (sender, e);

			if (e.PropertyName == RoundCornerView.BackgroundColorProperty.PropertyName ||
				e.PropertyName == RoundCornerView.BorderColorProperty.PropertyName ||
				e.PropertyName == RoundCornerView.BorderWidthProperty.PropertyName ||
				e.PropertyName == RoundCornerView.CornerRadiusProperty.PropertyName )
				UpdateElement ( Element as RoundCornerView);
		}

		/// <summary>
		/// Updates the element.
		/// </summary>
		/// <param name="element">Element.</param>
		private void UpdateElement(RoundCornerView element)
		{
			if (element.BackgroundColor == Xamarin.Forms.Color.Transparent)
				BackgroundColor = UIKit.UIColor.Clear;
			else
				BackgroundColor = element.BackgroundColor.ToUIColor ();
			
			Layer.CornerRadius = element.CornerRadius;
			Layer.BorderColor = element.BorderColor.ToUIColor ().CGColor;
			Layer.BorderWidth = (nfloat)element.BorderWidth;
		}
	}
}