namespace MimApp.Views;

public partial class AppSettingsPage : ContentPage
{
    public AppSettingsViewModel ViewModel { get; }

    public AppSettingsPage(AppSettingsViewModel appSettingsViewModel)
    {
        InitializeComponent();
        ViewModel = appSettingsViewModel;
        BindingContext = appSettingsViewModel;
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
