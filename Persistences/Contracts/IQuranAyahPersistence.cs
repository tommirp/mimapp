namespace MimApp.Persistences.Contracts
{
    public interface IQuranAyahPersistence
    {
        Task <bool> InsertAllItemAsync(List<QuranAyah>? ayahList);
        Task<bool> AyahCheck();
        Task<List<string>> GetAllAyahNumberListBySurah(string surah);
        Task<QuranAyah> GetOneAyah(int numberOfSurah, int numberOfVerse);
        Task<List<QuranAyah>> GetAyahBySurahAsync(int numberOfSurah);
        Task<List<QuranAyah>> GetAyahByJuzAsync(int Juz);
        Task<List<string>> GetAyahByKeyword(string keyword);

        Task<bool> DeleteAllItemsAsync();
    }
}
