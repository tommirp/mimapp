namespace MimApp.Utils
{
    public static class Helpers
    {
        #region General

        public const string BaseApiUrl = "https://quran-api-data.vercel.app";

        public const string DatabaseFilename = "MimDBLocal.db3";

        public const string GDriveUrl = "https://www.googleapis.com/drive/v3/files";

        public const SQLite.SQLiteOpenFlags Flags =
            // open the database in read/write mode
            SQLite.SQLiteOpenFlags.ReadWrite |
            // create the database if it doesn't exist
            SQLite.SQLiteOpenFlags.Create |
            // enable multi-threaded database access
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename);

        #endregion
    }
}
