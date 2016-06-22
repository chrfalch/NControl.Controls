using System;
using Xamarin.Forms;
using NControl.Abstractions;
using System.Threading.Tasks;

namespace NControl.Controls
{
    public enum CardPosition
    {
        Custom,
        Top,
        Bottom,
        Center
    }

    /// <summary>
	/// Card page. Based on custom transitions 
	/// </summary>
	public class CardPage: ContentPage
	{				
		#region Private Members

		/// <summary>
		/// The height of the requested.
		/// </summary>
		private double _requestedHeight = -1;

		/// <summary>
		/// The width of the requested.
		/// </summary>
		private double _requestedWidth = -1;

		/// <summary>
		/// The helper.
		/// </summary>
		private ICardPageHelper _platformHelper;

		/// <summary>
		/// The shadow.
		/// </summary>
		private readonly Frame _shadowLayer;

		/// <summary>
		/// The layout.
		/// </summary>
		private readonly RelativeLayout _layout;

		/// <summary>
		/// The overlay.
		/// </summary>
		private readonly BoxView _overlay;

		/// <summary>
		/// The overlay.
		/// </summary>
		private readonly ContentView _contentView;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="NControl.Controls.CardPage"/> class.
		/// </summary>
		public CardPage()
		{					
			// Get helper
			_platformHelper = DependencyService.Get<ICardPageHelper> ();
            if(_platformHelper == null)
                throw new InvalidOperationException("Error loading NControls - did you remember to call " + 
                    "NControls.Init() in your platform startup code?");

			_cardPadding = new Thickness (40, 100, 40, 100);
			BackgroundColor = Color.Transparent;
            _hasHaddow = true;

			NavigationPage.SetHasNavigationBar (this, true);
			NavigationPage.SetHasBackButton (this, false);

			_layout = new RelativeLayout ();
			base.Content = _layout;

			// Shadow 
			_shadowLayer = new Frame{
				BackgroundColor = Color.White,
				HasShadow = Device.OnPlatform<bool>(true, false, false),
			};

			_overlay = new BoxView {
				BackgroundColor = Color.Black,
				Opacity = 0.0F,
			};

            if (_platformHelper.ControlAnimatesItself)
            {
                // Card 
                _contentView = new RoundCornerView {
                    BackgroundColor = Color.White,
                    CornerRadius = 4,
                };

                _layout.Children.Add(_overlay, () => _layout.Bounds);

                _layout.Children.Add(_shadowLayer, () => this.LayerPosition);

                _layout.Children.Add(_contentView, () => this.LayerPosition);

                if (_platformHelper.ControlAnimatesItself)
                {
                    _shadowLayer.TranslationY = _platformHelper.GetScreenSize().Height - (CardPadding.Top);
                    _contentView.TranslationY = _platformHelper.GetScreenSize().Height - (CardPadding.Top);
                }

                // Add tap
                _overlay.GestureRecognizers.Add(new TapGestureRecognizer
                {
                    Command = new Command(async () => await CloseAsync())
                });
            }
            else
            {
                // Card 
                _contentView = new ContentView {
                    BackgroundColor = Color.White,                    
                };

                _layout.Children.Add(_contentView, () => 
                    _layout.Bounds);
            }
        }

        private Rectangle LayerPosition 
        { 
            get 
            { 
                if (!_platformHelper.ControlAnimatesItself)
                    return new Rectangle(0, 0, _layout.Width, _layout.Height);

                if (Position == CardPosition.Custom)
                {
                    return new Rectangle(CardPadding.Left, CardPadding.Top,
                        _layout.Width - CardPadding.Left - CardPadding.Right,
                        _layout.Height - CardPadding.Bottom - CardPadding.Top);
                }
                var screen = _platformHelper.GetScreenSize();
                var width = WidthRequest > 0 
                    ? RequestedWidth 
                    : (_contentView.Content.Width < 0 ? _layout.Width : _contentView.Content.Width);

                var height = HeightRequest > 0 
                    ? RequestedHeight
                    : (_contentView.Content.Height < 0 ? _layout.Height : _contentView.Content.Height);

                var dx = (screen.Width - width) / 2;
                var dy = (screen.Height - height) / 2;

                if (Position == CardPosition.Bottom)
                    return new Rectangle(dx,2 * dy, width, height);
                if (Position == CardPosition.Center)
                    return new Rectangle(dx, dy, width, height);

                return new Rectangle(dx, 0, width, height);
            } 
        }

