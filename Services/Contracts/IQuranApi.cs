namespace MimApp.Services.Contracts
{
    public interface IQuranApi
    {
        Task OnlineSyncQuran();
        Task<bool> GetQuranAsmaulHusnaAsync();
        Task<bool> SyncSholatTimeByMonthAsync(string cityCode);
        Task<List<CityCodes>> SyncCityCodesAsync();
    }
}
