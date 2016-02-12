using System;
using Xamarin.Forms;
using System.Reflection;

namespace NControl.Controls.Demo.FormsApp
{

	public class SvgImagePage: ContentPage
	{
		public SvgImagePage ()
		{
			Title = "SvgImage";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
		
			Content = new ScrollView{
				Content = new StackLayout{
					Orientation = StackOrientation.Vertical,
					Children = {
						new StackLayout{
							Padding = 50,
							Orientation = StackOrientation.Vertical,
							Children = {
								new SvgImage{
									SvgAssembly = this.GetType().GetTypeInfo().Assembly,
									SvgResource = "NControl.Controls.Demo.FormsApp.Resources.Svg.Demo.svg",
								},
							}
						},
						new StackLayout{
							Padding = 4,
							BackgroundColor = Color.Aqua,
							Spacing = 4,
							HeightRequest = 200,
							Orientation = StackOrientation.Horizontal,
							Children = {
								new SvgImage{
									SvgAssembly = this.GetType().GetTypeInfo().Assembly,
									SvgResource = "NControl.Controls.Demo.FormsApp.Resources.Svg.Smile.svg",
								},
								new SvgImage{
									SvgAssembly = this.GetType().GetTypeInfo().Assembly,
									SvgResource = "NControl.Controls.Demo.FormsApp.Resources.Svg.Arrows.svg",
								},
							}
						},
						new StackLayout{
							Padding = 50,
							Orientation = StackOrientation.Vertical,
							Children = {
								new ContentView{
									HeightRequest = 100,
									BackgroundColor = Color.Red,
									HorizontalOptions = LayoutOptions.Center,
									Padding = 2,
									Content = 
										new SvgImage{
											WidthRequest = 100,
											SvgAssembly = this.GetType().GetTypeInfo().Assembly,
											SvgResource = "NControl.Controls.Demo.FormsApp.Resources.Svg.SpaceShips.svg",
										}
								},
								new ContentView{
									WidthRequest = 100,
									BackgroundColor = Color.Yellow,
									HorizontalOptions = LayoutOptions.Center,
									Padding = 2,
									Content = 
										new SvgImage{
										WidthRequest = 100,
										SvgAssembly = this.GetType().GetTypeInfo().Assembly,
										SvgResource = "NControl.Controls.Demo.FormsApp.Resources.Svg.SpaceShips.svg",
									}
								}
							}
						},
					}
				}
			};
		}
	}
}

