namespace MimApp.Services.Contracts
{
    public interface IQuranApi
    {
       Task<IEnumerable<QuranSurah>> GetQuranSurahAsync();
    }
}
