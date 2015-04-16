using System;
using System.Reflection;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace NControl.Controls.Demo.FormsApp
{
	public class BlurViewPage: ContentPage
	{
		public BlurViewPage ()
		{
			Title = "BlurView";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			var layout = new RelativeLayout ();
			Content = layout;

			// Add image
			var image = new BlurImageView {
				Aspect = Aspect.AspectFill,
				Source = ImageSource.FromStream(() => 
					this.GetType().GetTypeInfo().Assembly.GetManifestResourceStream(
						"NControl.Controls.Demo.FormsApp.Resources.Images.kaffe.jpg"))};

			layout.Children.Add (image, () => new Rectangle (150, 100, 130, 100));
	
		}
	}
}

