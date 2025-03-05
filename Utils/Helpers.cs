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

        public static string[] surahVerses = new string[]
        {
            "7","286","200","176","120","165","206","75","129","109","123","111","43","52",
            "99","128","111","110","98","135","112","78","118","64","77","227","93","88","69",
            "60","34","30","73","54","45","83","182","88","75","85","54","53","89","59","37",
            "35","38","29","18","45","60","49","62","55","78","96","29","22","24","13","14",
            "11","11","18","12","12","30","52","52","44","28","28","20","56","40","31","50",
            "40","46","42","29","19","36","25","22","17","19","26","30","20","15","21","11",
            "8","8","19","5","8","8","11","11","8","3","9","5","4","7","3","6","3","5","4","5","6"
        };

        public static string[] surahList = {
            "1 - Al-Fatihah",
            "2 - Al-Baqarah",
            "3 - Ali 'Imran",
            "4 - An-Nisa'",
            "5 - Al-Ma'idah",
            "6 - Al-An'am",
            "7 - Al-A'raf",
            "8 - Al-Anfal",
            "9 - At-Taubah",
            "10 - Yunus",
            "11 - Hud",
            "12 - Yusuf",
            "13 - Ar-Ra'd",
            "14 - Ibrahim",
            "15 - Al-Hijr",
            "16 - An-Nahl",
            "17 - Al-Isra'",
            "18 - Al-Kahf",
            "19 - Maryam",
            "20 - Taha",
            "21 - Al-Anbiya'",
            "22 - Al-Hajj",
            "23 - Al-Mu'minun",
            "24 - An-Nur",
            "25 - Al-Furqan",
            "26 - Asy-Syu'ara'",
            "27 - An-Naml",
            "28 - Al-Qasas",
            "29 - Al-'Ankabut",
            "30 - Ar-Rum",
            "31 - Luqman",
            "32 - As-Sajdah",
            "33 - Al-Ahzab",
            "34 - Saba'",
            "35 - Fatir",
            "36 - Yasin",
            "37 - As-Saffat",
            "38 - Sad",
            "39 - Az-Zumar",
            "40 - Gafir",
            "41 - Fussilat",
            "42 - Asy-Syura",
            "43 - Az-Zukhruf",
            "44 - Ad-Dukhan",
            "45 - Al-Jasiyah",
            "46 - Al-Ahqaf",
            "47 - Muhammad",
            "48 - Al-Fath",
            "49 - Al-Hujurat",
            "50 - Qaf",
            "51 - Az-Zariyat",
            "52 - At-Tur",
            "53 - An-Najm",
            "54 - Al-Qamar",
            "55 - Ar-Rahman",
            "56 - Al-Waqi'ah",
            "57 - Al-Hadid",
            "58 - Al-Mujadalah",
            "59 - Al-Hasyr",
            "60 - Al-Mumtahanah",
            "61 - As-Saff",
            "62 - Al-Jumu'ah",
            "63 - Al-Munafiqun",
            "64 - At-Tagabun",
            "65 - At-Talaq",
            "66 - At-Tahrim",
            "67 - Al-Mulk",
            "68 - Al-Qalam",
            "69 - Al-Haqqah",
            "70 - Al-Ma'arij",
            "71 - Nuh",
            "72 - Al-Jinn",
            "73 - Al-Muzzammil",
            "74 - Al-Muddassir",
            "75 - Al-Qiyamah",
            "76 - Al-Insan",
            "77 - Al-Mursalat",
            "78 - An-Naba'",
            "79 - An-Nazi'at",
            "80 - 'Abasa",
            "81 - At-Takwir",
            "82 - Al-Infitar",
            "83 - Al-Mutaffifin",
            "84 - Al-Insyiqaq",
            "85 - Al-Buruj",
            "86 - At-Tariq",
            "87 - Al-A'la",
            "88 - Al-Gasyiyah",
            "89 - Al-Fajr",
            "90 - Al-Balad",
            "91 - Asy-Syams",
            "92 - Al-Lail",
            "93 - Ad-Duha",
            "94 - Asy-Syarh",
            "95 - At-Tin",
            "96 - Al-'Alaq",
            "97 - Al-Qadr",
            "98 - Al-Bayyinah",
            "99 - Az-Zalzalah",
            "100 - Al-'Adiyat",
            "101 - Al-Qari'ah",
            "102 - At-Takasur",
            "103 - Al-'Asr",
            "104 - Al-Humazah",
            "105 - Al-Fil",
            "106 - Quraisy",
            "107 - Al-Ma'un",
            "108 - Al-Kausar",
            "109 - Al-Kafirun",
            "110 - An-Nasr",
            "111 - Al-Lahab",
            "112 - Al-Ikhlas",
            "113 - Al-Falaq",
            "114 - An-Nas"
        };


        #endregion
    }
}
