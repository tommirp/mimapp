using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;
using MimApp.Utils;
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
    private readonly IGeneralMetaPersistence _generalMetaPersistence;
    private readonly IQuranApi _quranApi;

    public MainViewModel(IPreferences preferences, IQuranSurahPersistence quranSurahPersistence,
        IQuranAyahPersistence quranAyahPersistence, ISholatTimesPersistence sholatTimesPersistence,
        ICityCodesPersistence cityCodesPersistence, IQuranApi quranApi, IConnectivity connectivity, IGeneralMetaPersistence generalMetaPersistence)
    {
        SearchText = String.Empty;
        IsLoading = false;
        _preferences = preferences;
        _connectivity = connectivity;
        _quranSurahPersistence = quranSurahPersistence;
        _quranAyahPersistence = quranAyahPersistence;
        _sholatTimesPersistence = sholatTimesPersistence;
        _cityCodesPersistence = cityCodesPersistence;
        _generalMetaPersistence = generalMetaPersistence;
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

    [RelayCommand]
    async Task GoToWaktuSholat()
    {
        await Shell.Current.GoToAsync(nameof(SholatTimesPage));
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

    async Task<List<GeneralMetaData>?> GetYoutube()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("youtube.json");
        using var reader = new StreamReader(stream);
        var json = await reader.ReadToEndAsync();
        return JsonSerializer.Deserialize<List<GeneralMetaData>>(json);
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
        }, null, 7000, 7000);
    }
    #endregion

    [RelayCommand]
    public async Task InitPage()
    {
        try
        {
            IsLoading = true;

            bool SurahChecked = await _quranSurahPersistence.SurahCheck();
            bool AyahChecked = await _quranAyahPersistence.AyahCheck();

            if (!SurahChecked || !AyahChecked)
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
            bool AyahChecked = await _quranAyahPersistence.AyahCheck();

            if (!SurahChecked || !AyahChecked)
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

            _preferences.Set("QuranSearch", string.Empty);
        }
    }

    async Task MainSearch(string search_text, bool in_search_page = false)
    {
        if (!string.IsNullOrEmpty(search_text))
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

                _preferences.Set("QuranSearch", string.Format("{0}:{1}", surah, ayat));

                SearchText = "";
                await Shell.Current.GoToAsync(nameof(SurahDetailPage));
            }
            else
            {
                if (in_search_page)
                {
                    await SearchQuranByKeyword(search_text);
                }
                else
                {
                    _preferences.Set("QuranSearch", search_text);
                    await Shell.Current.GoToAsync(nameof(QuranSearchPage));
                }
            }
        }
    }

    [RelayCommand]
    async Task SearchQuran()
    {
        KeyboardHelper.CloseKeyboard(); // Closes the keyboard globally
        await MainSearch(SearchText);
    }

    [RelayCommand]
    async Task SearchBySelect()
    {

        KeyboardHelper.CloseKeyboard(); // Closes the keyboard globally
        await MainSearch(SelectedItemAutoComplete);
    }

    #region Quran Search
    public ObservableCollection<string> QuranSearchList { get; } = new ObservableCollection<string>();

    [RelayCommand]
    async Task SearchQuran_SearchPage()
    {
        IsLoading = true;
        KeyboardHelper.CloseKeyboard(); // Closes the keyboard globally
        await MainSearch(SearchText, true);
    }

    [RelayCommand]
    async Task SearchBySelect_SearchPage()
    {
        IsLoading = true;
        KeyboardHelper.CloseKeyboard(); // Closes the keyboard globally
        await MainSearch(SelectedItemAutoComplete, true);
    }

    [RelayCommand]
    async Task InitQuranSearchPage()
    {
        try
        {
            IsLoading = true;
            string keyword = _preferences.Get("QuranSearch", string.Empty);
            SearchText = keyword;
            await MainSearch(keyword, true);
        }
        finally
        {
            IsLoading = false;
        }
    }
    async Task SearchQuranByKeyword(string search_text)
    {
        QuranSearchList.Clear();

        if (!string.IsNullOrEmpty(search_text))
        {
            var search_result = await _quranAyahPersistence.GetAyahByKeyword(search_text);
            search_result.ForEach(x => QuranSearchList.Add(x));
        }

        IsLoading = false;
    }

    [RelayCommand]
    public async Task SelectSearchContent(string input)
    {
        try
        {
            if (!string.IsNullOrEmpty(input))
            {
                string[] inputSplited = input.Split(".");
                string QS = inputSplited[0];
                await MainSearch(QS);

            }
        }
        finally
        {
            if (!string.IsNullOrEmpty(input))
            {
                SearchText = string.Empty;
            }
        }
    }
    #endregion

    [RelayCommand]
    public async Task GoToTandaBaca()
    {
        string tandaBaca = _preferences.Get("AppSetting_TandaBaca", string.Empty);
        if (!string.IsNullOrEmpty(tandaBaca))
        {
            _preferences.Set("QuranSearch", tandaBaca);
            SearchText = "";
            await Shell.Current.GoToAsync(nameof(SurahDetailPage));
        }
        else
        {
            await Shell.Current.DisplayAlert("Error", "Belum Ada Tanda Baca, Yuk Baca Al-Quran Sekarang!", "OK");
        }
    }

    [RelayCommand]
    public async Task OpenLinkMekah()
    {
        await Launcher.OpenAsync("https://www.youtube.com/results?search_query=Mekah+Live");
    }

    [RelayCommand]
    public async Task OpenLinkMadinah()
    {
        await Launcher.OpenAsync("https://www.youtube.com/results?search_query=Madinah+Live");
    }

    [RelayCommand]
    public async Task OpenLinkMasjidNearby()
    {
        await Launcher.OpenAsync("https://www.google.com/maps/search/Masjid+Nearby");
    }
}
