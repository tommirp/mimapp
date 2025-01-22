using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;
using MimApp.Views.Quran;
using System.Timers;
using Timer = System.Timers.Timer;

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

        MyCity = "Select City";

        // Timer Area
        PlaceholderSearch = "";

        Timer myTimer = new Timer(2000);
        myTimer.Elapsed += Tick;
        myTimer.Enabled = true;
        // Timer Area
    }

    [ObservableProperty]
    bool isLoading;

    [ObservableProperty]
    string searchText;

    [ObservableProperty]
    string myCity;

    [ObservableProperty]
    QuranSholatTime todaySholatTime;

    #region Timer Search Placeholder
    public string PlaceholderSearch;

    int _counter = 0;
    List<string> PlaceholderList = new List<string>()
    {
        "Surat:Ayat (2:60)",
        "Nama Surat (Al-Fatihah)",
        "Nomor Surat (1)",
        "Keyword Ayat (Sedekah)"
    };

    private void Tick(object sender, ElapsedEventArgs e)
    {
        PlaceholderSearch = PlaceholderList[_counter];

        if (_counter >= 4) _counter = 0;
        _counter++;
    }
    #endregion

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
            if (!string.IsNullOrEmpty(city))
            {
                string[] citySplited = city.Split(" - ");
                MyCity = citySplited[1];
            }

            IsLoading = false;
        }
    }

    [RelayCommand]
    async Task SearchQuran()
    {
        _preferences.Set("QuranSearch", SearchText);
        // await Shell.Current.GoToAsync(nameof(QuranSearchPage));
    }
}
