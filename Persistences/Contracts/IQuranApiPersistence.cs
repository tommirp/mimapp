using MimApp.Models;

namespace MimApp.Persistences.Contracts
{
    public interface IQuranApiPersistence
    {
        Task<bool> AddItemAsync(QuranSurah item);

        Task<IEnumerable<QuranSurah>> GetAllItems();
        Task<QuranSurah> GetOneItem(int numberOfVerse);

        Task<bool> DeleteAllItemsAsync();
    }
}
