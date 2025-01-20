namespace MimApp.Persistences.Contracts
{
    public interface ISholatTimesPersistence
    {
        Task<bool> InsertAllItemAsync(List<QuranSholatTime>? list);
        Task<QuranSholatTime> GetSholatTimeByDate(string Date);
        Task<bool> DeleteAllItemsAsync();
    }
}
