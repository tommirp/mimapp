namespace MimApp.Persistences.Contracts
{
    public interface IGeneralMetaPersistence
    {
        Task<bool> InsertAllItemAsync(List<GeneralMetaData>? list);
        Task<List<GeneralMetaData>> GetAllGeneralMetaData(int limit);
        Task<List<GeneralMetaData>> GetMetaByName(string name);
        Task<List<GeneralMetaData>> GetMetaByType(string type);
        Task<bool> DeleteAllItemsAsync();
        Task<bool> DeleteAllItemsByTypeAsync(string type);
    }
}
