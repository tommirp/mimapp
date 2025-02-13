namespace MimApp.Models
{
    public class AppSettings
    {
        public bool? showTranslate { get; set; }
        public bool? showLatin { get; set; }
        public int? arabFontSize { get; set; }
        public int? translateFontSize { get; set; }
        public string? urlYoutubeMekah { get; set; }
        public string? urlYoutubeMadinah { get; set; }
        public string? tandaBacaSurah { get; set; } // Surah:Ayah
        public DateTime? tandaBacaTimestamp { get; set; } // Last Tanda Baca Timestamp
    }
}
