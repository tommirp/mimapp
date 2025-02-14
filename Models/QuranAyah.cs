namespace MimApp.Models
{
    public class QuranAyah
    {
        public int numberOfSurah { get; set; }
        public int numberInQuran { get; set; }
        public int numberInSurah { get; set; }
        public int numberOfVerses { get; set; } // for surah
        public string surahName { get; set; } // for surah
        public string surahRevelationId { get; set; } // for surah
        public string surahTranslationId { get; set; } // for surah
        public string surahTafsir { get; set; } // for surah

        public int juz { get; set; }
        public int page { get; set; }
        public int manzil { get; set; }
        public int ruku { get; set; }
        public int hizbQuarter { get; set; }
        public bool sajdaRecommended { get; set; }
        public bool sajdaObligatory { get; set; }
        public string? textArab { get; set; }
        public string? textTransliteration { get; set; }
        public string? translationEn { get; set; }
        public string? translationId { get; set; }
        public string? audioPrimary { get; set; }
        public string? audioSecondary { get; set; }
        public string? tafsirShort { get; set; }
        public string? tafsirLong { get; set; }
        public string? bgColor { get; set; }
        public string? realAudio { get; set; }
        public bool? isMarked { get; set; }

        public bool? showTranslate { get; set; }
        public bool? showLatin { get; set; }
        public int? arabFontSize { get; set; }
        public int? translateFontSize { get; set; }
    }
}