using MimApp.Persistences.Contracts;

namespace MimApp.ViewModels;

public partial class SholatTimesViewModel : ViewModelBase
{
    private bool _disposed = false;

    private readonly IPreferences _preferences;
    private readonly IConnectivity _connectivity;
    private readonly ISholatTimesPersistence _sholatTimesPersistence;
    private readonly ICityCodesPersistence _cityCodesPersistence;

    public SholatTimesViewModel(IPreferences preferences, ISholatTimesPersistence sholatTimesPersistence,
        ICityCodesPersistence cityCodesPersistence, IConnectivity connectivity)
    {
        _preferences = preferences;
        _connectivity = connectivity;
        _sholatTimesPersistence = sholatTimesPersistence;
        _cityCodesPersistence = cityCodesPersistence;
    }

    [ObservableProperty]
    bool isLoading;

    [ObservableProperty]
    string tanggal;

    [ObservableProperty]
    string lokasi;

    [ObservableProperty]
    string imsak;

    [ObservableProperty]
    string terbit;

    [ObservableProperty]
    string subuh;

    [ObservableProperty]
    string dhuha;

    [ObservableProperty]
    string zuhur;

    [ObservableProperty]
    string ashar;

    [ObservableProperty]
    string maghrib;

    [ObservableProperty]
    string isya;


    #region IDisposable Implementation
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Free any managed objects here.
            }

            // Free any unmanaged objects here.
            _disposed = true;
        }
    }
    ~SholatTimesViewModel()
    {
        Dispose(false);
    }

    private void CheckDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(QuranViewModel));
        }
    }
    #endregion

    [RelayCommand]
    public async Task InitPage()
    {
        CheckDisposed();

        try
        {
            IsLoading = true;

            var city = _preferences.Get("MyCity", string.Empty);
            if (string.IsNullOrEmpty(city))
            {
                await Shell.Current.DisplayAlert("Error", "No City Selected, Please select a city", "OK");

                await Shell.Current.Navigation.PopToRootAsync();
            }

            if (!string.IsNullOrEmpty(city) && city != "Select City")
            {
                var sholatTimes = await _sholatTimesPersistence.GetSholatTimeByDate(DateTime.Now.ToString("yyyy-MM-dd"));
                if (sholatTimes == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Data Waktu Sholat Belum Ada", "OK");

                    await Shell.Current.Navigation.PopToRootAsync();
                }

                Tanggal = sholatTimes?.tanggal ?? string.Empty;
                Lokasi = sholatTimes?.lokasi ?? string.Empty;
                Imsak = sholatTimes?.imsak ?? string.Empty;
                Terbit = sholatTimes?.terbit ?? string.Empty;
                Subuh = sholatTimes?.subuh ?? string.Empty;
                Dhuha = sholatTimes?.dhuha ?? string.Empty;
                Zuhur = sholatTimes?.dzuhur ?? string.Empty;
                Ashar = sholatTimes?.ashar ?? string.Empty;
                Maghrib = sholatTimes?.maghrib ?? string.Empty;
                Isya = sholatTimes?.isya ?? string.Empty;
            }
        }
        finally
        {
            IsLoading = false;
        }
    }
}
