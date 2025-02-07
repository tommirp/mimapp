namespace MimApp.Views.Quran;

public partial class SurahDetailPage : ContentPage
{
    public QuranViewModel ViewModel { get; }

    public SurahDetailPage(QuranViewModel quranViewModel)
    {
        InitializeComponent();
        ViewModel = quranViewModel;
        BindingContext = quranViewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Call the command manually when the page appears
        if (ViewModel.InitQuranAyahListPageCommand.CanExecute(null))
        {
            await ViewModel.InitQuranAyahListPageCommand.ExecuteAsync(null);
        }
    }
}
