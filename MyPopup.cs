using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;

namespace MimApp;
public class MyPopup : Popup
{
    public MyPopup()
    {
        Content = new Border
        {
            HorizontalOptions = LayoutOptions.Center, // Auto-width
            VerticalOptions = LayoutOptions.Center, // Centered popup
            Padding = 20, // Inner spacing

            Content = new VerticalStackLayout
            {
                BackgroundColor = Colors.Transparent,
                Children =
                {
                    new Label { Text = "Tafsir", FontSize = 18, TextColor = Colors.Black },
                    new Label { Text = "Tafsir", FontSize = 18, TextColor = Colors.Black },
                    new Button { Text = "Close", Command = new Command(Close), BackgroundColor = Colors.LightGray, TextColor = Colors.Black }
                }
            }
        };
    }
}
