using DevExpress.Maui.Editors;
using Microsoft.Maui;
using MimApp.Persistences.Contracts;

namespace MimApp.Views;

public partial class MainPage : ContentPage
{
    public MainViewModel ViewModel { get; }
    private readonly IQuranSurahPersistence _quranSurahPersistence;

    public MainPage(MainViewModel mainViewModel, IQuranSurahPersistence quranSurahPersistence)
	{
		InitializeComponent();
        ViewModel = mainViewModel;
        BindingContext = mainViewModel;
        _quranSurahPersistence = quranSurahPersistence;
    }
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Call the command manually when the page appears
        if (ViewModel.InitPageCommand.CanExecute(null))
        {
            await ViewModel.InitPageCommand.ExecuteAsync(null);
        }
    }
    void OnAutoCompleteRequested(object sender, ItemsRequestEventArgs e)
    {
        e.Request = () => {
            if (e.Text != null && e.Text.Contains(":"))
            {
                return new List<string>() { e.Text };
            }
            else {
                var isNumeric = int.TryParse(e.Text, out int n);
                if (e.Text.Length > 3 || isNumeric)
                {
                    return _quranSurahPersistence.GetSurahNameByKeyword(e.Text).Result.ToList();
                }
            }
            
            return null;
        };
    }
}
