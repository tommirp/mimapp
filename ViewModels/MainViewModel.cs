namespace MimApp.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    public MainViewModel()
    {
        SearchText = String.Empty;
    }

    [ObservableProperty]
    string searchText;

    [RelayCommand]
    async Task SearchQuran()
    {
        string result = SearchText;
        // Do Search
    }
}
