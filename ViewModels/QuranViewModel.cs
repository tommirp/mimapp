using MimApp.Utils;
using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CommunityToolkit.Maui.Views;
using DevExpress.Maui.Mvvm;
using DevExpress.Data.Extensions;
using System;

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
        all_cities.ForEach(x => CityCodeList.Add(string.Format("{0} - {1}", x.id, x.lokasi)));
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
                all_cities.ForEach(x => CityCodeList.Add(string.Format("{0} - {1}", x.id, x.lokasi)));
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
    int surahNumber;

    public ObservableCollection<QuranAyah> AyahList { get; set; } = new ObservableCollection<QuranAyah>();   

    [RelayCommand]
    public async Task AyahMenuSelected(QuranAyah SelectedQuranAyah)
    {
        QuranSurah theSurah = await _quranSurahPersistence.GetOneSurah(SelectedQuranAyah.numberInSurah);
        string title = string.Format("{0}. {1} : {2}", SelectedQuranAyah.numberOfSurah, theSurah?.nameTransliterationId?.ToUpper(), SelectedQuranAyah.numberInSurah);

        string[] btns = ["Info Detail", "Salin Ayat", "Tandai Batas Baca", "Bagikan", "Lapor Kesalahan"];

        string action = await Shell.Current.DisplayActionSheet(title, "Close", null, btns);

        if (btns.Contains(action))
        {
            if (action == "Info Detail")
            {
                Shell.Current.CurrentPage.ShowPopup(new QuranAyahPopup(theSurah, SelectedQuranAyah));
                // Go To Ayah Detail Page
            }
            else if (action == "Salin Ayat")
            {
                string arabicText = SelectedQuranAyah?.textArab ?? "";
                string translation = SelectedQuranAyah?.translationId ?? "";

                int columnWidth = 50;
                // Unicode Right-to-Left Embedding (RLE) → Forces Arabic text to be RTL
                string rtlMarker = "\u202B";
                // Unicode Left-to-Right Embedding (LRE) → Forces English text to be LTR
                string ltrMarker = "\u202A";
                // Unicode Pop Directional Formatting (PDF) → Resets text direction to normal
                string pdf = "\u202C";
                string formattedText = $"{rtlMarker}{arabicText.PadLeft(columnWidth)}{pdf}\n\n\t\t{ltrMarker}{translation.PadRight(columnWidth)}{pdf}";

                await Clipboard.SetTextAsync(formattedText);
                await Shell.Current.DisplayAlert("Success", "Ayat Berhasil Di Salin ke Clipboard", "OK");
            }
            else if (action == "Tandai Batas Baca")
            {
                try
                {
                    // Logic Tandai Batas Baca
                    string tandaBaca = string.Format("{0}:{1}", SelectedQuranAyah.numberOfSurah, SelectedQuranAyah.numberInSurah);
                    _preferences.Set("AppSetting_TandaBaca", tandaBaca, null);

                    await GetAllAyahBySurah();
                }
                finally
                {
                    OnPropertyChanged(nameof(AyahList));

                    // Ensure UI updates before scrolling
                    await Task.Delay(200);

                    // Notify View to scroll
                    ScrollToRequested?.Invoke(this, new EventArgs());

                    await Shell.Current.DisplayAlert("Success", "Ayat Berhasil Di Tandai", "OK");
                }
            }
            else if (action == "Bagikan")
            {
                string arabicText = SelectedQuranAyah?.textArab ?? "";
                string translation = SelectedQuranAyah?.translationId ?? "";

                int columnWidth = 50;
                // Unicode Right-to-Left Embedding (RLE) → Forces Arabic text to be RTL
                string rtlMarker = "\u202B";
                // Unicode Left-to-Right Embedding (LRE) → Forces English text to be LTR
                string ltrMarker = "\u202A";
                // Unicode Pop Directional Formatting (PDF) → Resets text direction to normal
                string pdf = "\u202C";
                string formattedText = $"{rtlMarker}{arabicText.PadLeft(columnWidth)}{pdf}\n\n\t\t{ltrMarker}{translation.PadRight(columnWidth)}{pdf}";

                await Share.Default.RequestAsync(new ShareTextRequest
                {
                    Text = formattedText,
                    Title = title
                });
                // Logic Bagikan Ayat
            }
            else if (action == "Lapor Kesalahan")
            {
                DateTime dt = new DateTime();
                string MailSubject = string.Format("Laporan MimApp Surah {0} Ayat {1} ({2})", SelectedQuranAyah.numberOfSurah, SelectedQuranAyah.numberInSurah, dt.ToString());

                await MailHelper.OpenEmailClientAsync("tech@jms.or.id", MailSubject, "Kepada tim Developer Mim App.");

            }
        }
    }

    [RelayCommand]
    public async Task NextQuranSurah()
    {
        try
        {
            IsLoading = true;
            int NewSurah = SurahNumber + 1;
            if (NewSurah > 114)
            {
                NewSurah = 1;
            }

            _preferences.Set("QuranSearch", NewSurah.ToString());
            await GetAllAyahBySurah();
        }
        finally
        {
            // Ensure UI updates before scrolling
            await Task.Delay(200);

            IsLoading = false;
        }
    }


    [RelayCommand]
    public async Task PrevQuranSurah()
    {
        try
        {
            IsLoading = true;
            int NewSurah = SurahNumber - 1;
            if (NewSurah < 1)
            {
                NewSurah = 114;
            }

            _preferences.Set("QuranSearch", NewSurah.ToString());
            await GetAllAyahBySurah();
        }
        finally
        {
            // Ensure UI updates before scrolling
            await Task.Delay(200);

            IsLoading = false;
        }
    }

    async Task GetAllAyahBySurah()
    {
        AyahList.Clear();

        string surahNum = _preferences.Get("QuranSearch", "1");

        if (surahNum.Contains(":"))
        {
            string[] searchSplited = surahNum.Split(":");
            surahNum = searchSplited[0];

            if (surahNum == null)
            {
                surahNum = "1";
                _preferences.Set("QuranSearch", "1");
            }

            if (int.Parse(surahNum) > 114)
            {
                _preferences.Set("QuranSearch", "1");
                surahNum = "1";
            }
        }
        else if (int.Parse(surahNum) > 114)
        {
            _preferences.Set("QuranSearch", "1");
            surahNum = "1";
        }

        SurahNumber = int.Parse(surahNum);

        QuranSurah theSurah = await _quranSurahPersistence.GetOneSurah(int.Parse(surahNum));
        SurahTitle = string.Format("{0}. {1}", theSurah?.number ,theSurah?.nameTransliterationId?.ToUpper() ?? "1. AL-FATIHAH");

        var all_ayah = await _quranAyahPersistence.GetAyahBySurahAsync(int.Parse(surahNum));
        all_ayah.ForEach(x => {
            x.bgColor = x.numberInSurah % 2 == 0 ? "#f2f2f2" : "White";
            x.realAudio = x.audioPrimary == null ? x.audioSecondary : x.audioPrimary;

            bool ayahMarked = false;
            string getMarked = _preferences.Get("AppSetting_TandaBaca", string.Empty);
            if (!string.IsNullOrEmpty(getMarked) && getMarked.Contains(":"))
            {
                string[] getMarkedSplited = getMarked.Split(":");
                string q1 = getMarkedSplited[0];
                string q2 = getMarkedSplited[1];

                if (x.numberOfSurah == int.Parse(q1) && x.numberInSurah == int.Parse(q2))
                {
                    ayahMarked = true;
                }
            }

            x.isMarked = ayahMarked;
            AyahList.Add(x);
        });
    }

    [RelayCommand]
    public async Task GoToAppSettings()
    {
        await Shell.Current.GoToAsync(nameof(AppSettingsPage));
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
