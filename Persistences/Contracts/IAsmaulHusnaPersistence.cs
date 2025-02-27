namespace MimApp.Persistences.Contracts
{
    public interface IAsmaulHusnaPersistence
    {
        Task<bool> InsertAllItemAsync(List<QuranAsmaulHusna>? list);
        Task<List<QuranAsmaulHusna>> GetAllQuranAsmaulHusna();
        Task<bool> DeleteAllItemsAsync();
        Task<bool> AsmaulHusnaCheck();
    }
}
