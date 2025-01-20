using MimApp.Persistences.Contracts;
using MimApp.Utils;
using SQLite;

namespace MimApp.Persistences
{
    public class SholatTimesPersistence : ISholatTimesPersistence
    {
        SQLiteAsyncConnection Database;

        public SholatTimesPersistence()
        {

        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Helpers.DatabasePath, Helpers.Flags);
            var result = await Database.CreateTableAsync<QuranSholatTime>();
            if (result == CreateTableResult.Created)
            {
                Debug.WriteLine("-----------------------> Table QuranSholatTime Created");
            }
        }

        public async Task<bool> InsertAllItemAsync(List<QuranSholatTime>? items)
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

        public async Task<QuranSholatTime> GetSholatTimeByDate(string Date)
        {
            await Init();
            return await Database.Table<QuranSholatTime>().Where(x => x.Date == Date).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAllItemsAsync()
        {
            await Init();
            var result = await Database.DeleteAllAsync<QuranSholatTime>();
            if (result > 0) return true;
            return false;
        }
    }
}
