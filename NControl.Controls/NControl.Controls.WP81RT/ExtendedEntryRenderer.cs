using Xamarin.Forms.Platform.WinRT;
using NControl.Controls;
using Xamarin.Forms;
using NControl.Controls.WP81RT;

[assembly: ExportRenderer(typeof(ExtendedEntry), typeof(ExtendedEntryRenderer))]

namespace NControl.Controls.WP81RT
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        /*protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null)
                return;

            var textBox = (FormsPhoneTextBox)Control;
            
            var foregroundColor = Colors.Black;
            var hintColor = Colors.DarkGray;

            var darkBackgroundVisibility = (Visibility)Application.Current.Resources["PhoneDarkThemeVisibility"];
            if (darkBackgroundVisibility == Visibility.Visible)
            {
                foregroundColor = Colors.White;
                hintColor = Colors.DarkGray;
            }

            // remove borders and backgrounds in regular mode
            textBox.BorderThickness = new Thickness(0);
            textBox.BorderBrush = new SolidColorBrush(Colors.Transparent);
            textBox.Background = new SolidColorBrush(Colors.Transparent);
            
            // Foregrounds
            textBox.Foreground = new SolidColorBrush(foregroundColor);
            textBox.SelectionForeground = new SolidColorBrush(foregroundColor);
            
            // Focus handling
            textBox.GotFocus += (sender, evt) =>
            {
                textBox.Background = new SolidColorBrush(Colors.Transparent);
                textBox.CaretBrush = new SolidColorBrush(foregroundColor);
            
                textBox.ActualHintVisibility = string.IsNullOrWhiteSpace(textBox.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            };

            textBox.TextChanged += (sender, evt) =>
            {
                textBox.ActualHintVisibility = string.IsNullOrWhiteSpace(textBox.Text)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            };

            // Update hint
            var style = new Style { TargetType = typeof(ContentControl) };
            style.Setters.Add(new Setter(System.Windows.Controls.Control.ForegroundProperty, new SolidColorBrush(hintColor)));
            textBox.HintStyle = style;
        }
        */
    }
}

