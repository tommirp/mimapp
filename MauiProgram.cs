using MimApp.Persistences;
using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;
using MimApp.Views.Auth;
using DevExpress.Maui;
using MimApp.Views.Quran;

namespace MimApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseDevExpress(useLocalization: true)
            .UseDevExpressCollectionView()
            .UseDevExpressControls()
            .UseDevExpressEditors()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("AmiriQuran-Regular.ttf", "AmiriQuran");
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("roboto-regular.ttf", "Roboto");
                fonts.AddFont("roboto-medium.ttf", "Roboto-Medium");
                fonts.AddFont("roboto-bold.ttf", "Roboto-Bold");
                fonts.AddFont("univia-pro-regular.ttf", "Univia-Pro");
                fonts.AddFont("univia-pro-medium.ttf", "Univia-Pro Medium");
            }).RegisterServices().RegisterViewModels().RegisterViews().UsePageResolver().UseMauiCommunityToolkit();

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
        mauiAppBuilder.Services.AddScoped<IQuranSurahPersistence, QuranSurahPersistence>();
        mauiAppBuilder.Services.AddScoped<IQuranAyahPersistence, QuranAyahPersistence>();
        mauiAppBuilder.Services.AddScoped<ICityCodesPersistence, CityCodesPersistence>();
        mauiAppBuilder.Services.AddScoped<ISholatTimesPersistence, SholatTimesPersistence>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<MainViewModel>();
        mauiAppBuilder.Services.AddScoped<QuranViewModel>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddScoped<MainPage>();

        // Quran
        mauiAppBuilder.Services.AddScoped<CitySelectionPage>();
        mauiAppBuilder.Services.AddScoped<SurahDetailPage>();

        // Auth
        mauiAppBuilder.Services.AddSingleton<Login>();
        return mauiAppBuilder;
    }
}
