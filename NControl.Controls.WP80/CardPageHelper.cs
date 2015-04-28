using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NControl.Controls.WP80;

[assembly: Xamarin.Forms.Dependency(typeof(CardPageHelper))]
namespace NControl.Controls.WP80
{
    /// <summary>
    /// CardPage helper implementation
    /// </summary>
    public class CardPageHelper : ICardPageHelper
    {
        /// <summary>
        /// Returns screen size
        /// </summary>
        /// <returns></returns>
        public Xamarin.Forms.Size GetScreenSize()
        {
            return new Xamarin.Forms.Size(Application.Current.Host.Content.ActualWidth,
                Application.Current.Host.Content.ActualHeight);
        }

        public Task ShowAsync(CardPage page)
        {
            throw new NotImplementedException();
        }

        public Task CloseAsync(CardPage page)
        {
            throw new NotImplementedException();
        }

        public bool ControlAnimatesItself { get { return true; } }
    }
}
