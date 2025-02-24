using DevExpress.Maui.Editors;
using MimApp.Persistences.Contracts;

namespace MimApp.Views.Quran;

public partial class QuranSearchPage : ContentPage
{
    public MainViewModel ViewModel { get; }
    private readonly IQuranAyahPersistence _quranAyahPersistence;

    public QuranSearchPage(MainViewModel mainViewModel, IQuranAyahPersistence quranAyahPersistence)
    {
        InitializeComponent();
        ViewModel = mainViewModel;
        BindingContext = mainViewModel;
        _quranAyahPersistence = quranAyahPersistence;
    }

    void OnAutoCompleteRequested(object sender, ItemsRequestEventArgs e)
    {
        e.Request = () => {
            if (e.Text != null && e.Text.Contains(":"))
            {
                return new List<string>() { e.Text };
            }
            else
            {
                var isNumeric = int.TryParse(e.Text, out int n);
                if (e.Text.Length > 3 || isNumeric)
                {
                    List<string> QuranSearchList = new List<string>();

                    var search_result = _quranAyahPersistence.GetAyahByKeyword(e.Text).Result.ToList();
                    search_result.ForEach(x => QuranSearchList.Add(x));
                    return QuranSearchList;
                }
            }

            return null;
        };
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        // Call the command manually when the page appears
        if (ViewModel.InitQuranSearchPageCommand.CanExecute(null))
        {
            await ViewModel.InitQuranSearchPageCommand.ExecuteAsync(null);
        }
    }
    private async void SelectQuranTapped(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string selectedCard)
        {
            await ViewModel.SelectSearchContent(selectedCard);
        }
    }
}
