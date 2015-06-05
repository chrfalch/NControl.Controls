using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using NControl.Controls.WP80;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

[assembly: Xamarin.Forms.Dependency(typeof(CardPageHelper))]
namespace NControl.Controls.WP80
{
    /// <summary>
    /// CardPage helper implementation
    /// </summary>
    /// <summary>
    /// CardPage helper implementation
    /// </summary>
    public class CardPageHelper : ICardPageHelper
    {
        /// <summary>
        /// The presented controllers.
        /// </summary>
        private readonly Dictionary<CardPage, CardPageContext> _presentedCardPageContexts =
            new Dictionary<CardPage, CardPageContext>();

        /// <summary>
        /// Returns the screen size
        /// </summary>
        /// <returns></returns>
        public Xamarin.Forms.Size GetScreenSize()
        {
            return new Xamarin.Forms.Size(System.Windows.Application.Current.Host.Content.ActualWidth,
                System.Windows.Application.Current.Host.Content.ActualHeight);
        }

        public Task ShowAsync(CardPage page)
        {
            // Get ahold of the main window
            var frame = System.Windows.Application.Current.RootVisual as PhoneApplicationFrame;
            var startPage = frame.Content as PhoneApplicationPage;
            var canvas = startPage.Content as Canvas;
            var pageRenderer = GetRenderer(canvas);


            // Create popup
            var popup = new Popup();
            pageRenderer.Children.Add(popup);

            // Add inner grid
            var popupGrid = new System.Windows.Controls.Grid
            {
                Opacity = 0.5,
                Background = new SolidColorBrush(Colors.Gray),
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch,
            };

            // Set inner grid as parent to popup
            popup.Child = popupGrid;
            popupGrid.Width = pageRenderer.ActualWidth;
            popupGrid.Height = pageRenderer.ActualHeight;

            // Get XF representation of card page
            var cardElement = page.ConvertPageToUIElement(startPage);
            popupGrid.Children.Add(cardElement);

            // Add to context list
            _presentedCardPageContexts.Add(page, new CardPageContext { Popup = popup });

            // Show
            popup.IsOpen = true;

            return Task.FromResult(true);
        }

        /// <summary>
        /// Returns the page renderer in the given canvas
        /// </summary>
        /// <param name="canvas"></param>
        /// <returns></returns>
        private MasterDetailRenderer GetRenderer(Canvas canvas)
        {
            if (canvas == null)
                throw new InvalidOperationException("MainPage.Content should be a Canvas element.");

            var pageRenderer = canvas.Children[0] as MasterDetailRenderer;
            if (pageRenderer == null)
                throw new InvalidOperationException("MainPage.Content as Canvas should contain a PageRenderer element.");

            return pageRenderer;
        }

        public Task CloseAsync(CardPage page)
        {
            if (_presentedCardPageContexts.ContainsKey(page))
            {
                var context = _presentedCardPageContexts[page];
                _presentedCardPageContexts.Remove(page);
                context.Popup.IsOpen = false;
            }

            return Task.FromResult(true);
        }

        public bool ControlAnimatesItself
        {
            get { return true; }
        }
    }

    /// <summary>
    /// Card page context.
    /// </summary>
    public class CardPageContext
    {
        /// <summary>
        /// Gets or sets the popup
        /// </summary>
        /// <value>The popup.</value>
        public Popup Popup { get; set; }
    }
}
