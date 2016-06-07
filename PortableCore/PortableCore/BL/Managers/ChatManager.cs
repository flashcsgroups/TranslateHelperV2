using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Contracts;
using PortableCore.BL.Models;
using PortableCore.DAL;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public class ChatManager : IInitDataTable<Chat>, IChatManager
    {
        ISQLiteTesting db;
        ILanguageManager languageManager;
        IChatHistoryManager chatHistoryManager;

        public ChatManager(ISQLiteTesting dbHelper, ILanguageManager languageManager, IChatHistoryManager chatHistoryManager)
        {
            this.db = dbHelper;
            this.languageManager = languageManager;
            this.chatHistoryManager = chatHistoryManager;
        }

        public void InitDefaultData()
        {
        }

        public Chat GetItemForId(int id)
        {
            Repository<Chat> repos = new Repository<Chat>();
            return repos.GetItem(id);
        }

        public int SaveItem(Chat item)
        {
            Repository<Chat> repo = new Repository<Chat>();
            return repo.Save(item);
        }

        public Chat GetChatByLanguage(Language userLanguage, Language robotLanguage)
        {
            return db.Table<Chat>().SingleOrDefault(item=>item.LanguageFrom == userLanguage.ID && item.LanguageTo == robotLanguage.ID);
        }

        public List<DirectionsRecentItem> GetChatsForLastDays(int countOfDays)
        {
            var lstLanguages = languageManager.GetDefaultData();
            var lst = db.Table<Chat>()
                .Where(item => item.UpdateDate >= DateTime.Now.Add(new TimeSpan(-countOfDays, 0, 0, 0)))
                .Select(t => new DirectionsRecentItem() { ChatId = t.ID, LangFrom = t.LanguageCaptionFrom, LangTo = t.LanguageCaptionTo, LangToFlagImageResourcePath = lstLanguages.Where(i => i.ID == t.LanguageTo).SingleOrDefault() != null ? lstLanguages.Where(i => i.ID == t.LanguageTo).SingleOrDefault().NameImageResource : string.Empty })
                .ToList();
            //ChatHistoryManager chatHistoryManager = new ChatHistoryManager(db);
            //ToDo:Убрать запрос из цикла
            foreach (var item in lst)
            {
                item.CountOfAllMessages = chatHistoryManager.GetCountOfMessagesForChat(item.ChatId);
            }
            return lst.Where(t=>t.CountOfAllMessages > 0).ToList();
        }
    }
}
