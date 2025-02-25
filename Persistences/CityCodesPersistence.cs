using MimApp.Utils;
using MimApp.Persistences.Contracts;
using SQLite;

namespace MimApp.Persistences
{
    public class CityCodesPersistence : ICityCodesPersistence
    {
        SQLiteAsyncConnection Database;

        public CityCodesPersistence()
        {

        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Helpers.DatabasePath, Helpers.Flags);
            var result = await Database.CreateTableAsync<CityCodes>();
            if (result == CreateTableResult.Created)
            {
                Debug.WriteLine("-----------------------> Table CityCodes Created");
            }
        }

        public async Task<bool> InsertAllItemAsync(List<CityCodes>? items)
        {
            try
            {
                await Init();
                await Database.InsertAllAsync(items);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<CityCodes>> GetAllCityCodes(int limit = 50)
        {
            await Init();
            var results = new List<CityCodes>();
            results = await Database.Table<CityCodes>().Take(limit).ToListAsync();
            
            return results;
        }

        public async Task<List<CityCodes>> GetCityByName(string name)
        {
            await Init();
            var results = new List<CityCodes>();
            results = await Database.Table<CityCodes>().Where(x => x.lokasi.ToUpper().Contains(name.ToUpper())).ToListAsync();

            return results;
        }

        public async Task<bool> DeleteAllItemsAsync()
        {
            await Init();
            var result = await Database.DeleteAllAsync<CityCodes>();
            if (result > 0) return true;
            return false;
        }
    }
}
