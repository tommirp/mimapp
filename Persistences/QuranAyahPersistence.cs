using MimApp.Utils;
using MimApp.Persistences.Contracts;
using SQLite;
using static SQLite.SQLite3;

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
            return await Database.Table<QuranAyah>().Where(i => i.numberOfSurah == numberOfSurah && i.numberInSurah == numberOfVerse).FirstOrDefaultAsync();
        }

        public async Task<List<QuranAyah>> GetAyahBySurahAsync(int numberOfSurah)
        {
            await Init();
            return await Database.Table<QuranAyah>().Where(i => i.numberOfSurah == numberOfSurah).ToListAsync();
        }

        public async Task<List<QuranAyah>> GetAyahByJuzAsync(int Juz)
        {
            await Init();
            return await Database.Table<QuranAyah>().Where(i => i.juz == Juz).ToListAsync();
        }

        public async Task<List<string>> GetAyahByKeyword(string keyword)
        {
            await Init();

            var finalResult = new List<string>();
            List<QuranAyah> results = await Database.Table<QuranAyah>().ToListAsync();
            List<QuranAyah> newResults = new List<QuranAyah>();

            //newResults = results.OrderBy(data => StringSimilarity.Levenshtein(data.translationId, keyword)).Take(20).ToList();
            newResults = results.Where(x => x.translationId.ToLower().Contains(keyword.ToLower())).Take(30).ToList();

            newResults.ForEach(x =>
            {
                string ayah = string.Format("{0}:{1}. {2}", x.numberOfSurah, x.numberInSurah, x.translationId);
                finalResult.Add(ayah);
            });

            return finalResult;
        }

        public async Task<List<string>> GetAllAyahNumberListBySurah(string surah)
        {
            await Init();
            var finalResult = new List<string>();
            List<QuranAyah> results = await Database.Table<QuranAyah>().ToListAsync();

            List<QuranAyah> new_result = results.Where(x => x.numberOfSurah.ToString() == surah).ToList();

            new_result.ForEach(x =>
            {
                finalResult.Add(x.numberInSurah.ToString());
            });

            return finalResult;
        }

        public async Task<bool> AyahCheck()
        {
            try
            {
                await Init();
                var count = await Database.Table<QuranAyah>().CountAsync();

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

        public async Task<bool> DeleteAllItemsAsync()
        {
            await Init();
            var result = await Database.DeleteAllAsync<QuranAyah>();
            if (result > 0) return true;
            return false;
        }
    }
}
