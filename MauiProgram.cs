using MimApp.Persistences;
using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;

namespace MimApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			}).RegisterServices().RegisterViewModels().RegisterViews().UsePageResolver();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}

	public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
        mauiAppBuilder.Services.AddSingleton<IPreferences>(Preferences.Default);

        // Services
        mauiAppBuilder.Services.AddHttpClient<IQuranApi, QuranApiService>();

        // Persistences
        mauiAppBuilder.Services.AddHttpClient<IQuranApiPersistence, QuranApiPersistence>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainViewModel>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<MainPage>();
        return mauiAppBuilder;
    }
}
