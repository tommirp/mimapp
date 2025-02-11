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

    #region City Page
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
    async Task InitCitySelectionPage()
    {
        try
        {
            IsLoading = true;
            await GetAllCities();
        }
        finally
        {

            IsLoading = false;
        }
    }

    [RelayCommand]
    async Task SearchCity()
    {
        await Task.Delay(1000).ContinueWith(async (x) =>
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
        });
    }

    [RelayCommand]
    public async Task SelectCity(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            IsLoading = true;
            _preferences.Set("MyCity", input);

            string[] inputSplited = input.Split(" - ");
            string inputCode = inputSplited[0];

            try
            {
                await _quranApi.SyncSholatTimeByMonthAsync(inputCode);
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
                //SearchTextCity = string.Empty;
                //IsLoading = false;
                await Shell.Current.GoToAsync("..");
            }
        }
    }
    #endregion


    #region Surah Page Showing All Ayah

    [ObservableProperty]
    string surahTitle;

    [ObservableProperty]
    QuranAyah selectedQuranAyah;
    public ObservableCollection<QuranAyah> AyahList { get; set; } = new ObservableCollection<QuranAyah>();

    [RelayCommand]
    public async Task AyahSelected()
    {
        if (SelectedQuranAyah != null)
        {
            QuranSurah theSurah = await _quranSurahPersistence.GetOneSurah(SelectedQuranAyah.numberInSurah);
            string title = String.Format("{0}. {1} : {2}", SelectedQuranAyah.numberOfSurah, theSurah?.nameTransliterationId?.ToUpper(), SelectedQuranAyah.numberInSurah);

            string[] btns = ["Info Detail", "Salin Ayat", "Tandai Batas Baca", "Bagikan", "Lapor Kesalahan"];

            string action = await Shell.Current.DisplayActionSheet(title, "Close", null, btns);

            if (btns.Contains(action))
            {
                if (action == "Info Detail")
                {
                    // Go To Ayah Detail Page
                }
                else if (action == "Salin Ayat")
                {
                    // Logic Salin Ayat to Clipboard
                    await Shell.Current.DisplayAlert("Success", "Ayat Berhasil Di Salin ke Clipboard", "OK");
                }
                else if (action == "Tandai Batas Baca")
                {
                    // Logic Tandai Batas Baca
                    await Shell.Current.DisplayAlert("Success", "Ayat Berhasil Di Tandai", "OK");
                }
                else if (action == "Bagikan")
                {
                    //string sharedFormat = string.Format("{0,50}{0}\n\t{1,-50}{1}", SelectedQuranAyah?.textArab, SelectedQuranAyah?.translationId);
                    string sharedFormat = $"{SelectedQuranAyah?.textArab,30}\n{SelectedQuranAyah?.translationId,-30}";

                    await Share.Default.RequestAsync(new ShareTextRequest
                    {
                        Text = sharedFormat,
                        Title = title
                    });
                    // Logic Bagikan Ayat
                }
                else if (action == "Lapor Kesalahan")
                {
                    // Logic Lapor Kesalahan
                }

                SelectedQuranAyah = null;
            }
        }
    }

    async Task GetAllAyahBySurah()
    {
        AyahList.Clear();

        string surahNum =  _preferences.Get("QuranSearch", "1");

        if (surahNum.Contains(":"))
        {
            string[] searchSplited = surahNum.Split(":");
            surahNum = searchSplited[0];
        }

        QuranSurah theSurah = await _quranSurahPersistence.GetOneSurah(int.Parse(surahNum));
        SurahTitle = string.Format("{0}. {1}", theSurah?.number ,theSurah?.nameTransliterationId?.ToUpper() ?? "1. AL-FATIHAH");

        var all_ayah = await _quranAyahPersistence.GetAyahBySurahAsync(int.Parse(surahNum));
        all_ayah.ForEach(x => AyahList.Add(x));
    }

    [RelayCommand]
    async Task InitQuranAyahListPage()
    {
        try
        {
            IsLoading = true;
            await GetAllAyahBySurah();
        }
        finally
        {
            OnPropertyChanged(nameof(AyahList));

            // Ensure UI updates before scrolling
            await Task.Delay(200);

            // Notify View to scroll
            ScrollToRequested?.Invoke(this, new EventArgs());

            IsLoading = false;
        }
    }

    public event EventHandler ScrollToRequested;

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    #endregion
}
