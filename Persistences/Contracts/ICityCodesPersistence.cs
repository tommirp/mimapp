namespace MimApp.Persistences.Contracts
{
    public interface ICityCodesPersistence
    {
        Task<bool> InsertAllItemAsync(List<CityCodes>? list);
        Task<List<CityCodes>> GetAllCityCodes();
        Task<bool> DeleteAllItemsAsync();
    }
}
