namespace MimApp.Views.Quran;

public partial class CitySelectionPage : ContentPage
{
    public CitySelectionPage(QuranViewModel quranViewModel)
    {
        InitializeComponent();
        BindingContext = quranViewModel;
    }
}
