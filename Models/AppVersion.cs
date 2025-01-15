namespace MimApp.Models
{
    public class AppVersion
    {
        public bool? needUpdateApp { get; set; }
        public int? versionId { get; set; }
        public string version { get; set; }
        public string updateList { get; set; }
    }
}
