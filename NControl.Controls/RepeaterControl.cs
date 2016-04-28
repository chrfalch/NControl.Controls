using System;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace NControl.Controls
{
	/// <summary>
	/// Implements a layout that uses item templates like a listview
	/// to build repeating controls. Inspired by:
	/// http://www.ericgrover.com/#!/post/1027
	/// </summary>
	public class RepeaterControl<T>: StackLayout where T : class
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="HomeDrive.App.RepeaterLayout`1"/> class.
		/// </summary>
		public RepeaterControl()
		{
			this.Spacing = 0;
		}

		#endregion

		#region Properties

		/// <summary>
		/// The items source property.
		/// </summary>
		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(nameof(RepeaterControl<T>.ItemsSource), typeof(ObservableCollection<T>), 
				typeof(RepeaterControl<T>), new ObservableCollection<T>(), BindingMode.OneWay, 
				propertyChanged:(bindable, oldValue, newValue) => {

					var control = bindable as RepeaterControl<T>;
					control.ItemsSource = newValue as ObservableCollection<T>;
				});

		/// <summary>
		/// Gets or sets the items source.
		/// </summary>
		/// <value>The items source.</value>
		public ObservableCollection<T> ItemsSource
		{
			get { return (ObservableCollection<T>)GetValue(ItemsSourceProperty); }
			set 
			{
				if (ItemsSource != null)
					ItemsSource.CollectionChanged -= ItemsSource_CollectionChanged;

				SetValue(ItemsSourceProperty, value); 

				if (ItemsSource != null)
					ItemsSource.CollectionChanged += ItemsSource_CollectionChanged;

				UpdateItems();
			}
		}

		/// <summary>
		/// The item template property.
		/// </summary>
		public static readonly BindableProperty ItemTemplateProperty =
			BindableProperty.Create(nameof(RepeaterControl<T>.ItemsSourceProperty), typeof(DataTemplate),
				typeof(RepeaterControl<T>), default(DataTemplate));

		/// <summary>
		/// Gets or sets the item template.
		/// </summary>
		/// <value>The item template.</value>
		public DataTemplate ItemTemplate
		{
			get { return (DataTemplate)GetValue(ItemTemplateProperty); }
			set 
			{ 
				SetValue(ItemTemplateProperty, value); 
				UpdateItems ();
			}
		}

		#endregion

		#region Private Members

		/// <summary>
		/// Updates the items.
		/// </summary>
		private void UpdateItems()
		{
			Children.Clear();

			if(ItemsSource != null && this.ItemTemplate != null)
			{
				foreach (T item in ItemsSource)
				{
					var view = this.ItemTemplate.CreateContent() as View;
					if (view == null)
						throw new InvalidOperationException("DataTemplates needs to inherit from the View class");

					view.BindingContext = item;
					this.Children.Insert(ItemsSource.IndexOf(item), view);
				}

				this.UpdateChildrenLayout();
				this.InvalidateLayout();
			}
		}
		#endregion

		#region Event Handlers

		/// <summary>
		/// Itemses the source collection changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		void ItemsSource_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{                        
			UpdateItems();
		}

		#endregion
	}
}

