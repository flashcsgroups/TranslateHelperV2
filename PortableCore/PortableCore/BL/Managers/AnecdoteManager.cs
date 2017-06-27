using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Contracts;
using PortableCore.BL.Models;
using PortableCore.DAL;
using PortableCore.DL;
using System.IO;

namespace PortableCore.BL.Managers
{
    public class AnecdoteManager : IInitDataTable<Anecdote>, IAnecdoteManager
    {
        readonly ISQLiteTesting db;
        private ILanguageManager languageManager;
        //public string FileAnecdotesContent;


        public AnecdoteManager(ISQLiteTesting dbHelper, ILanguageManager languageManager)
        {
            this.db = dbHelper;
            this.languageManager = languageManager;
        }

        public void ClearAnecdoteTable()
        {
            DAL.Repository<Anecdote> repos = new DAL.Repository<Anecdote>();
            repos.DeleteAllDataInTable();
        }

        public void LoadDataFromString(DirectionAnecdoteItem storyInfo, string anecdotes)
        {
            var anecdotesArray = anecdotes.Split('^');
            List<Anecdote> data = new List<Anecdote>();
            foreach(var item in anecdotesArray)
            {
                var arrTranslated = item.Trim().Split('#');
                if (arrTranslated.Length == 2)
                {
                    data.Add(new Anecdote() { LanguageFrom = storyInfo.LanguageFrom.ID, LanguageTo = storyInfo.LanguageTo.ID, TextFrom = arrTranslated[0].TrimEnd(), TextTo = arrTranslated[1] });
                }
            }
            DAL.Repository<Anecdote> repos = new DAL.Repository<Anecdote>();
            repos.AddItemsInTransaction(data);
        }
        public void InitDefaultData()
        {
        }

        public Anecdote GetItemForId(int id)
        {
            Repository<Anecdote> repos = new Repository<Anecdote>();
            return repos.GetItem(id);
        }

        internal IndexedCollection<AnecdoteItem> GetAllAnecdotesForDirections(int languageFromId, int languageToId)
        {
            var viewAnecdotes = db.Table<Anecdote>().Where(item => (((item.LanguageFrom == languageToId && item.LanguageTo == languageFromId)||(item.LanguageFrom == languageFromId && item.LanguageTo == languageToId)) && item.DeleteMark == 0));
            IndexedCollection<AnecdoteItem> indexedAnecdotes = new IndexedCollection<AnecdoteItem>();
            foreach(var item in viewAnecdotes)
            {
                indexedAnecdotes.Add(new AnecdoteItem() { AnecdoteId = item.ID, OriginalText = item.TextFrom, TranslatedText = item.TextTo, ReadMark = item.ReadMark});
            }
            return indexedAnecdotes;
        }

        public List<DirectionAnecdoteItem> GetListDirectionsForStories()
        {
            List<DirectionAnecdoteItem> list = new List<DirectionAnecdoteItem>();
            var eng = languageManager.GetItemForShortName("en");
            var de = languageManager.GetItemForShortName("de");
            var es = languageManager.GetItemForShortName("es");
            var rus = languageManager.GetItemForShortName("ru");
            list.Add(new DirectionAnecdoteItem(eng, rus, "anecdotesEN_RUv1.txt"));
            list.Add(new DirectionAnecdoteItem(de, rus, "anecdotesDE_RUv1.txt"));
            list.Add(new DirectionAnecdoteItem(es, rus, "anecdotesES_RUv1.txt"));
            return list;
        }

        public int SaveItem(Anecdote item)
        {
            Repository<Anecdote> repo = new Repository<Anecdote>();
            return repo.Save(item);
        }
    }
}
