namespace MimApp.Persistences.Contracts
{
    public interface IQuranSurahPersistence
    {
        Task<bool> InsertAllItemAsync(List<QuranSurah>? surahList);
        Task<bool> SurahCheck();
        Task<QuranSurah> GetOneSurah(int numberOfSurah);
        Task<List<QuranSurah>> GetAllSurah();
        Task<List<string>> GetAllSurahNameList();
        
        Task<List<string>> GetSurahNameByKeyword(string keyword);

        Task<bool> DeleteAllItemsAsync();
    }
}
