using CommunityToolkit.Maui.Core.Primitives;
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
    private async void OnAyahNumberTapped(object sender, EventArgs e)
    {
        if (sender is VisualElement visualElement && visualElement.BindingContext is QuranAyah ayah)
        {
            var result = await Shell.Current.DisplayPromptAsync("Masukkan Nomor Ayat", null, "Menuju Ayat", "Batalkan", null, 3, Keyboard.Numeric, null);
            if (result != null)
            {
                var itemToScrollTo = ViewModel.AyahList.FirstOrDefault(x => x.numberOfSurah == ayah.numberOfSurah && x.numberInSurah == int.Parse(result));
                if (itemToScrollTo != null)
                {
                    QuranSurahList.ScrollTo(itemToScrollTo, position: ScrollToPosition.Start, animate: false);
                }
            }
        }
    }

    private void OnPlayPauseClicked(object sender, EventArgs e)
    {
        var stackLayout = (StackLayout)sender;
        var mediaElement = stackLayout.Children.OfType<MediaElement>().FirstOrDefault();
        var mediaImage = stackLayout.Children.OfType<Image>().FirstOrDefault();

        if (mediaElement == null)
            return;

        if (mediaElement.CurrentState == MediaElementState.Playing)
        {
            mediaElement.Pause();
            mediaImage.Source = "playsound.png";
        }
        else
        {
            mediaElement.Play();
            mediaImage.Source = "stopsound.png";
        }
    }
}
