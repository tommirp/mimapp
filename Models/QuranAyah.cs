namespace MimApp.Models
{
    public class QuranAyah
    {
        public int NumberOfSurah { get; set; }
        public int NumberInQuran { get; set; }
        public int NumberInSurah { get; set; }
        public int Juz { get; set; }
        public int Page { get; set; }
        public int Manzil { get; set; }
        public int Ruku { get; set; }
        public int HizbQuarter { get; set; }
        public bool SajdaRecommended { get; set; }
        public bool SajdaObligatory { get; set; }
        public string? TextArab { get; set; }
        public string? TextTransliteration { get; set; }
        public string? TranslationEn { get; set; }
        public string? TranslationId { get; set; }
        public string? AudioPrimary { get; set; }
        public string? AudioSecondary { get; set; }
        public string? TafsirShort { get; set; }
        public string? TafsirLong { get; set; }
    }
}