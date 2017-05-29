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
        //public string FileAnecdotesContent;


        public AnecdoteManager(ISQLiteTesting dbHelper)
        {
            this.db = dbHelper;
            //this.languageManager = languageManager;
        }

        public bool LoadDataFromString(string anecdotes)
        {
            var anecdotesArray = anecdotes.Split('^');
            List<Anecdote> data = new List<Anecdote>();
            foreach(var item in anecdotesArray)
            {
                var arrTranslated = item.Trim().Split('#');
                if (arrTranslated.Length == 2)
                {
                    data.Add(new Anecdote() { LanguageFrom = 2, LanguageTo = 1, TextFrom = arrTranslated[0].TrimEnd(), TextTo = arrTranslated[1] });
                }
            }
            DAL.Repository<Anecdote> repos = new DAL.Repository<Anecdote>();
            repos.DeleteAllDataInTable();
            repos.AddItemsInTransaction(data);
            return true;
        }
        public void InitDefaultData()
        {
            //DAL.Repository<Anecdote> repos = new DAL.Repository<Anecdote>();
            //AnecdoteUpdater updater = new AnecdoteUpdater();
            //Anecdote[] data = updater.getArrayFromResource(LibraryPath);
            /*Anecdote[] data = getDefaultData();
            if (!db.Table<Anecdote>().Any())
            {
                repos.DeleteAllDataInTable();
                repos.AddItemsInTransaction(data);
            }*/
        }

        public Anecdote GetItemForId(int id)
        {
            Repository<Anecdote> repos = new Repository<Anecdote>();
            return repos.GetItem(id);
        }

        internal IndexedCollection<AnecdoteItem> GetAllAnecdotesForChat(int selectedChatID)
        {
            DAL.Repository<Chat> repos = new DAL.Repository<Chat>();
            Chat chat = repos.GetItem(selectedChatID);
            var viewAnecdotes = db.Table<Anecdote>().Where(item => (item.LanguageFrom == chat.LanguageFrom && item.LanguageTo == chat.LanguageTo && item.DeleteMark == 0));
            IndexedCollection<AnecdoteItem> indexedAnecdotes = new IndexedCollection<AnecdoteItem>();
            foreach(var item in viewAnecdotes)
            {
                indexedAnecdotes.Add(new AnecdoteItem() { AnecdoteId = item.ID, OriginalText = item.TextFrom, TranslatedText = item.TextTo, ReadMark = item.ReadMark});
            }
            return indexedAnecdotes;
        }
        /*internal object GetUnreadedAnecdotesForChat(int selectedChatID)
        {
            DAL.Repository<Chat> repos = new DAL.Repository<Chat>();
            Chat chat = repos.GetItem(selectedChatID);
            var view = db.Table<Anecdote>().Where(item => (item.LanguageFrom == chat.LanguageFrom && item.LanguageTo == chat.LanguageTo && item.DeleteMark == 0));
        }*/

        public int SaveItem(Anecdote item)
        {
            Repository<Anecdote> repo = new Repository<Anecdote>();
            return repo.Save(item);
        }

        private Anecdote[] getDefaultData()
        {
            //http://www.anekdot.ws/archives/category/russian-jokes/page/2
            Anecdote[] defTypesList = new Anecdote[] {
                new Anecdote (){ LanguageFrom = 2, LanguageTo = 1, TextFrom = "Scientists have discovered Soviet Lunokhod-2 on the moon. In the moon-buggy the wheels were twisted off and the cassette rack was pulled out.", TextTo = "Ученые обнаружили на Луне советский «Луноход-2». У лунохода были скручены колеса и вытащена магнитола." },
                new Anecdote (){ LanguageFrom = 2, LanguageTo = 1, TextFrom = "If Russian decided to do nothing, nothing can stop him.", TextTo = "Если русский человек решил ничего не делать, то его не остановить." },
                new Anecdote (){ LanguageFrom = 2, LanguageTo = 1, TextFrom = "Only Russian comes back to work from sick leave with strong tan.", TextTo = "Только русский человек может прийти с больничного загорелым." },
                new Anecdote (){ LanguageFrom = 2, LanguageTo = 1, TextFrom = "Only Russian comes back to work from sick leave with strong tan. Second", TextTo = "Только русский человек может прийти с больничного загорелым." },
                new Anecdote (){ LanguageFrom = 2, LanguageTo = 1, TextFrom = @"Russians invented electric light bulbs.
Edison was the first who invented that these bulbs can be sold.*
* Russians believe that first electric light bulb was invented by Russian engineer Alexander Lodygin in 1874. Edison patented his invention in 1880. By the way Russians think that radio was invented by … who do you think … Russian engineer Alexander Popov in 1895.", TextTo = @"Русские первые придумали лампу накаливания.
Эдисон первый, кто придумал, что эти лампы можно продавать." },
                new Anecdote (){ LanguageFrom = 1, LanguageTo = 2, TextFrom = "Если русский человек решил ничего не делать, то его не остановить.", TextTo = "If Russian decided to do nothing, nothing can stop him." },
                new Anecdote (){ LanguageFrom = 1, LanguageTo = 2, TextFrom = "Только русский человек может прийти с больничного загорелым.", TextTo = "Only Russian comes back to work from sick leave with strong tan." },
            };
            return defTypesList;
        }
    }
}
