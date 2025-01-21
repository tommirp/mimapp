using MimApp.Utils;
using MimApp.Persistences.Contracts;
using SQLite;

namespace MimApp.Persistences
{
    public class QuranSurahPersistence : IQuranSurahPersistence
    {
        SQLiteAsyncConnection Database;

        public QuranSurahPersistence()
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

        public async Task<bool> InsertAllItemAsync(List<QuranSurah>? items)
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

        public async Task<bool> SurahCheck()
        {
            try
            {
                await Init();
                var results = new List<QuranSurah>();
                results = await Database.Table<QuranSurah>().ToListAsync();

                if (results.Count > 0)
                {
                    return true;
                }

                return false;
            }
            catch(Exception ex)
            {
                return false;

            }
        }

        public async Task<QuranSurah> GetOneSurah(int numberOfSurah)
        {
            await Init();
            return await Database.Table<QuranSurah>().Where(i => i.number == numberOfSurah).FirstOrDefaultAsync();
        }

        public async Task<List<QuranSurah>> GetAllSurah()
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
