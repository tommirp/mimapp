using MimApp.Persistences.Contracts;

namespace MimApp.Views.Quran;

public partial class SholatTimesPage : ContentPage
{
    public SholatTimesViewModel ViewModel { get; }
    private readonly ISholatTimesPersistence _sholatTimesPersistence;

    public SholatTimesPage(SholatTimesViewModel sholatTimesViewModel, ISholatTimesPersistence sholatTimesPersistence)
    {
        InitializeComponent();
        ViewModel = sholatTimesViewModel;
        BindingContext = sholatTimesViewModel;
        _sholatTimesPersistence = sholatTimesPersistence;
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
