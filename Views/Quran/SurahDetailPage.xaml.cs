using CommunityToolkit.Maui.Views;
using MimApp.Utils;

namespace MimApp.Views.Quran;

public partial class SurahDetailPage : ContentPage
{
    public QuranViewModel ViewModel { get; }
    private readonly IPreferences _preferences;

    public SurahDetailPage(QuranViewModel quranViewModel, IPreferences preferences)
    {
        InitializeComponent();
        ViewModel = quranViewModel;
        BindingContext = quranViewModel;
        _preferences = preferences;

        // Subscribe to event to scroll when data is ready
        ViewModel.ScrollToRequested += (s, e) => ScrollToTarget();
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Call the command manually when the page appears
        if (ViewModel.InitQuranAyahListPageCommand.CanExecute(null))
        {
            ViewModel.InitQuranAyahListPageCommand.ExecuteAsync(null);
        }
    }
    private async void ScrollToTarget()
    {
        await Task.Delay(100); // Ensure UI is ready

        if (ViewModel.AyahList.Count > 0)
        {
            string qsearch = _preferences.Get("QuranSearch", "1");
            if (qsearch.Contains(":"))
            {
                string[] searchSplited = qsearch.Split(":");
                string surah = searchSplited[0];
                string ayah = searchSplited[1];

                var itemToScrollTo = ViewModel.AyahList.FirstOrDefault(x => x.numberOfSurah == int.Parse(surah) && x.numberInSurah == int.Parse(ayah));
                if (itemToScrollTo != null)
                {
                    QuranSurahList.ScrollTo(itemToScrollTo, position: ScrollToPosition.Start, animate: false);
                }
            }

        }
    }
}
