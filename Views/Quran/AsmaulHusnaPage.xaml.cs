namespace MimApp.Views.Quran;

public partial class AsmaulHusnaPage : ContentPage
{
    public AsmaulHusnaViewModel ViewModel { get; }

    public AsmaulHusnaPage(AsmaulHusnaViewModel asmaulHusnaViewModel)
    {
        InitializeComponent();
        ViewModel = asmaulHusnaViewModel;
        BindingContext = asmaulHusnaViewModel;
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
