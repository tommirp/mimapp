using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;

namespace MimApp.ViewModels;

public partial class AsmaulHusnaViewModel : ViewModelBase
{
    private bool _disposed = false;

    private readonly IPreferences _preferences;
    private readonly IConnectivity _connectivity;
    private readonly IAsmaulHusnaPersistence _asmaulHusnaPersistence;
    private readonly IQuranApi _quranApi;

    public AsmaulHusnaViewModel(IPreferences preferences, IConnectivity connectivity, IAsmaulHusnaPersistence asmaulHusnaPersistence, IQuranApi quranApi)
    {
        _preferences = preferences;
        _connectivity = connectivity;
        _asmaulHusnaPersistence = asmaulHusnaPersistence;
        _quranApi = quranApi;
    }

    [ObservableProperty]
    bool isLoading;

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
    ~AsmaulHusnaViewModel()
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

    public ObservableCollection<QuranAsmaulHusna> AsmaulHusnaList { get; set; } = new ObservableCollection<QuranAsmaulHusna>();

    [RelayCommand]
    public async Task InitPage()
    {
        CheckDisposed();

        try
        {
            IsLoading = true;
            bool IsExist = await _asmaulHusnaPersistence.AsmaulHusnaCheck();
            if (!IsExist)
            {
                await _quranApi.GetQuranAsmaulHusnaAsync();
            }
        }
        finally
        {
            var result = await _asmaulHusnaPersistence.GetAllQuranAsmaulHusna();
            result.ForEach(x => AsmaulHusnaList.Add(x));
            IsLoading = false;
        }
    }
}
