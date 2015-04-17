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
		readonly private RoundCornerView _contentView;

		/// <summary>
		/// The shadow view.
		/// </summary>
		readonly private NControlView _shadowView;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.CardPage"/> class.
		/// </summary>
		public CardPage()
		{						
			BackgroundColor = Color.Transparent;

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

			_contentView.OnTouchesBegan += (sender, e) => PopCardAsync();

			_layout.Children.Add(_shadowView, () => new Rectangle (CardInsets.X, CardInsets.Y, 
					(_layout.Width+2) - CardInsets.Width, (_layout.Height+1) - CardInsets.Height));
				
			_layout.Children.Add(_contentView, ()=> new Rectangle (CardInsets.X, CardInsets.Y, 
				(_layout.Width+2) - CardInsets.Width, (_layout.Height+1) - CardInsets.Height));

		}

//		/// <summary>
//		/// Raises the appearing event.
//		/// </summary>
//		protected override void OnAppearing ()
//		{
//			base.OnAppearing ();
//
//			AnimateStartForCard (_contentView);
//		}
//
		/// <summary>
		/// Pops the card.
		/// </summary>
		public async Task PopCardAsync()
		{
			await Navigation.PopAsync (true);
		}
//
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

//		/// <summary>
//		/// Sets the start translation for the card. By default
//		/// only the opacity is set
//		/// </summary>
//		public virtual void SetupStartStateForCard(ContentView card)
//		{
//			card.Opacity = 0.0;
//		}
//
//		/// <summary>
//		/// Animates the start for card.
//		/// </summary>
//		/// <param name="card">Card.</param>
//		public virtual void AnimateStartForCard(ContentView card)
//		{
//			card.Animate ("FadeIn", (d) => card.Opacity = d, 0.0, 1.0, 350, easing: Easing.CubicInOut);
//		}
//
//		/// <summary>
//		/// Animates the end for card.
//		/// </summary>
//		/// <param name="card">Card.</param>
//		public virtual void AnimateEndForCard(ContentView card)
//		{
//			card.Animate ("FadeOut", (d) => card.Opacity = d, 1.0, 0.0, 350, easing: Easing.CubicInOut);
//		}
//
		#endregion
	}
}

