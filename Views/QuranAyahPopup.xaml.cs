using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Xaml;

namespace MimApp.Views;

[XamlCompilation(XamlCompilationOptions.Compile)] // Ensure XAML Compilation
public partial class QuranAyahPopup : Popup
{
    public QuranAyahPopup(QuranSurah Surah, QuranAyah Ayah)
    {
        this.LoadFromXaml(typeof(QuranAyahPopup)); // Manually load XAML
        Ayah.surahName = $"{Ayah.numberOfSurah}.{Surah.nameTransliterationId} Ayat {Ayah.numberInSurah}"; // Set the SurahName
        Ayah.numberOfVerses = Surah.numberOfVerses;
        Ayah.surahRevelationId = Surah.revelationId;
        Ayah.surahTafsir = Surah.tafsir;
        Ayah.surahTranslationId = Surah.nameTranslationId;
        BindingContext = Ayah; // Set the BindingContext to the SelectedQuranAyah
    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        Close(); // Closes the popup
    }
}
