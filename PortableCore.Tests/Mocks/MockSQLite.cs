using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using PortableCore.DL;

namespace PortableCore.Tests.Mocks
{
    public class MockSQLite : ISQLiteTesting
    {
        public IEnumerable<T> Table<T>() where T : IBusinessEntity, new()
        {
            List<T> listItems = new List<T>();
            var type = typeof(T);
            if (type == typeof(Language))
            {
                listItems = getMockedDataForLanguage() as List<T>;
            }
            if (type == typeof(ChatHistory))
            {
                listItems = getMockedDataForLastChats() as List<T>;
            }
            if (type == typeof(Chat))
            {
                listItems = getMockedDataForChat() as List<T>;
            }
            if (type == typeof(Direction))
            {
                List<Direction> listFav = new List<Direction>();
                listFav.Add(new Direction() { ID = 1 });
                listItems = listFav as List<T>;
            }

            return listItems;
        }

        private List<ChatHistory> getMockedDataForLastChats()
        {
            List<ChatHistory> listObj = new List<ChatHistory>();
            listObj.Add(new ChatHistory() { ID = 1, ChatID = 1 });
            listObj.Add(new ChatHistory() { ID = 2, ChatID = 2 });
            listObj.Add(new ChatHistory() { ID = 3, ChatID = 3 });
            return listObj;
        }

        private List<Language> getMockedDataForLanguage()
        {
            LanguageManager langMan = new LanguageManager(this);
            List<Language> listObj = langMan.GetDefaultData().ToList();
            return listObj;
        }

        private List<Chat> getMockedDataForChat()
        {
            List<Chat> listObj = new List<Chat>();
            listObj.Add(new Chat() { ID = 1, UpdateDate = DateTime.Now.Add(new TimeSpan(-1, 0, 0, 0)), LanguageFrom = 1, LanguageTo = 2 });
            listObj.Add(new Chat() { ID = 2, UpdateDate = DateTime.Now.Add(new TimeSpan(-1, 0, 0, 0)), LanguageFrom = 2, LanguageTo = 3 });
            return listObj;
        }
    }
}
