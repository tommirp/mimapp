namespace MimApp.Persistences.Contracts
{
    public interface IQuranSurahPersistence
    {
        Task<bool> InsertAllItemAsync(List<QuranSurah>? surahList);
        Task<List<QuranSurah>> GetAllSurah();
        Task<QuranSurah> GetOneSurah(int numberOfSurah);

        Task<bool> DeleteAllItemsAsync();
        Task<bool> SurahCheck();
    }
}
