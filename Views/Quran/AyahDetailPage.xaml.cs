namespace MimApp.Views.Quran;

public partial class AyahDetailPage : ContentPage
{

    public AyahDetailPage(QuranViewModel quranViewModel)
    {
        InitializeComponent();
        BindingContext = quranViewModel;
    }
}
