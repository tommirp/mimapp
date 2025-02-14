namespace MimApp.Views.Quran;

public partial class QuranAppSetting : ContentPage
{
    public QuranViewModel ViewModel { get; }

    public QuranAppSetting(QuranViewModel quranViewModel)
    {
        InitializeComponent();
        ViewModel = quranViewModel;
        BindingContext = quranViewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Call the command manually when the page appears
        if (ViewModel.InitAppSettingPageCommand.CanExecute(null))
        {
            await ViewModel.InitAppSettingPageCommand.ExecuteAsync(null);
        }
    }
}
