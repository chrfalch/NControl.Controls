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

		/// <summary>
		/// The delta.
		/// </summary>
		private double _delta;

		/// <summary>
		/// The start time.
		/// </summary>
		private DateTime _startTime;

		#endregion

		#region Events

		/// <summary>
		/// The sender.
		/// </summary>
		public delegate void OnPageChanged(object sender, int oldIndex, int newIndex);

		/// <summary>
		/// Occurs when on page changed.
		/// </summary>
		public event OnPageChanged PageChanged;

		/// <summary>
		/// Occurs when clicked.
		/// </summary>
		public event EventHandler Clicked;

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

			_childLayout = new Grid ();
			var layout = new Grid ();

			// Touch handling
			var delta = 0.0;
			var startTime = DateTime.MinValue;

			// Handle child changes
			_children.CollectionChanged += (sender, e) => {

				_childLayout.BatchBegin();
				_childLayout.Children.Clear();

				for(var i=0; i<_children.Count; i++)
					_childLayout.Children.Add(_children[i]);				

				_childLayout.BatchCommit();
			};

			layout.Children.Add (_childLayout);

			Content = layout;
		}

		#region Touches

		/// <summary>
		/// Toucheses the began.
		/// </summary>
		/// <returns><c>true</c>, if began was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesBegan (IEnumerable<NGraphics.Point> points)
		{
			base.TouchesBegan (points);

			// Touch down
			var first = points.FirstOrDefault();
			_delta = first.X;
			_startTime = DateTime.Now;

			return true;
		}

		/// <summary>
		/// Toucheses the moved.
		/// </summary>
		/// <returns><c>true</c>, if moved was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesMoved (IEnumerable<NGraphics.Point> points)
		{
			base.TouchesMoved (points);

			// Touches moved
			var first = points.FirstOrDefault();
			var firstChild = Children.FirstOrDefault();
			var diff = first.X - _delta - (firstChild != null ? Width*GetIndexOfPage(Page):0);
			SetTranslationAsync(diff, false);

			return true;
		}

		/// <summary>
		/// Toucheses the ended.
		/// </summary>
		/// <returns><c>true</c>, if ended was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesEnded (IEnumerable<NGraphics.Point> points)
		{
			base.TouchesEnded (points);

			if (ShouldBeClick (points.FirstOrDefault ().X, _delta)) {
				if (Clicked != null)
					Clicked (this, EventArgs.Empty);
				
				return false;
			}

			HandleEndAsync(points.FirstOrDefault().X, _delta, _startTime, DateTime.Now);
			return true;
		}

		/// <summary>
		/// Toucheses the cancelled.
		/// </summary>
		/// <returns><c>true</c>, if cancelled was touchesed, <c>false</c> otherwise.</returns>
		/// <param name="points">Points.</param>
		public override bool TouchesCancelled (IEnumerable<NGraphics.Point> points)
		{
			base.TouchesCancelled (points);

			if (ShouldBeClick (points.FirstOrDefault ().X, _delta)){
				if (Clicked != null)
					Clicked (this, EventArgs.Empty);

				return false;
			}

			HandleEndAsync(points.FirstOrDefault().X, _delta, _startTime, DateTime.Now);
			return true;
		}

		#endregion

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

		#region Public Members

		/// <summary>
		/// Activates the page.
		/// </summary>
		/// <param name="view">View.</param>
		/// <param name="b">If set to <c>true</c> b.</param>
		public void ActivatePage (View view, bool animate)
		{
			if (Width == -1)
				_activePage = view;
			else
				ActivateAsync (view, animate);
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
		private async Task ActivateAsync (View value, bool animated, double delta, DateTime startTime, DateTime endTime)
		{
			if (_activePage == value)
				return;

			var oldIndex = _childLayout.Children.IndexOf (_activePage);
			var newIndex = _childLayout.Children.IndexOf (value);
			_activePage = value;
			var translationTo = -Width * GetIndexOfPage (value);
			await SetTranslationAsync (translationTo, animated, delta, startTime, endTime);

			if (PageChanged != null)
				PageChanged (this, oldIndex, newIndex);
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
			double speed = 250.0;

			// velocity:
			if(startTime != DateTime.MinValue)
			{
				var deltaT = (endTime - startTime).Milliseconds;
				double velocity_X = (double)deltaT / (double)delta;
				speed = Math.Abs((translation  -_childLayout.TranslationX) * velocity_X);
			}

			var animation = new Animation ();

			if (animate)
				animation.Add(0.0, 1.0, new Animation((d) => _childLayout.TranslationX = d,
					_childLayout.TranslationX, translation));
			else
				_childLayout.TranslationX = translation;			

			if (animate) 
			{
				var endAnimation = new Animation ((d) => {
				}, 0, 1, Easing.CubicInOut, () => {
					tcs.SetResult (true);
					System.Diagnostics.Debug.WriteLine (_childLayout.TranslationX);
				});

				animation.Add (0.0, 1.0, endAnimation);
				animation.Commit (this, "Translate", 16, Math.Min (350, (uint)speed), Easing.CubicOut);
			} 
			else 
			{
				tcs.SetResult (true);
			}

			System.Diagnostics.Debug.WriteLine (_childLayout.TranslationX);

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
		/// Shoulds the change page.
		/// </summary>
		/// <returns><c>true</c>, if change page was shoulded, <c>false</c> otherwise.</returns>
		/// <param name="endX">End x.</param>
		/// <param name="delta">Delta.</param>
		private bool ShouldBeClick(double endX, double delta)
		{
			var diff = Math.Abs(endX - delta);
			if (diff < 10)
				return true;

			return false;
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

		#region Overridden Members

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

			var newx = 0.0;
			for (int i = 0; i < Children.Count; i++) {
				Children [i].TranslationX = newx;
				newx += width;
			}

			if (_activePage != null)
				_childLayout.TranslationX = Children.IndexOf (_activePage) * -Width;
		}

		#endregion
	}
}
		