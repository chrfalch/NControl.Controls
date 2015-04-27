using System;
using NControl.Abstractions;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace NControl.Controls
{
	/// <summary>
	/// Gallery view with paging and snapping
	/// </summary>
	public class GalleryView: RoundCornerView
	{
		#region Private Members

		/// <summary>
		/// The layout.
		/// </summary>
		private readonly Grid _childLayout;

		/// <summary>
		/// The children.
		/// </summary>
		private readonly ObservableCollection<View> _children = 
			new ObservableCollection<View>();

		/// <summary>
		/// The active page.
		/// </summary>
		private View _activePage;

		/// <summary>
		/// The padding.
		/// </summary>
		private Thickness _padding;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.GalleryView"/> class.
		/// </summary>
		public GalleryView ():base()
		{
			CornerRadius = 0;
			BorderWidth = 0.0;
			IsClippedToBounds = true;
			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;

			_childLayout = new RelativeLayoutWithTranslation ();
			var layout = new Grid ();

			// Touch handling
			var delta = 0.0;
			var startTime = DateTime.MinValue;
			var touchInterceptor = new NControlView {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			touchInterceptor.OnTouchesBegan += (object sender, IEnumerable<NGraphics.Point> e) => {

				// Touch down
				var first = e.FirstOrDefault();
				delta = first.X;
				startTime = DateTime.Now;
			};

			touchInterceptor.OnTouchesMoved += (object sender, IEnumerable<NGraphics.Point> e) => {

				// Touches moved
				var first = e.FirstOrDefault();
				var firstChild = Children.FirstOrDefault();
				var diff = first.X - delta - (firstChild != null ? Width*GetIndexOfPage(Page):0);
				SetTranslationAsync(diff, false);
			};

			touchInterceptor.OnTouchesCancelled +=async  (object sender, IEnumerable<NGraphics.Point> e) => {

				// Touches was cancelled
				await HandleEndAsync(e.FirstOrDefault().X, delta, startTime, DateTime.Now);
			};

			touchInterceptor.OnTouchesEnded += async (object sender, IEnumerable<NGraphics.Point> e) => {

				// Touches ended
				await HandleEndAsync(e.FirstOrDefault().X, delta, startTime, DateTime.Now);
			};

			// Handle child changes
			_children.CollectionChanged += (sender, e) => {

				_childLayout.BatchBegin();
				_childLayout.Children.Clear();

				for(var i=_children.Count-1; i>=0; i--)
					_childLayout.Children.Insert(0, _children[i]);

				_childLayout.BatchCommit();
			};

			layout.Children.Add (_childLayout);
			layout.Children.Add (touchInterceptor);

			Content = layout;
		}

		#region Properties

		/// <summary>
		/// To be added.
		/// </summary>
		/// <value>To be added.</value>
		/// <remarks>To be added.</remarks>
		public ObservableCollection<View> Children
		{
			get{
				return _children;
			}
		}

		/// <summary>
		/// Gets or sets the inner padding of the Layout.
		/// </summary>
		/// <value>The Thickness values for the layout. The default value is a Thickness with all values set to 0.</value>
		public new Thickness Padding
		{
			get { return _padding; }
			set{
				if (_padding == value)
					return;

				_padding = value;
			}
		}

		/// <summary>
		/// Gets or sets the page.
		/// </summary>
		/// <value>The page.</value>
		public View Page
		{
			get { 				
				return _activePage ?? Children.FirstOrDefault(); 
			}
			set {
				ActivateAsync (value, true);
			}
		}

		/// <summary>
		/// Gets the page count.
		/// </summary>
		/// <value>The page count.</value>
		public int PageCount
		{
			get { return _children.Count; }
		}
		#endregion

		#region Private Members

		/// <summary>
		/// Activates the async.
		/// </summary>
		/// <returns>The async.</returns>
		/// <param name="value">Value.</param>
		/// <param name="animated">If set to <c>true</c> animated.</param>
		private Task ActivateAsync(View value, bool animated)
		{
			return ActivateAsync (value, animated, 0.0, DateTime.MinValue, DateTime.MinValue);
		}

		/// <summary>
		/// Activates the specified value.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="b">If set to <c>true</c> b.</param>
		private Task ActivateAsync (View value, bool animated, double delta, DateTime startTime, DateTime endTime)
		{
			if (_activePage == value)
				return Task.FromResult (true);

			_activePage = value;
			var translation = -Width * GetIndexOfPage (value);
			return SetTranslationAsync (translation, true, delta, startTime, endTime);
		}

		/// <summary>
		/// Sets the translation async.
		/// </summary>
		/// <returns>The translation async.</returns>
		/// <param name="translation">Translation.</param>
		/// <param name="animate">If set to <c>true</c> animate.</param>
		private Task SetTranslationAsync(double translation, bool animate)
		{
			return SetTranslationAsync (translation, animate, 0.0, DateTime.MinValue, DateTime.MinValue);
		}

		/// <summary>
		/// Sets the translation.
		/// </summary>
		/// <param name="translation">Translation.</param>
		private Task SetTranslationAsync(double translation, bool animate, double delta,
			DateTime startTime, DateTime endTime)
		{
			if (Children.Count == 0)
				return Task.FromResult (true);
			
			var tcs = new TaskCompletionSource<bool> ();

			// velocity:
			var deltaT = (endTime - startTime).Milliseconds;
			double velocity_X = (double)deltaT / (double)delta;
			double speed = Math.Abs((translation  + (_activePage != null ? -_activePage.TranslationX : 0)) * velocity_X);

			if (startTime == DateTime.MinValue)
				speed = 250;

			var newx = 0.0;
			var animation = new Animation ();
			for (int i = 0; i < _childLayout.Children.Count; i++) {

				var child = _childLayout.Children [i];
				if (animate)
					animation.Add(0.0, 1.0, new Animation((d) => child.TranslationX = d,
						child.TranslationX, translation + newx));
				else
					child.TranslationX = translation + newx;
				
				newx += Width;
			}

			if (animate) {
				var endAnimation = new Animation ((d) => {
					}, 0, 1, Easing.CubicInOut, () => tcs.SetResult (true));

				animation.Add (0.0, 1.0, endAnimation);
				animation.Commit (this, "Translate", 16, Math.Min(350, (uint)speed), Easing.CubicOut);
			}
			else
				tcs.SetResult (true);

			return tcs.Task;
		}

		/// <summary>
		/// Gets the index of page.
		/// </summary>
		/// <returns>The index of page.</returns>
		/// <param name="page">Page.</param>
		private int GetIndexOfPage(View page)
		{
			return _children.IndexOf (page);
		}

		/// <summary>
		/// Handles the end.
		/// </summary>
		private async Task<bool> HandleEndAsync (double endX, double delta, 
			DateTime startTime, DateTime endTime)
		{
			var indexOfPage = GetIndexOfPage (Page);

			if (endX - delta < -(Width/3))
			{
				// select next if there are more items
				if (indexOfPage == PageCount - 1) {
					await SetTranslationAsync(-Width*GetIndexOfPage(Page), true);
					return false;
				}

				await ActivateAsync (_children [indexOfPage + 1], true, delta, startTime, endTime);

				return true;
			}  
			else if(endX - delta > (Width/3)) 
			{
				// select previous if we are not on the start
				if (indexOfPage <= 0) {
					await SetTranslationAsync (-Width*GetIndexOfPage(Page), true);
					return false;
				}

				await ActivateAsync (_children [indexOfPage - 1], true, delta, startTime, endTime);

				return true;
			}

			await SetTranslationAsync(-Width*GetIndexOfPage(Page), true);
			return false;
		}
		#endregion


	}

	/// <summary>
	/// Relative layout with translation.
	/// </summary>
	internal class RelativeLayoutWithTranslation: Grid
	{
		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.RelativeLayoutWithTranslation"/> class.
		/// </summary>
		internal RelativeLayoutWithTranslation(): base()
		{
			// Parent is clipping
			IsClippedToBounds = false;

			HorizontalOptions = LayoutOptions.FillAndExpand;
			VerticalOptions = LayoutOptions.FillAndExpand;
		}
		#endregion

		#region Overridden Members

		/// <summary>
		/// Positions and sizes the children of a Layout.
		/// </summary>
		/// <remarks>Implementors wishing to change the default behavior of a Layout should override this method. It is suggested to
		/// still call the base method and modify its calculated results.</remarks>
		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren (x, y, width, height);

			var newx = 0.0;
			for (int i = 0; i < Children.Count; i++) {
				Children[i].TranslationX = newx;
				newx += width;
			}
		}
		#endregion
	}
}

