using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;
using MimApp.Views.Quran;

namespace MimApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IPreferences _preferences;
    private readonly IConnectivity _connectivity;
    private readonly IQuranSurahPersistence _quranSurahPersistence;
    private readonly IQuranAyahPersistence _quranAyahPersistence;
    private readonly ISholatTimesPersistence _sholatTimesPersistence;
    private readonly ICityCodesPersistence _cityCodesPersistence;
    private readonly IQuranApi _quranApi;

    public MainViewModel(IPreferences preferences, IQuranSurahPersistence quranSurahPersistence,
        IQuranAyahPersistence quranAyahPersistence, ISholatTimesPersistence sholatTimesPersistence,
        ICityCodesPersistence cityCodesPersistence, IQuranApi quranApi, IConnectivity connectivity)
    {
        SearchText = String.Empty;
        IsLoading = false;
        _preferences = preferences;
        _connectivity = connectivity;
        _quranSurahPersistence = quranSurahPersistence;
        _quranAyahPersistence = quranAyahPersistence;
        _sholatTimesPersistence = sholatTimesPersistence;
        _cityCodesPersistence = cityCodesPersistence;
        _quranApi = quranApi;

        SelectedItemAutoComplete = null;

        MyCity = "Select City";

        PlaceholderSearch = PlaceholderList[0];
    }

    [ObservableProperty]
    bool isLoading;

    [ObservableProperty]
    string searchText;

    [ObservableProperty]
    string myCity;

    [ObservableProperty]
    QuranSholatTime todaySholatTime;

    [ObservableProperty]
    string selectedItemAutoComplete;

    [RelayCommand]
    async Task GoToCitySelection()
    {
        await Shell.Current.GoToAsync(nameof(CitySelectionPage));
    }
    async Task<List<QuranSurah>?> GetSurah()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("quran_surah.json");
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();
        return JsonSerializer.Deserialize<List<QuranSurah>>(json);
    }

    async Task<List<QuranAyah>?> GetAyah()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("quran_ayat.json");
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();
        return JsonSerializer.Deserialize<List<QuranAyah>>(json);
    }

    #region Timer Search Placeholder
    [ObservableProperty]
    string placeholderSearch;

    int _counter = 0;
    List<string> PlaceholderList = new List<string>()
    {
        "Surat:Ayat (2:60)",
        "Nama Surat (Al-Fatihah)",
        "Nomor Surat (1)",
        "Keyword Ayat (Sedekah)"
    };

    public void InitTimer()
    {
        var timer = new System.Threading.Timer(_ =>
        {
            if (_counter == PlaceholderList.Count)
            {
                _counter = 0;
            }

            PlaceholderSearch = PlaceholderList[_counter];

            _counter++;
        }, null, 2500, 2500);
    }
    #endregion

    [RelayCommand]
    public async Task InitPage()
    {
        try
        {
            IsLoading = true;

            bool SurahChecked = await _quranSurahPersistence.SurahCheck();

            if (!SurahChecked)
            {
                List<QuranSurah>? surah = await GetSurah();

                if (surah is not null)
                {
                    await _quranSurahPersistence.DeleteAllItemsAsync();
                    await _quranSurahPersistence.InsertAllItemAsync(surah);
                }

                List<QuranAyah>? ayah = await GetAyah();
                if (ayah is not null)
                {
                    await _quranAyahPersistence.DeleteAllItemsAsync();
                    await _quranAyahPersistence.InsertAllItemAsync(ayah);
                }
            }
        }
        finally
        {
            bool SurahChecked = await _quranSurahPersistence.SurahCheck();

            if (!SurahChecked)
            {
                await _quranApi.OnlineSyncQuran();
            }

            if (_connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                await _quranApi.SyncCityCodesAsync();

                TodaySholatTime = await _sholatTimesPersistence.GetSholatTimeByDate(DateTime.Now.ToString("yyyy-MM-dd"));
            }

            string city = _preferences.Get("MyCity", "Select City");
            if (!string.IsNullOrEmpty(city) && city != "Select City")
            {
                string[] citySplited = city.Split(" - ");
                MyCity = citySplited[1];
            }

            IsLoading = false;

            InitTimer();
        }
    }

    async Task MainSearch(string search_text)
    {
        string search_text_valid = search_text;
        if (SearchText.Contains(" - "))
        {
            string[] searchSplited = search_text.Split(" - ");
            search_text_valid = searchSplited[0];

            _preferences.Set("QuranSearch", search_text_valid);
            SearchText = "";
            await Shell.Current.GoToAsync(nameof(SurahDetailPage));
        }
        else if (search_text.Contains(":"))
        {
            string[] searchSplited = search_text.Replace(" ", "").Split(":");
            string surah = searchSplited[0];
            string ayat = searchSplited[1];

            // Go To Surah Detail Page with surah number and scroll to ayat
        }
        else
        {
            _preferences.Set("QuranSearch", search_text);
            // await Shell.Current.GoToAsync(nameof(QuranSearchKeywordPage));
        }
    }

    [RelayCommand]
    async Task SearchQuran()
    {
        await MainSearch(SearchText);
    }

    [RelayCommand]
    async Task SearchBySelect()
    {
        await MainSearch(SelectedItemAutoComplete);
    }
}