		/// <summary>
		/// Raises the appearing event.
		/// </summary>
		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			if (_platformHelper.ControlAnimatesItself) 
            {
				_overlay.FadeTo (0.5F);
				_shadowLayer.TranslateTo (0.0, 0.0, 150, Easing.CubicInOut);
				_contentView.TranslateTo (0.0, 0.0, 150, Easing.CubicInOut);
			} 
		}

		/// <param name="x">Left-hand side of layout area.</param>
		/// <param name="y">Top of layout area.</param>
		/// <param name="width">Width of layout area.</param>
		/// <param name="height">Height of layout area.</param>
		/// <summary>
		/// Layouts the children.
		/// </summary>
		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			base.LayoutChildren (x, y, width, height);
			((Layout)_contentView.Content).ForceLayout ();
			var l = _contentView.Content.Bounds;
		}

		#region Properties

		/// <summary>
		/// Gets or sets the height of the requested.
		/// </summary>
		/// <value>The height of the requested.</value>
		public virtual double RequestedHeight 
		{			
			get { return _requestedHeight; }
			set 
			{
				var height = value;
				var padding = (_platformHelper.GetScreenSize().Height - height)/2;
                CardPadding = new Thickness (CardPadding.Left, padding, CardPadding.Right, padding);

				_requestedHeight = value;
				InvalidateMeasure ();
			}
		}

        /// <summary>
        /// Gets or sets the width of the requested.
        /// </summary>
        /// <value>The width of the requested.</value>
        public virtual double RequestedWidth 
        {           
            get { return _requestedWidth; }
            set 
            {
                var padding = (_platformHelper.GetScreenSize().Width - value)/2;
                CardPadding = new Thickness (padding, CardPadding.Top, padding, CardPadding.Bottom);

                _requestedWidth = value;
                InvalidateMeasure ();
            }
        }   

        private bool _hasHaddow;
        public bool HasShaddow
        {
            get { return _hasHaddow; }
            set 
            {
                if (_hasHaddow == value) return;
                _hasHaddow = value;

				_shadowLayer.IsVisible = value;
            }
        }

        private CardPosition _position = CardPosition.Custom;
        public CardPosition Position
        {
            get { return _position; }
            set 
            {
                if (_position == value) return;
                _position = value;
            }
        }

        private Thickness _cardPadding;
        public Thickness CardPadding 
        {
            get { return _cardPadding; }
            set 
            { 
                _cardPadding = value;
                Position = CardPosition.Custom;
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

		#region Public Members

		/// <summary>
		/// Shows the card async
		/// </summary>
		/// <returns>The async.</returns>
		public virtual Task ShowAsync()
		{
			return _platformHelper.ShowAsync (this);
		}

		/// <summary>
		/// Closes the async.
		/// </summary>
		/// <returns>The async.</returns>
		public virtual async Task CloseAsync()
		{
			if (_platformHelper.ControlAnimatesItself) {

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
				_shadowLayer.TranslateTo(0.0,

#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
_platformHelper.GetScreenSize().Height - (CardPadding.Top), 250, Easing.CubicInOut);

                await _contentView.TranslateTo(0.0,
                    _platformHelper.GetScreenSize().Height - (CardPadding.Top), 250, Easing.CubicInOut);

                await _overlay.FadeTo(0.0F, 150, Easing.CubicInOut);

            }

            await _platformHelper.CloseAsync (this);
		}

		#endregion
	}
}

