using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using NControl.Controls.WP81;
using NGraphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;
using Application = System.Windows.Application;
using Colors = System.Windows.Media.Colors;
using Grid = System.Windows.Controls.Grid;
using Rectangle = System.Windows.Shapes.Rectangle;
using Coding4Fun.Toolkit.Controls;
using NControl.Win;

[assembly: Dependency(typeof(CardPageHelper))]
namespace NControl.Controls.WP81
{
    /// <summary>
    /// CardPage helper implementation
    /// </summary>
    public class CardPageHelper: ICardPageHelper, IPopupInformationProvider
    {
        /// <summary>
        /// List of presented wrappers
        /// </summary>
        private static Stack<CardPageWrapperPopup> _presentationContext = new Stack<CardPageWrapperPopup>();

        /// <summary>
        /// Returns the screen size
        /// </summary>
        /// <returns></returns>
        public Xamarin.Forms.Size GetScreenSize()
        {
            return new Xamarin.Forms.Size(Application.Current.Host.Content.ActualWidth,
                Application.Current.Host.Content.ActualHeight);
        }

        public Task ShowAsync(CardPage card)
        {
            var cardPageWrapper = new CardPageWrapperPopup(card){
                Title = card.Title,                
            };

            _presentationContext.Push(cardPageWrapper);            
            cardPageWrapper.Show();
            
            return Task.FromResult(true);
        }

        /// <summary>
        /// Hides the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="card">Card.</param>
        public Task CloseAsync(CardPage card)
        {
            var currentCardPageWrapper = _presentationContext.Pop();
            currentCardPageWrapper.Hide();
            return Task.FromResult(true);
        }

        public FrameworkElement GetPopupParent()
        {
            if (!_presentationContext.Any())
                return null;

            return _presentationContext.Peek().GetVisualParent();
        }

        public bool ControlAnimatesItself {
            get { return false;  }
        }
    }

    /// <summary>
    /// Class for wrapping xamarin forms page in a popup on Windows Phone
    /// </summary>
    public class CardPageWrapperPopup : MessagePrompt
    {
        private CardPage _card;

        public CardPageWrapperPopup(CardPage card)
        {
            _card = card;
            
            // Remove the standard circular button            
            ActionPopUpButtons.RemoveAll(new Predicate<System.Windows.Controls.Button>(
                delegate (System.Windows.Controls.Button arg) { return true; }));

            // Do any further initialization. e.g. loading some elements into the popup
            // This is our great hack. Without having the Platform property set, sizing isnt working
            // See the VisualElement.OnSizeRequest method.
            var pi = _card.GetType().GetProperty("Platform", System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var val = pi.GetValue(Xamarin.Forms.Application.Current.MainPage);
            pi.SetValue(_card, val);

            // Set renderer
            if (Xamarin.Forms.Platform.WinPhone.ViewExtensions.GetRenderer(_card) == null)
                Xamarin.Forms.Platform.WinPhone.ViewExtensions.SetRenderer(_card,
                    RendererFactory.GetRenderer(_card));

            // Layout
            _card.Layout(new Xamarin.Forms.Rectangle(0.0, 0.0, Application.Current.Host.Content.ActualWidth - 45,
                _card.RequestedHeight));

            var el = (UIElement)Xamarin.Forms.Platform.WinPhone.ViewExtensions.GetRenderer(_card);

            this.Body = new Border
            {
                Width = Application.Current.Host.Content.ActualWidth - 45,
                Height = _card.RequestedHeight,
                Background = new SolidColorBrush(Colors.Blue),
                Child = el
            };
        }
    }
    
}
