namespace MimApp.Models
{
    public class UserWithToken
    {
        public string? userName { get; set; }
        public string jwtToken { get; set; }
        public string jwtRefreshToken { get; set; }
        public int expiresIn { get; set; }
        public List<string> role { get; set; }
        public string team { get; set; }
        public string title { get; set; }
        public DateTime expireDate { get; set; }
        public string mfacode { get; set; }
        public string message { get; set; }
        public string group { get; set; }
        public string gdriveKey { get; set; }
        public string gdriveFolderID { get; set; }
    }
}
