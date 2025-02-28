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
        Routing.RegisterRoute(nameof(QuranAppSettingPage), typeof(QuranAppSettingPage));
        Routing.RegisterRoute(nameof(AyahDetailPage), typeof(AyahDetailPage));
        Routing.RegisterRoute(nameof(QuranSearchPage), typeof(QuranSearchPage));
        Routing.RegisterRoute(nameof(SholatTimesPage), typeof(SholatTimesPage));
        Routing.RegisterRoute(nameof(AsmaulHusnaPage), typeof(AsmaulHusnaPage));

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
