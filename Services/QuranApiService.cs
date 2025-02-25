using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;
using MimApp.Utils;

namespace MimApp.Services
{
    public class QuranApiService : IQuranApi
    {
        private readonly HttpClient _httpClient;
        private readonly IConnectivity _connectivity;
        private readonly IQuranSurahPersistence _quranSurahPersistence;
        private readonly IQuranAyahPersistence _quranAyahPersistence;
        private readonly ISholatTimesPersistence _sholatTimesPersistence;
        private readonly ICityCodesPersistence _cityCodesPersistence;
        private readonly IGeneralMetaPersistence _generalMetaPersistence;

        public QuranApiService(HttpClient httpClient, IConnectivity connectivity, IPreferences preferences,
            IQuranSurahPersistence quranSurahPersistence, IQuranAyahPersistence quranAyahPersistence,            
            ISholatTimesPersistence sholatTimesPersistence, ICityCodesPersistence cityCodesPersistence, IGeneralMetaPersistence generalMetaPersistence)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Helpers.BaseApiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            int timeOut = int.Parse(preferences.Get("TimeOut", "60"));
            _httpClient.Timeout = TimeSpan.FromSeconds(timeOut);

            _connectivity = connectivity;
            _quranSurahPersistence = quranSurahPersistence;
            _quranAyahPersistence = quranAyahPersistence;
            _sholatTimesPersistence = sholatTimesPersistence;
            _cityCodesPersistence = cityCodesPersistence;
            _generalMetaPersistence = generalMetaPersistence;
        }

        private async Task<bool> GetQuranSurahAsync()
        {
            try
            {

                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/surah");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        List<QuranSurah>? data = JsonSerializer.Deserialize<List<QuranSurah>>(content);

                        await _quranSurahPersistence.DeleteAllItemsAsync();
                        await _quranSurahPersistence.InsertAllItemAsync(data);

                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> GetQuranAyahAsync()
        {
            try
            {
                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/ayah");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        List<QuranAyah>? data = JsonSerializer.Deserialize<List<QuranAyah>>(content);

                        await _quranAyahPersistence.DeleteAllItemsAsync();
                        await _quranAyahPersistence.InsertAllItemAsync(data);

                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> GetMetaDataAsync(string metaLink)
        {
            try
            {
                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/{metaLink}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (metaLink == "getyoutubelink")
                        {
                            List<GeneralMetaData>? data = JsonSerializer.Deserialize<List<GeneralMetaData>>(content);

                            await _generalMetaPersistence.DeleteAllItemsByTypeAsync("youtube");
                            await _generalMetaPersistence.InsertAllItemAsync(data);
                        }

                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }


        public async Task<List<QuranSholatTime>?> GetSholatTimeByMonthAsync(string cityCode, string year, string month)
        {
            try
            {

                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/sholat/{cityCode}/{year}/{month}");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonSerializer.Deserialize<List<QuranSholatTime>>(content);
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> SyncSholatTimeByMonthAsync(string cityCode)
        {
            try
            {
                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    DateTime thisDate = DateTime.Now;
                    string thisMonth = thisDate.Month.ToString();
                    string thisYear = thisDate.Year.ToString();

                    DateTime startDate = DateTime.Parse($"{thisYear}-{thisMonth}-01");
                    string sMonth = startDate.Month.ToString();
                    string sYear = startDate.Year.ToString();

                    List<QuranSholatTime> allSholatTime = new List<QuranSholatTime>();
                    for (int i = 1; i <= 12; i++)
                    {
                        var response = await GetSholatTimeByMonthAsync(cityCode, sYear, sMonth);
                        if (response != null)
                        {
                            response.ForEach(x =>
                            {
                                allSholatTime.Add(x);
                            });
                        }

                        startDate = startDate.AddMonths(1).AddDays(-1);
                        sMonth = startDate.Month.ToString();
                        sYear = startDate.Year.ToString();
                    }

                    await _sholatTimesPersistence.DeleteAllItemsAsync();
                    await _sholatTimesPersistence.InsertAllItemAsync(allSholatTime);

                    return true;

                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<CityCodes>> SyncCityCodesAsync()
        {
            try
            {
                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/sholat/citycode");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        List<CityCodes>? data = JsonSerializer.Deserialize<List<CityCodes>>(content);

                        await _cityCodesPersistence.DeleteAllItemsAsync();
                        await _cityCodesPersistence.InsertAllItemAsync(data);
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task OnlineSyncQuran()
        {
            await GetQuranSurahAsync();
            await GetQuranAyahAsync();
            await GetMetaDataAsync("getyoutubelink");
        }

        public async Task<List<QuranAsmaulHusna>?> GetQuranAsmaulHusnaAsync()
        {
            try
            {
                if (_connectivity.NetworkAccess == NetworkAccess.Internet)
                {
                    var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/asmaulhusna");
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        return JsonSerializer.Deserialize<List<QuranAsmaulHusna>>(content);
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
