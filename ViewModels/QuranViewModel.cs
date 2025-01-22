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
        IsLoading = false;
        _preferences = preferences;
        _quranSurahPersistence = quranSurahPersistence;
        _quranAyahPersistence = quranAyahPersistence;
        _sholatTimesPersistence = sholatTimesPersistence;
        _cityCodesPersistence = cityCodesPersistence;
        _quranApi = quranApi;
    }

    [ObservableProperty]
    bool isLoading;

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
    }

    [RelayCommand]
    async Task InitCitySelectionPage()
    {
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
    public async Task SelectCity(string city)
    {
        if (!string.IsNullOrEmpty(city))
        {
            IsLoading = true;
            _preferences.Set("MyCity", city);

            string[] citySplited = city.Split(" - ");
            string cityCode = citySplited[0];

            try
            {
                await _quranApi.SyncSholatTimeByMonthAsync(cityCode);
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("hostname could not be provided"))
            {
                // Handle the specific case where the hostname is not provided
                IsLoading = false;
                await Shell.Current.DisplayAlert("Error", "Failed to Connect to the Server!.", "OK");
            }
            finally
            {
                CityCodeList.Clear();
                IsLoading = false;
                await Shell.Current.GoToAsync("..");
            }
        }
    }
}
