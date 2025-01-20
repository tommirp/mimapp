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
        //private UserWithToken? userWithToken;

        public QuranApiService(HttpClient httpClient, IConnectivity connectivity, IPreferences preferences,
            IQuranSurahPersistence quranSurahPersistence, IQuranAyahPersistence quranAyahPersistence, ISholatTimesPersistence sholatTimesPersistence)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Helpers.BaseApiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            int timeOut = int.Parse(preferences.Get("TimeOut", "20"));
            _httpClient.Timeout = TimeSpan.FromSeconds(timeOut);

            _connectivity = connectivity;
            _quranSurahPersistence = quranSurahPersistence;
            _quranAyahPersistence = quranAyahPersistence;
            _sholatTimesPersistence = sholatTimesPersistence;
        }

        private async Task<bool> GetQuranSurahAsync()
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

        private async Task<bool> GetQuranAyahAsync()
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

        public async Task<List<QuranSholatTime>?> GetSholatTimeByMonthAsync(string cityCode, string year, string month)
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

        public async Task<bool> SyncSholatTimeByMonthAsync(string cityCode)
        {
            if (_connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                DateTime thisDate = DateTime.Now;
                string thisMonth = thisDate.Month.ToString("MM");
                string thisYear = thisDate.Month.ToString("yyyy");

                DateTime startDate = DateTime.Parse($"{thisYear}-{thisMonth}-01");
                string sMonth = startDate.Month.ToString("MM");
                string sYear = startDate.Month.ToString("yyyy");

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
                    sMonth = startDate.Month.ToString("MM");
                    sYear = startDate.Month.ToString("yyyy");
                }

                await _sholatTimesPersistence.DeleteAllItemsAsync();
                await _sholatTimesPersistence.InsertAllItemAsync(allSholatTime);

                return true;

            }

            return false;
        }

        public async Task<List<CityCodes>?> SyncCityCodesAsync()
        {
            if (_connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/sholat/citycode");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<List<CityCodes>>(content);
                }
            }

            return null;
        }

        public async Task OnlineSyncQuran()
        {
            await GetQuranSurahAsync();
            await GetQuranAyahAsync();
        }

        public async Task<List<QuranAsmaulHusna>?> GetQuranAsmaulHusnaAsync()
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
    }
}
