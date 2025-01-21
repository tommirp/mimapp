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
        IsRefreshing = false;
        _preferences = preferences;
        _quranSurahPersistence = quranSurahPersistence;
        _quranAyahPersistence = quranAyahPersistence;
        _sholatTimesPersistence = sholatTimesPersistence;
        _cityCodesPersistence = cityCodesPersistence;
        _quranApi = quranApi;
    }

    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    string searchTextCity;

    [ObservableProperty]
    string myCity;
    public ObservableCollection<string> CityCodeList { get; } = new ObservableCollection<string>();

    async Task GetAllCities()
    {
        CityCodeList.Clear();

        var all_cities = await _cityCodesPersistence.GetAllCityCodes(50);
        all_cities.ForEach(x => CityCodeList.Add(String.Format("{0} - {1}", x.id, x.lokasi)));
    }

    [RelayCommand]
    async Task InitPage()
    {
        if (_hasLoaded)
            return;

        _hasLoaded = true;

        await GetAllCities();
    }

    [RelayCommand]
    async Task SearchCity()
    {
        if (string.IsNullOrEmpty(SearchTextCity))
        {
            CityCodeList.Clear();
            await GetAllCities();
        }
        else
        {
            var all_cities = await _cityCodesPersistence.GetCityByName(SearchTextCity);
            CityCodeList.Clear();
            all_cities.ForEach(x => CityCodeList.Add(String.Format("{0} - {1}", x.id, x.lokasi)));
        }
    }

    [RelayCommand]
    public void OnTapCity(string city)
    {
        Debug.WriteLine(city);

        _preferences.Set("MyCity", MyCity);
        _ = _quranApi.SyncSholatTimeByMonthAsync(MyCity);

        CityCodeList.Clear();
    }
}
