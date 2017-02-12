using System;
using Xamarin.Forms;
using NControl.Controls;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;

namespace NControl.Controls
{
    /// <summary>
    /// Wizard layout.
    /// </summary>
    public class WizardLayout: Grid
    {
        #region Private Members

        readonly WizardStackLayout _contentStack;
        readonly PagingView _pager;
		readonly ObservableCollection<View> _pages = new ObservableCollection<View>();

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:HomeDrive.App.WizardLayout"/> class.
        /// </summary>
        public WizardLayout()
        {
			_pages.CollectionChanged += PagesChanged;

            // Wrapping layout
            var layout = new RelativeLayout();
            Children.Add(layout);

			// Content
			_contentStack = new WizardStackLayout();

            // Pager
            _pager = new PagingView();
            _pager.Page = 0;

            var bottomHeight = 44;
            var pagerHeight = Device.OnPlatform<int>(22, 18, 18);
            layout.Children.Add(_contentStack, () => new Rectangle(0, 0, layout.Bounds.Width * GetChildCount(),
                layout.Bounds.Height - bottomHeight));

            layout.Children.Add(_pager, () => new Rectangle(0, layout.Bounds.Height - bottomHeight, 
                layout.Bounds.Width, pagerHeight));
			         
        }

        #region Properties

		/// <summary>
		/// The page property.
		/// </summary>
		public static BindableProperty PageProperty = BindableProperty.Create(nameof(Page),
			typeof(int), typeof(WizardLayout), 0, BindingMode.TwoWay, propertyChanged: (bindable, oldValue, newValue) => { 
				var ctrl = (WizardLayout)bindable;
				ctrl.Page = (int)newValue;
			});
		
        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>The page.</value>
        public int Page
        {
            get { return (int)GetValue(PageProperty); }
            set 
            { 
                SetValue(PageProperty, value);

                // focus first entry
                FocusFirstEntry();

                Device.BeginInvokeOnMainThread(async () => await _contentStack.TranslateTo(-(Width * value), 0));
                _pager.Page = value;
            }                
        }

        /// <summary>
        /// Gets or sets the pages.
        /// </summary>
        /// <value>The pages.</value>
        public IList<View> Pages
		{
			get { return _pages; }
		}

        #endregion

        #region Event Handlers

        /// <summary>
        /// Pageses the changed.
        /// </summary>
        /// <returns>The changed.</returns>
        /// <param name="sender">Sender.</param>
        /// <param name="e">E.</param>
        private void PagesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdatePages();      
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Updates the pages.
        /// </summary>
        /// <returns>The pages.</returns>
        private void UpdatePages()
        {
            _contentStack.Children.Clear();

            if (Pages != null)
            {
                foreach (var item in Pages)
                    _contentStack.Children.Add(new ContentView { Content = item });
                
                _pager.PageCount = _contentStack.Children.Count;

				FocusFirstEntry();
            }
            else
            {
                _pager.PageCount = 0;
            }
        }

        /// <summary>
        /// Gets the child count.
        /// </summary>
        /// <returns>The child count.</returns>
        private int GetChildCount()
        {
            return _contentStack.Children.Count;
        }

        /// <summary>
        /// Focuses the first entry.
        /// </summary>
        /// <returns>The first entry.</returns>
        private void FocusFirstEntry()
        {
            if (Page < _contentStack.Children.Count)
            {

                var activeControl = _contentStack.Children.ElementAt(Page);
                var firstEntry = GetFirstEntry((activeControl as ContentView).Content as IViewContainer<View>);
                if (firstEntry != null)
                    firstEntry.Focus();

            }
        }

        /// <summary>
        /// Gets the first entry.
        /// </summary>
        /// <returns>The first entry.</returns>
        /// <param name="viewContainer">View container.</param>
        private Entry GetFirstEntry(IViewContainer<View> viewContainer)
        {
            if (viewContainer == null)
                return null;
            
            var firstEntry = viewContainer.Children.FirstOrDefault(mx => mx is Entry) as Entry;
            if(firstEntry != null)
                return firstEntry;

            foreach (var child in viewContainer.Children)
                if (child is IViewContainer<View>)
                    return GetFirstEntry(child as IViewContainer<View>);

            return null;
        }
        #endregion
    }

    /// <summary>
    /// Wizard scroll view.
    /// </summary>
    public class WizardStackLayout : Layout<View>
    {
		/// <summary>
        /// Layouts the children.
        /// </summary>
        /// <returns>The children.</returns>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        protected override void LayoutChildren(double x, double y, double width, double height)
        {
            if (Children.Count == 0)
                return;

            var childWidth = (width / Children.Count);
            foreach (var child in Children)
            {
                child.Layout(new Rectangle(x, y, childWidth, height));
                x += childWidth;
            }
        }
    }
}

