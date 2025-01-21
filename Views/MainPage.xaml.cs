namespace MimApp.Views;

public partial class MainPage : ContentPage
{
    public MainViewModel ViewModel { get; }

    public MainPage(MainViewModel mainViewModel)
	{
		InitializeComponent();
        ViewModel = mainViewModel;
        BindingContext = mainViewModel;
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
