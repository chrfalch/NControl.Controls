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
        /// 
        /// </summary>
        /// <param name="card"></param>
        /// <returns></returns>
        public Task ShowAsync(CardPage card)
        {
            return Task.FromResult(true);
        }

        public Task HideAsync(CardPage card)
        {
            return Task.FromResult(true);
        }

        public Task CloseAsync(CardPage card)
        {
            return Task.FromResult(true);
        }

        public bool ShouldRenderChrome()
        {
            return false;
        }

        public Xamarin.Forms.Size GetScreenSize()
        {
            return new Xamarin.Forms.Size(Application.Current.Host.Content.ActualWidth,
                Application.Current.Host.Content.ActualHeight);
        }
    }
}
