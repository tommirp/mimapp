namespace MimApp.Models
{
    public class QuranSurah
    {
        public int Number { get; set; }
        public int Sequence { get; set; }
        public int NumberOfVerses { get; set; }
        public string? NameShort { get; set; }
        public string? NameLong { get; set; }
        public string? NameTransliterationEn { get; set; }
        public string? NameTransliterationId { get; set; }
        public string? NameTranslationEn { get; set; }
        public string? NameTranslationId { get; set; }
        public string? RevelationArab { get; set; }
        public string? RevelationEn { get; set; }
        public string? RevelationId { get; set; }
        public string? Tafsir { get; set; }
        public string? PreBismillahArab { get; set; }
        public string? PreBismillahEn { get; set; }
        public string? PreBismillahTranslationEn { get; set; }
        public string? PreBismillahTranslationId { get; set; }
        public string? PreBismillahAudioPrimary { get; set; }
        public string? PreBismillahAudioSecondary { get; set; }
        public string? PreBismillahAudioAlternative { get; set; }
        //public List<QuranAyah>? Verses { get; set; }
    }
}