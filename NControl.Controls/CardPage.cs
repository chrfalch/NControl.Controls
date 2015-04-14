using System;
using Xamarin.Forms;
using NControl.Abstractions;
using System.Threading.Tasks;

namespace NControl.Controls
{
	/// <summary>
	/// Card page. Based on custom transitions and screenshots
	/// http://danielhindrikes.se/xamarin/building-a-screenshotmanager-to-capture-the-screen-with-code/
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
		private readonly NControlView _overlay;

		/// <summary>
		/// The overlay.
		/// </summary>
		readonly private RoundCornerView _contentView;

		/// <summary>
		/// The shadow view.
		/// </summary>
		readonly private NControlView _shadowView;

		/// <summary>
		/// The background image.
		/// </summary>
		private Image _backgroundImage;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.CardPage"/> class.
		/// </summary>
		public CardPage()
		{			
			_layout = new RelativeLayout ();

			base.Content = _layout;

			// Background
			_backgroundImage = new Image{
				Aspect= Aspect.AspectFill,
			};

			// overlay
			_overlay = new NControlView {
				BackgroundColor = Color.FromHex("#000000"),
				Opacity = 0.0,
			};
			_overlay.OnTouchesBegan += (sender, e) => PopCardAsync();

			// Shadow
			_shadowView = new NControlView {
				DrawingFunction = (canvas, rect) => {
					// TODO
				}
			};

			// Card 
			_contentView = new RoundCornerView {
				BackgroundColor = Color.White,
				CornerRadius = 2,
			};

			_layout.Children.Add (_backgroundImage, () => _layout.Bounds);
			_layout.Children.Add (_overlay, ()=> _layout.Bounds);
			_layout.Children.Add(_shadowView, () => new Rectangle (CardInsets.Width-1, CardInsets.Height+1, 
					(_layout.Width+2) - CardInsets.Width*2, (_layout.Height+1) - CardInsets.Height*2));
				
			_layout.Children.Add(_contentView, ()=> new Rectangle (CardInsets.Width, CardInsets.Height, 
					_layout.Width - CardInsets.Width*2, _layout.Height - CardInsets.Height*2));

			// Start animation
			SetupStartStateForCard (_contentView);

		}

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			_overlay.FadeTo (0.5, 350, Easing.CubicInOut);

			AnimateStartForCard (_contentView);
		}

		/// <summary>
		/// Pops the card.
		/// </summary>
		public async Task PopCardAsync()
		{
			AnimateEndForCard (_contentView);
			await _overlay.FadeTo(0.0, 350, Easing.CubicInOut);
			await Navigation.PopModalAsync (false);
		}

		#region Properties

		/// <summary>
		/// Gets or sets the background image.
		/// </summary>
		/// <value>The background image.</value>
		public ImageSource BackgroundImageSource
		{
			get 
			{
				return _backgroundImage.Source;
			}
			set
			{
				_backgroundImage.Source = value;
			}
		}
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
		public virtual Size CardInsets
		{
			get { return new Size (40, 80); }
		}

		/// <summary>
		/// Sets the start translation for the card. By default
		/// only the opacity is set
		/// </summary>
		public virtual void SetupStartStateForCard(ContentView card)
		{
			card.Opacity = 0.0;
		}

		/// <summary>
		/// Animates the start for card.
		/// </summary>
		/// <param name="card">Card.</param>
		public virtual void AnimateStartForCard(ContentView card)
		{
			card.Animate ("FadeIn", (d) => card.Opacity = d, 0.0, 1.0, 350, easing: Easing.CubicInOut);
		}

		/// <summary>
		/// Animates the end for card.
		/// </summary>
		/// <param name="card">Card.</param>
		public virtual void AnimateEndForCard(ContentView card)
		{
			card.Animate ("FadeOut", (d) => card.Opacity = d, 1.0, 0.0, 350, easing: Easing.CubicInOut);
		}

		#endregion
	}
}

