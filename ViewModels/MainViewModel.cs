using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;
using MimApp.Views.Quran;

namespace MimApp.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly IPreferences _preferences;
    private readonly IQuranSurahPersistence _quranSurahPersistence;
    private readonly IQuranAyahPersistence _quranAyahPersistence;
    private readonly ISholatTimesPersistence _sholatTimesPersistence;
    private readonly ICityCodesPersistence _cityCodesPersistence;
    private readonly IQuranApi _quranApi;

    public MainViewModel(IPreferences preferences, IQuranSurahPersistence quranSurahPersistence,
        IQuranAyahPersistence quranAyahPersistence, ISholatTimesPersistence sholatTimesPersistence,
        ICityCodesPersistence cityCodesPersistence, IQuranApi quranApi)
    {
        SearchText = String.Empty;
        IsLoading = false;
        _preferences = preferences;
        _quranSurahPersistence = quranSurahPersistence;
        _quranAyahPersistence = quranAyahPersistence;
        _sholatTimesPersistence = sholatTimesPersistence;
        _cityCodesPersistence = cityCodesPersistence;
        _quranApi = quranApi;

        _ = InitPage();
    }

    [ObservableProperty]
    bool isLoading;

    [ObservableProperty]
    string searchText;

    [ObservableProperty]
    string myCity;

    [ObservableProperty]
    QuranSholatTime todaySholatTime;


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
    async Task InitPage()
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

            await _quranApi.SyncCityCodesAsync();

            TodaySholatTime = await _sholatTimesPersistence.GetSholatTimeByDate(DateTime.Now.ToString("yyyy-MM-dd"));

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
