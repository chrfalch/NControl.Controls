using System;
using Xamarin.Forms;
using NControl.Abstractions;
using System.Threading.Tasks;

namespace NControl.Controls
{
	/// <summary>
	/// Card page. Based on custom transitions 
	/// </summary>
	public class CardPage: ContentPage
	{		
		#region Private Members

		/// <summary>
		/// The layout.
		/// </summary>
		private readonly RelativeLayout _layout;

		/// <summary>
		/// The overlay.
		/// </summary>
		private readonly RoundCornerView _contentView;

		/// <summary>
		/// The shadow view.
		/// </summary>
		private readonly NControlView _shadowView;

		/// <summary>
		/// The presenting layout.
		/// </summary>
		private RelativeLayout _presentingLayout;

		/// <summary>
		/// The overlay.
		/// </summary>
		private NControlView _overlay;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.CardPage"/> class.
		/// </summary>
		public CardPage()
		{						
			BackgroundColor = Color.Transparent;
			NavigationPage.SetHasNavigationBar (this, false);
			NavigationPage.SetHasBackButton (this, false);

			_layout = new RelativeLayout ();

			base.Content = _layout;

			// Shadow
			_shadowView = new NControlView {
				DrawingFunction = (canvas, rect) => {
					// TODO
				}
			};

			// Card 
			_contentView = new RoundCornerView {
				BackgroundColor = Color.White,
				CornerRadius = 4,
			};
				
			_layout.Children.Add(_shadowView, () => new Rectangle (CardInsets.X, CardInsets.Y, 
					(_layout.Width+2) - CardInsets.Width, (_layout.Height+1) - CardInsets.Height));
				
			_layout.Children.Add(_contentView, ()=> new Rectangle (CardInsets.X, CardInsets.Y, 
				(_layout.Width+2) - CardInsets.Width, (_layout.Height+1) - CardInsets.Height));

		}

		#region Public Members

		/// <summary>
		/// Pushs the card async.
		/// </summary>
		/// <returns>The card async.</returns>
		public async Task ShowCardPageAsync(RelativeLayout relativeLayout)
		{
			if (_presentingLayout != null)
				await HideCardPageAsync ();

			// Overlay
			_overlay = new NControlView{
				BackgroundColor = Color.Black,
				Opacity = 0.2,
			};

			relativeLayout.Children.Add (_overlay, ()=> relativeLayout.Bounds);
			relativeLayout.Children.Add (_layout, ()=> relativeLayout.Bounds);

			_presentingLayout = relativeLayout;
		}

		/// <summary>
		/// Pops the card.
		/// </summary>
		public async Task HideCardPageAsync()
		{
			if (_presentingLayout == null)
				return;

			_presentingLayout.Children.Remove (_layout);
			_presentingLayout.Children.Remove (_overlay);

			_presentingLayout = null;
			_overlay = null;
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets the View element representing the content of the Page.
		/// </summary>
		/// <value>The content.</value>
		public new View Content
		{
			get { return _contentView.Content; }
			set { _contentView.Content = value; }
		}

		#endregion

		#region Overridable Members

		/// <summary>
		/// Defines the insets/padding for the card
		/// </summary>
		/// <value>The card insets.</value>
		public virtual Rectangle CardInsets
		{
			get { return new Rectangle (40, 100, 80, 200); }
		}

		#endregion
	}
}

