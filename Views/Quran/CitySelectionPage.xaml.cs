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
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Call the command manually when the page appears
        if (ViewModel.InitPageCommand.CanExecute(null))
        {
            await ViewModel.InitPageCommand.ExecuteAsync(null);
        }
    }
}
