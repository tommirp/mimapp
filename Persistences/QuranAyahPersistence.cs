using MimApp.Utils;
using MimApp.Persistences.Contracts;
using SQLite;

namespace MimApp.Persistences
{
    public class QuranAyahPersistence : IQuranAyahPersistence
    {
        SQLiteAsyncConnection Database;

        public QuranAyahPersistence()
        {

        }

        async Task Init()
        {
            if (Database is not null)
                return;

            Database = new SQLiteAsyncConnection(Helpers.DatabasePath, Helpers.Flags);
            var result = await Database.CreateTableAsync<QuranAyah>();
            if (result == CreateTableResult.Created)
            {
                Debug.WriteLine("-----------------------> Table QuranAyah Created");
            }
        }

        public async Task<bool> InsertAllItemAsync(List<QuranAyah>? items)
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
        public async Task<QuranAyah> GetOneAyah(int numberOfSurah, int numberOfVerse)
        {
            await Init();
            return await Database.Table<QuranAyah>().Where(i => i.NumberOfSurah == numberOfSurah && i.NumberInSurah == numberOfVerse).FirstOrDefaultAsync();
        }

        public async Task<List<QuranAyah>> GetAyahBySurahAsync(int numberOfSurah)
        {
            await Init();
            return await Database.Table<QuranAyah>().Where(i => i.NumberOfSurah == numberOfSurah).ToListAsync();
        }

        public async Task<List<QuranAyah>> GetAyahByJuzAsync(int Juz)
        {
            await Init();
            return await Database.Table<QuranAyah>().Where(i => i.Juz == Juz).ToListAsync();
        }

        public async Task<bool> DeleteAllItemsAsync()
        {
            await Init();
            var result = await Database.DeleteAllAsync<QuranAyah>();
            if (result > 0) return true;
            return false;
        }
    }
}
