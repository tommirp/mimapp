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
                var count = await Database.Table<QuranSurah>().CountAsync();

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

        public async Task<List<string>> GetSurahNameByKeyword(string keyword = null)
        {
            await Init();
            var finalResult = new List<string>();
            List<QuranSurah> results = await Database.Table<QuranSurah>().ToListAsync();
            List<QuranSurah> newResults = new List<QuranSurah>();

            var isNumeric = int.TryParse(keyword, out int n);
            if (isNumeric)
            {
                newResults = results.Where(data => data.number == n).ToList();
            }
            else
            {
                newResults = results.OrderBy(data => StringSimilarity.Levenshtein(data.nameTransliterationId, keyword)).Take(10).ToList();
            }

            newResults.ForEach(x =>
            {
                string surah = string.Format("{0} - {1}", x.number, x.nameTransliterationId);
                finalResult.Add(surah);
            });

            return finalResult;
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
