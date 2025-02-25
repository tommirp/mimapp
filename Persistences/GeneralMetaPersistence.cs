using MimApp.Utils;
using MimApp.Persistences.Contracts;
using SQLite;

namespace MimApp.Persistences
{
    public class GeneralMetaPersistence : IGeneralMetaPersistence
    {
        SQLiteAsyncConnection Database;

        public GeneralMetaPersistence()
        {

        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Helpers.DatabasePath, Helpers.Flags);
            var result = await Database.CreateTableAsync<GeneralMetaData>();
            if (result == CreateTableResult.Created)
            {
                Debug.WriteLine("-----------------------> Table GeneralMetaData Created");
            }
        }

        public async Task<bool> InsertAllItemAsync(List<GeneralMetaData>? items)
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

        public async Task<List<GeneralMetaData>> GetAllGeneralMetaData(int limit = 50)
        {
            await Init();
            var results = new List<GeneralMetaData>();
            results = await Database.Table<GeneralMetaData>().Take(limit).ToListAsync();
            
            return results;
        }

        public async Task<List<GeneralMetaData>> GetMetaByName(string name)
        {
            await Init();
            var results = new List<GeneralMetaData>();
            results = await Database.Table<GeneralMetaData>().Where(x => x.metaName.ToUpper().Contains(name.ToUpper())).ToListAsync();

            return results;
        }

        public async Task<List<GeneralMetaData>> GetMetaByType(string type)
        {
            await Init();
            var results = new List<GeneralMetaData>();
            results = await Database.Table<GeneralMetaData>().Where(x => x.metaType.ToUpper().Contains(type.ToUpper())).ToListAsync();

            return results;
        }

        public async Task<bool> DeleteAllItemsByTypeAsync(string type)
        {
            await Init();
            var result = await Database.Table<GeneralMetaData>().DeleteAsync(x => x.metaType == type);
            if (result > 0) return true;
            return false;
        }

        public async Task<bool> DeleteAllItemsAsync()
        {
            await Init();
            var result = await Database.DeleteAllAsync<GeneralMetaData>();
            if (result > 0) return true;
            return false;
        }
    }
}
