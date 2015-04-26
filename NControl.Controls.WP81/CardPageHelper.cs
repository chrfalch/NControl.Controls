using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NControl.Controls.WP81;
using Xamarin.Forms;
using Application = System.Windows.Application;

[assembly: Dependency(typeof(CardPageHelper))]
namespace NControl.Controls.WP81
{
    /// <summary>
    /// CardPage helper implementation
    /// </summary>
    public class CardPageHelper: ICardPageHelper
    {      
        /// <summary>
        /// Returns the screen size
        /// </summary>
        /// <returns></returns>
        public Xamarin.Forms.Size GetScreenSize()
        {
            return new Xamarin.Forms.Size(Application.Current.Host.Content.ActualWidth,
                Application.Current.Host.Content.ActualHeight);
        }
    }
}
