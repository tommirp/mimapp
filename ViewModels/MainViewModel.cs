using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;

namespace MimApp.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly IPreferences _preferences;
    private readonly IQuranSurahPersistence _quranSurahPersistence;
    private readonly IQuranAyahPersistence _quranAyahPersistence;
    private readonly ISholatTimesPersistence _sholatTimesPersistence;
    private readonly IQuranApi _quranApi;

    public MainViewModel(IPreferences preferences, IQuranSurahPersistence quranSurahPersistence,
        IQuranAyahPersistence quranAyahPersistence, ISholatTimesPersistence sholatTimesPersistence,
        IQuranApi quranApi)
    {
        SearchText = String.Empty;
        _preferences = preferences;
        _quranSurahPersistence = quranSurahPersistence;
        _quranAyahPersistence = quranAyahPersistence;
        _sholatTimesPersistence = sholatTimesPersistence;
        _quranApi = quranApi;
    }

    [ObservableProperty]
    string searchText;

    [ObservableProperty]
    string myCity;

    [ObservableProperty]
    QuranSholatTime todaySholatTime;

    [RelayCommand]
    async Task SelectCity()
    {
        try
        {
            _preferences.Set("MyCity", MyCity);
            await _quranApi.SyncSholatTimeByMonthAsync(MyCity);
        }
        finally
        {
            TodaySholatTime = await _sholatTimesPersistence.GetSholatTimeByDate(DateTime.Now.ToString("yyyy-MM-dd"));
        }
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
    async Task SetupQuran()
    {
        try
        {
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
        }
    }

    [RelayCommand]
    async Task SetupSholatTime()
    {
        
        await _quranApi.SyncSholatTimeByMonthAsync();
    }

    [RelayCommand]
    async Task SearchQuran()
    {
        string result = SearchText;
        // Do Search
    }
}
