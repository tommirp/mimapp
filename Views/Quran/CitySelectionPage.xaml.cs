namespace MimApp.Views.Quran;

public partial class CitySelectionPage : ContentPage
{
    public QuranViewModel ViewModel { get; }

    public CitySelectionPage(QuranViewModel quranViewModel)
    {
        InitializeComponent();
        ViewModel = quranViewModel;
        BindingContext = quranViewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Call the command manually when the page appears
        if (ViewModel.InitCitySelectionPageCommand.CanExecute(null))
        {
            await ViewModel.InitCitySelectionPageCommand.ExecuteAsync(null);
        }
    }

    private async void SelectCityTapped(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string selectedCard)
        {
            await ViewModel.SelectCity(selectedCard);
        }
    }
}
