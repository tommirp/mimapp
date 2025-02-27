using MimApp.Utils;
using MimApp.Persistences.Contracts;
using SQLite;

namespace MimApp.Persistences
{
    public class AsmaulHusnaPersistence : IAsmaulHusnaPersistence
    {
        SQLiteAsyncConnection Database;

        public AsmaulHusnaPersistence()
        {

        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Helpers.DatabasePath, Helpers.Flags);
            var result = await Database.CreateTableAsync<QuranAsmaulHusna>();
            if (result == CreateTableResult.Created)
            {
                Debug.WriteLine("-----------------------> Table QuranAsmaulHusna Created");
            }
        }

        public async Task<bool> InsertAllItemAsync(List<QuranAsmaulHusna>? items)
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

        public async Task<bool> AsmaulHusnaCheck()
        {
            try
            {
                await Init();
                var count = await Database.Table<QuranAsmaulHusna>().CountAsync();

                if (count > 0)
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;

            }
        }

        public async Task<List<QuranAsmaulHusna>> GetAllQuranAsmaulHusna()
        {
            await Init();
            var results = new List<QuranAsmaulHusna>();
            results = await Database.Table<QuranAsmaulHusna>().ToListAsync();
            
            return results;
        }

        public async Task<bool> DeleteAllItemsAsync()
        {
            await Init();
            var result = await Database.DeleteAllAsync<QuranAsmaulHusna>();
            if (result > 0) return true;
            return false;
        }
    }
}
