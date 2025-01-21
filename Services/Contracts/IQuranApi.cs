namespace MimApp.Services.Contracts
{
    public interface IQuranApi
    {
        Task OnlineSyncQuran();
        Task<List<QuranAsmaulHusna>?> GetQuranAsmaulHusnaAsync();
        Task<bool> SyncSholatTimeByMonthAsync(string cityCode);
        Task<List<CityCodes>> SyncCityCodesAsync();
    }
}
