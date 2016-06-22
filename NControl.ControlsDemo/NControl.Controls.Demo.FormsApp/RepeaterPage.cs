using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace NControl.Controls.Demo.FormsApp
{
	public class RepeaterPage: ContentPage
	{		
		public RepeaterPage ()
		{
			Title = "RepeaterControl";
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			Content = new RepeaterControl<ModelObject> {
				ItemsSource = new ObservableCollection<ModelObject>{
					new ModelObject{ Name = "Test 1" },
					new ModelObject{ Name = "Test 2" },
					new ModelObject{ Name = "Test 3" },
					new ModelObject{ Name = "Test 4" },
					new ModelObject{ Name = "Test 5" },
					new ModelObject{ Name = "Test 6" },
				},
				ItemTemplate = new DataTemplate(typeof(ModelView)),
			};
		}
	}

	public class ModelView: Label
	{
		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			if(BindingContext != null)
				Text = (BindingContext as ModelObject).Name;
		}
	}

	public class ModelObject
	{
		public string Name {get;set;}
	}
}

