using MimApp.Utils;
using MimApp.Persistences.Contracts;
using SQLite;

namespace MimApp.Persistences
{
    public class QuranApiPersistence : IQuranApiPersistence
    {
        SQLiteAsyncConnection Database;

        public QuranApiPersistence()
        {

        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Helpers.DatabasePath, Helpers.Flags);
            var result = await Database.CreateTableAsync<QuranSurah>();
            if (result == CreateTableResult.Created)
            {
                Debug.WriteLine("-----------------------> Table QuranSurah Created");
            }
        }

        public async Task<bool> AddItemAsync(QuranSurah item)
        {
            try
            {
                await Init();
                await Database.InsertAsync(item);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<QuranSurah> GetOneItem(int numberOfVerse)
        {
            await Init();
            return await Database.Table<QuranSurah>().Where(i => i.NumberOfVerses == numberOfVerse).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<QuranSurah>> GetAllItems()
        {
            await Init();
            var results = new List<QuranSurah>();
            results = await Database.Table<QuranSurah>().ToListAsync();
            
            return results;
        }

        public async Task<bool> DeleteAllItemsAsync()
        {
            await Init();
            var result = await Database.DeleteAllAsync<QuranSurah>();
            if (result > 0) return true;
            return false;
        }
    }
}
