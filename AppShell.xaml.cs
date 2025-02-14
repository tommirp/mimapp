using MimApp.Views;
using MimApp.Views.Quran;

namespace MimApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(CitySelectionPage), typeof(CitySelectionPage));
        Routing.RegisterRoute(nameof(SurahDetailPage), typeof(SurahDetailPage));
        Routing.RegisterRoute(nameof(QuranAppSetting), typeof(QuranAppSetting));

        //CrossFirebaseCloudMessaging.Current.NotificationReceived += (sender, e) =>
        //{
        //    Debug.WriteLine($"-------------------------> Notification received: {e.Notification.Title}");

        //};

        //CrossFirebaseCloudMessaging.Current.SubscribeToTopicAsync("WellStatus");

        //CrossFirebaseCloudMessaging.Current.NotificationTapped += async (sender, e) =>
        //{
        //    await Shell.Current.GoToAsync($"//{nameof(AboutPage)}");
        //};
    }
}
