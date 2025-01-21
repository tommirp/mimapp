namespace MimApp.Persistences.Contracts
{
    public interface ICityCodesPersistence
    {
        Task<bool> InsertAllItemAsync(List<CityCodes>? list);
        Task<List<CityCodes>> GetAllCityCodes(int limit);
        Task<List<CityCodes>> GetCityByName(string name);
        Task<bool> DeleteAllItemsAsync();
    }
}
