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
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Call the command manually when the page appears
        if (ViewModel.InitPageCommand.CanExecute(null))
        {
            await ViewModel.InitPageCommand.ExecuteAsync(null);
        }
    }
}
