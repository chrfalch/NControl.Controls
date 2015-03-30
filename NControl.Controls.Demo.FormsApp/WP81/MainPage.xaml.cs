using Microsoft.Phone.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;

namespace NControl.Controls.Demo.FormsApp.WP81
{
    public partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            SupportedOrientations = SupportedPageOrientation.PortraitOrLandscape;

            Forms.Init();                        
            Controls.WP81.NControls.Init();

            LoadApplication(new FormsApp.MyApp());
        }      
    }
}