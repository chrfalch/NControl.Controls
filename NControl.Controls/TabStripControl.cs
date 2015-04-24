using System;
using NControl.Abstractions;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NControl.Controls
{
	/// <summary>
	/// Tab strip control.
	/// </summary>
	public class TabStripControl:NControlView
	{
		/// <summary>
		/// Tab strip location.
		/// </summary>
		public enum TabLocation
		{
			Top,
			Bottom
		}

		#region Private Members

		/// <summary>
		/// The list of views that can be displayed.
		/// </summary>
		private readonly ObservableCollection<TabChild> _children = 
			new ObservableCollection<TabChild> ();

		/// <summary>
		/// The tab control.
		/// </summary>
		private readonly NControlView _tabControl;

		/// <summary>
		/// The content view.
		/// </summary>
		private readonly Grid _contentView;

		/// <summary>
		/// The button stack.
		/// </summary>
		private readonly StackLayout _buttonStack;

		/// <summary>
		/// The indicator.
		/// </summary>
		private readonly TabBarIndicator _indicator;

		/// <summary>
		/// The layout.
		/// </summary>
		private readonly RelativeLayout _mainLayout;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.TabStripControl"/> class.
		/// </summary>
		public TabStripControl ()
		{
			_mainLayout = new RelativeLayout ();
			Content = _mainLayout;

			// Create tab control
			_buttonStack = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Padding = 0,
				Spacing = 0,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions= LayoutOptions.FillAndExpand,
			};

			_indicator = new TabBarIndicator {
				VerticalOptions = Location == TabLocation.Top ? LayoutOptions.End : LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.Start,
				HeightRequest = 6,
			};

			_tabControl = new NControlView{
				BackgroundColor = TabBackColor,
				Content = new Grid{
					Padding = 0,
					ColumnSpacing = 0,
					RowSpacing=0,
					Children = {
						_buttonStack,
						_indicator,
					}
				}
			};

			_mainLayout.Children.Add (_tabControl, () => new Rectangle (
				0, Location == TabLocation.Top ? 0 : _mainLayout.Height-TabHeight,
				_mainLayout.Width, TabHeight)
			);

			// Create content control
			_contentView = new Grid {
				ColumnSpacing = 0,
				RowSpacing = 0,
				Padding = 0,
				BackgroundColor = Color.Transparent,
			};

			_mainLayout.Children.Add (_contentView, () => new Rectangle (
				0, Location == TabLocation.Top ? TabHeight : 0,
				_mainLayout.Width, _mainLayout.Height-TabHeight)
			);

			_children.CollectionChanged += (sender, e) => {

				_contentView.Children.Clear();
				_buttonStack.Children.Clear();

				foreach(var tabChild in Children)
				{
					var tabItemControl = new TabBarButton(null, tabChild.Title, () => {
						Activate(tabChild, true);
					}){
						WidthRequest = _contentView.Width/Children.Count
					};
						
					_buttonStack.Children.Add(tabItemControl);
				}

				_indicator.WidthRequest = _contentView.Width/Children.Count;

				if(Children.Any())
					Activate(Children.First(), false);
			};

		}

		/// <param name="x">A value representing the x coordinate of the child region bounding box.</param>
		/// <param name="y">A value representing the y coordinate of the child region bounding box.</param>
		/// <param name="width">A value representing the width of the child region bounding box.</param>
		/// <param name="height">A value representing the height of the child region bounding box.</param>
		/// <summary>
		/// Positions and sizes the children of a Layout.
		/// </summary>
		/// <remarks>Implementors wishing to change the default behavior of a Layout should override this method. It is suggested to
		/// still call the base method and modify its calculated results.</remarks>
		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren (x, y, width, height);

			foreach (var view in _buttonStack.Children)
				view.WidthRequest = width / Children.Count;

			_indicator.WidthRequest = width/Children.Count;

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.TabStripControl"/> class.
		/// </summary>
		/// <param name="view">View.</param>
		public void Activate (TabChild tabChild, bool animate)
		{
			var existingChild = Children.FirstOrDefault (t => t.View == _contentView.Children.FirstOrDefault ());
			if (existingChild == tabChild)
				return;
			
			var idxOfExisting = existingChild != null ? Children.IndexOf (existingChild) : -1;
			var idxOfNew = Children.IndexOf (tabChild);

			if (idxOfExisting > -1 && animate) 
			{
				// Animate
				var translation = idxOfExisting < idxOfNew ? 
					_contentView.Width : - _contentView.Width;

				_contentView.Children.Add(tabChild.View);			

				var animation = new Animation ();
				var existingViewOutAnimation = new Animation ((d) => existingChild.View.TranslationX = d,
					0, -translation, Easing.CubicInOut, () => _contentView.Children.Remove (existingChild.View));

				var newViewInAnimation = new Animation ((d) => tabChild.View.TranslationX = d,
                     translation, 0, Easing.CubicInOut);

				var existingTranslation = _indicator.TranslationX;
				var itemWidth = (_contentView.Width / Children.Count ());
				var indicatorTranslation = itemWidth * idxOfNew;
				var indicatorViewAnimation = new Animation ((d) => _indicator.TranslationX = d,
					existingTranslation, indicatorTranslation, Easing.CubicInOut);

				animation.Add (0.0, 1.0, existingViewOutAnimation);
				animation.Add (0.0, 1.0, newViewInAnimation);
				animation.Add (0.0, 1.0, indicatorViewAnimation);
				animation.Commit (this, "TabAnimation");
			} 
			else 
			{
				// Just set first view
				_contentView.Children.Clear();
				_contentView.Children.Add(tabChild.View);
			}
		}

		/// <summary>
		/// Gets the views.
		/// </summary>
		/// <value>The views.</value>
		public IList<TabChild> Children
		{
			get{ return _children;}
		}

		/// <summary>
		/// The TabLocation property.
		/// </summary>
		public static BindableProperty TabLocationProperty = 
			BindableProperty.Create<TabStripControl, TabLocation> (p => p.Location, TabLocation.Top,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (TabStripControl)bindable;
					ctrl.Location = newValue;
				});

		/// <summary>
		/// Gets or sets the TabLocation of the TabStripControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public TabLocation Location {
			get{ return (TabLocation)GetValue (TabLocationProperty); }
			set {
				SetValue (TabLocationProperty, value);
				_indicator.VerticalOptions = Location == TabLocation.Top ? 
					LayoutOptions.End : LayoutOptions.Start;
				_mainLayout.ForceLayout ();
			}
		}

		/// <summary>
		/// The TabIndicatorColor property.
		/// </summary>
		public static BindableProperty TabIndicatorColorProperty = 
			BindableProperty.Create<TabStripControl, Color> (p => p.TabIndicatorColor, Color.Accent,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (TabStripControl)bindable;
					ctrl.TabIndicatorColor = newValue;
				});

		/// <summary>
		/// Gets or sets the TabIndicatorColor of the TabStripControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color TabIndicatorColor {
			get{ return (Color)GetValue (TabIndicatorColorProperty); }
			set {
				SetValue (TabIndicatorColorProperty, value);
				_indicator.Color = value;
			}
		}

		/// <summary>
		/// The TabHeight property.
		/// </summary>
		public static BindableProperty TabHeightProperty = 
			BindableProperty.Create<TabStripControl, double> (p => p.TabHeight, 40,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (TabStripControl)bindable;
					ctrl.TabHeight = newValue;
				});

		/// <summary>
		/// Gets or sets the TabHeight of the TabStripControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public double TabHeight {
			get{ return (double)GetValue (TabHeightProperty); }
			set {
				SetValue (TabHeightProperty, value);
			}
		}

		/// <summary>
		/// The TabBackColor property.
		/// </summary>
		public static BindableProperty TabBackColorProperty = 
			BindableProperty.Create<TabStripControl, Color> (p => p.TabBackColor, Color.White,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (TabStripControl)bindable;
					ctrl.TabBackColor = newValue;
				});

		/// <summary>
		/// Gets or sets the TabBackColor of the TabStripControl instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color TabBackColor {
			get{ return (Color)GetValue (TabBackColorProperty); }
			set {
				SetValue (TabBackColorProperty, value);
				_tabControl.BackgroundColor = value;
			}
		}

		/// <summary>
		/// Draw the specified canvas.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="rect">Rect.</param>
		public override void Draw (NGraphics.ICanvas canvas, NGraphics.Rect rect)
		{
			base.Draw (canvas, rect);

			if (Location == TabLocation.Top)
				canvas.DrawPath (new NGraphics.PathOp[]{
					new NGraphics.MoveTo(0, TabHeight),
					new NGraphics.LineTo(rect.Width, TabHeight)
				}, NGraphics.Pens.Gray, null);
			else
				canvas.DrawPath (new NGraphics.PathOp[]{
					new NGraphics.MoveTo(0, 0),
					new NGraphics.LineTo(rect.Width, 0)
				}, NGraphics.Pens.Gray, null);


		}
	}

	/// <summary>
	/// Tab child.
	/// </summary>
	public class TabChild
	{
		/// <summary>
		/// Gets the name.
		/// </summary>
		/// <value>The name.</value>
		public string Title { get; set; }

		/// <summary>
		/// Gets the view.
		/// </summary>
		/// <value>The view.</value>
		public View View { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.TabChild"/> class.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="view">View.</param>
		public TabChild(string title, View view)
		{
			Title = title;
			View = view;
		}
	}

	/// <summary>
	/// Tab bar indicator.
	/// </summary>
	public class TabBarIndicator: NControlView
	{
		/// <summary>
		/// The Color property.
		/// </summary>
		public static BindableProperty ColorProperty = 
			BindableProperty.Create<TabBarIndicator, Color> (p => p.Color, Color.Accent,
				propertyChanged: (bindable, oldValue, newValue) => {
					var ctrl = (TabBarIndicator)bindable;
					ctrl.Color = newValue;
				});

		/// <summary>
		/// Gets or sets the Color of the TabBarIndicator instance.
		/// </summary>
		/// <value>The color of the buton.</value>
		public Color Color {
			get{ return (Color)GetValue (ColorProperty); }
			set {
				SetValue (ColorProperty, value);
			}
		}

		/// <summary>
		/// Draw the specified canvas and rect.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="rect">Rect.</param>
		public override void Draw(NGraphics.ICanvas canvas, NGraphics.Rect rect)
		{
			base.Draw(canvas, rect);

			canvas.DrawRectangle (rect, null, new NGraphics.SolidBrush(this.Color.ToNColor ()));
		}            
	}

	/// <summary>
	/// Tab bar button.
	/// </summary>
	public class TabBarButton: NControlView
	{
		private readonly FontAwesomeLabel _imageLabel;
		private readonly FontAwesomeLabel _selectedImageLabel;
		private readonly Label _label;
		private readonly Action _clickCallback;

		public Color DarkTextColor = Color.Black;
		public Color AccentColor = Color.Accent;

		/// <summary>
		/// Initializes a new instance of the <see cref="Clooger.FormsApp.UserControls.TabBarButton"/> class.
		/// </summary>
		public TabBarButton(string imageName, string buttonText, Action callback)
		{
			_clickCallback = callback;

			if (!string.IsNullOrWhiteSpace (imageName)) {
				_imageLabel = new FontAwesomeLabel {
					Text = imageName,
					XAlign = TextAlignment.Center,
					YAlign = TextAlignment.Center,                
					TextColor = DarkTextColor,
					IsVisible = true
				};			

				_selectedImageLabel = new FontAwesomeLabel {
					Text = imageName,
					XAlign = TextAlignment.Center,
					YAlign = TextAlignment.Center,                
					TextColor = AccentColor,
					IsVisible = false,
				};
			}

			_label = new Label {
				Text = buttonText,
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,                
				FontSize = 12,
				LineBreakMode = LineBreakMode.TailTruncation,					
				TextColor = DarkTextColor,
			};

			Content = new StackLayout{
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.CenterAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = 10,
			};

			if (!string.IsNullOrWhiteSpace (imageName))
				(Content as StackLayout).Children.Add (new Grid { ColumnSpacing = 0, RowSpacing = 0, Padding = 0, 
					HeightRequest = 38,
					Children = { _imageLabel, _selectedImageLabel }
				});

			(Content as StackLayout).Children.Add (_label);
		}

		/// <summary>
		/// Draw the specified canvas.
		/// </summary>
		/// <param name="canvas">Canvas.</param>
		/// <param name="rect">Rect.</param>
		public override void Draw (NGraphics.ICanvas canvas, NGraphics.Rect rect)
		{
			base.Draw (canvas, rect);

//			canvas.DrawRectangle (new NGraphics.Rect (rect.Width, 0, rect.Width, rect.Height),
//				NGraphics.Pens.Gray, null);
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is selected.
		/// </summary>
		/// <value><c>true</c> if this instance is selected; otherwise, <c>false</c>.</value>
		public bool IsSelected
		{
			get
			{
				return _label.TextColor == AccentColor;
			}
			set
			{
				if (value)
					_label.TextColor = AccentColor;
				else
					_label.TextColor = DarkTextColor;

				_selectedImageLabel.IsVisible = value;
				_imageLabel.IsVisible = !value;
			}
		}

		/// <summary>
		/// Toucheses the began.
		/// </summary>
		/// <param name="points">Points.</param>
		public override bool TouchesBegan(System.Collections.Generic.IEnumerable<NGraphics.Point> points)
		{
			base.TouchesBegan(points);

			_label.TextColor = AccentColor;
			if (_clickCallback != null)
				_clickCallback();

			return true;
		}
	}
}

