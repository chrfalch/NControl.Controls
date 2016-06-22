using Microsoft.Phone.Controls;
using NControl.Controls.WP80;
using NControl.WP80;
using Xamarin.Forms;

namespace NControl.Controls.Demo.FormsApp.WP80
{
    public partial class MainPage 
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            Forms.Init();
            NControlViewRenderer.Init();
            NControls.Init();
            
            LoadApplication(new FormsApp.MyApp());
        }
    }
}