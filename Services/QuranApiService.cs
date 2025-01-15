using MimApp.Persistences.Contracts;
using MimApp.Services.Contracts;
using MimApp.Utils;

namespace MimApp.Services
{
    public class QuranApiService : IQuranApi
    {
        private readonly HttpClient _httpClient;
        private readonly IConnectivity _connectivity;
        private readonly IPreferences _preferences;
        private readonly IQuranApiPersistence _quranApiPersistence;
        private UserWithToken userWithToken;

        public QuranApiService(HttpClient httpClient, IConnectivity connectivity, IPreferences preferences,
            IQuranApiPersistence quranApiPersistence)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(Helpers.BaseApiUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            int timeOut = int.Parse(preferences.Get("TimeOut", "10"));
            _httpClient.Timeout = TimeSpan.FromSeconds(timeOut);

            _connectivity = connectivity;
            _preferences = preferences;
            _quranApiPersistence = quranApiPersistence;
        }

        public async Task<IEnumerable<QuranSurah>> GetQuranSurahAsync()
        {
            if (_connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                userWithToken = JsonSerializer.Deserialize<UserWithToken>(_preferences.Get("UserWithToken", string.Empty));
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userWithToken?.jwtToken);

                var response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/surah");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<IEnumerable<QuranSurah>>(content);
                }
            }

            return null;
        }


    }
}
