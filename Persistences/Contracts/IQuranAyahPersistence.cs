namespace MimApp.Persistences.Contracts
{
    public interface IQuranAyahPersistence
    {
        Task <bool> InsertAllItemAsync(List<QuranAyah>? ayahList);
        Task<QuranAyah> GetOneAyah(int numberOfSurah, int numberOfVerse);
        Task<List<QuranAyah>> GetAyahBySurahAsync(int numberOfSurah);
        Task<List<QuranAyah>> GetAyahByJuzAsync(int Juz);

        Task<bool> DeleteAllItemsAsync();
    }
}
