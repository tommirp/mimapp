namespace MimApp.Models
{
    public class QuranSurah
    {
        public int number { get; set; }
        public int sequence { get; set; }
        public int numberOfVerses { get; set; }
        public string? nameShort { get; set; }
        public string? nameLong { get; set; }
        public string? nameTransliterationEn { get; set; }
        public string? nameTransliterationId { get; set; }
        public string? nameTranslationEn { get; set; }
        public string? nameTranslationId { get; set; }
        public string? revelationArab { get; set; }
        public string? revelationEn { get; set; }
        public string? revelationId { get; set; }
        public string? tafsir { get; set; }
        public string? preBismillahArab { get; set; }
        public string? preBismillahEn { get; set; }
        public string? preBismillahTranslationEn { get; set; }
        public string? preBismillahTranslationId { get; set; }
        public string? preBismillahAudioPrimary { get; set; }
        public string? preBismillahAudioSecondary { get; set; }
        public string? preBismillahAudioAlternative { get; set; }
    }
}