using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;

namespace MimApp.ViewModels;

public partial class QuranViewModel : ViewModelBase
{
    private readonly IPreferences _preferences;
    private readonly IQuranSurahPersistence _quranSurahPersistence;
    private readonly IQuranAyahPersistence _quranAyahPersistence;
    private readonly ISholatTimesPersistence _sholatTimesPersistence;
    private readonly ICityCodesPersistence _cityCodesPersistence;
    private readonly IQuranApi _quranApi;

    public QuranViewModel(IPreferences preferences, IQuranSurahPersistence quranSurahPersistence,
        IQuranAyahPersistence quranAyahPersistence, ISholatTimesPersistence sholatTimesPersistence,
        ICityCodesPersistence cityCodesPersistence, IQuranApi quranApi)
    {
        CityCodesList = new List<CityCodes>();
        IsRefreshing = false;
        _preferences = preferences;
        _quranSurahPersistence = quranSurahPersistence;
        _quranAyahPersistence = quranAyahPersistence;
        _sholatTimesPersistence = sholatTimesPersistence;
        _cityCodesPersistence = cityCodesPersistence;
        _quranApi = quranApi;

        _ = InitPage();
    }

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    string myCity;

    [ObservableProperty]
    List<CityCodes> cityCodesList;

    [RelayCommand]
    async Task InitPage()
    {
        CityCodesList = await _cityCodesPersistence.GetAllCityCodes();
    }

    [RelayCommand]
    async Task SelectCity()
    {
        _preferences.Set("MyCity", MyCity);
        await _quranApi.SyncSholatTimeByMonthAsync(MyCity);
    }
}
