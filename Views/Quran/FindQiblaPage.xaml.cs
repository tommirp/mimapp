namespace MimApp.Views.Quran;

public partial class FindQiblaPage : ContentPage
{
    public SholatTimesViewModel ViewModel { get; }

    public FindQiblaPage(SholatTimesViewModel sholatTimesViewModel)
    {
        InitializeComponent();
        ViewModel = sholatTimesViewModel;
        BindingContext = sholatTimesViewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Call the command manually when the page appears
        if (ViewModel.InitQiblaPageCommand.CanExecute(null))
        {
            await ViewModel.InitQiblaPageCommand.ExecuteAsync(null);
        }
    }
}
