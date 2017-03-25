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
                listItems = getMockedDataForChatHistory() as List<T>;
            }
            if (type == typeof(Chat))
            {
                listItems = getMockedDataForChat() as List<T>;
            }

            return listItems;
        }

        private List<ChatHistory> getMockedDataForChatHistory()
        {
            List<ChatHistory> listObj = new List<ChatHistory>();
            //данные для проверки рандомной выборки
            int maxId = 0;
            for (int x = 1; x<=2000; x++)
            {
                fillTestItemsInChatHistory(listObj, ref maxId, 1, 1, 2);
            }

            //данные для проверки порядка выборки
            fillTestItemsInChatHistory(listObj, ref maxId, 2, 3, 1);

            //данные для проверки выборки сообщений одного направления
            for (int x = 1; x <= 20; x++)
            {
                fillTestItemsInChatHistory(listObj, ref maxId, 3, 1, 4);
            }
            for (int x = 1; x <= 20; x++)
            {
                fillTestItemsInChatHistory(listObj, ref maxId, 3, 4, 1);
            }
            return listObj;
        }

        private static int fillTestItemsInChatHistory(List<ChatHistory> objectsList, ref int maxId, int chatId, int LanguageFrom, int LanguageTo)
        {
            objectsList.Add(new ChatHistory() { ID = maxId, ChatID = chatId, InFavorites = false, LanguageFrom = LanguageFrom, LanguageTo = LanguageTo, TextFrom = "test" + maxId.ToString(), TextTo = string.Empty, UpdateDate = DateTime.Now });
            maxId++;
            objectsList.Add(new ChatHistory() { ParentRequestID = maxId - 1, ID = maxId, ChatID = chatId, InFavorites = true, LanguageFrom = LanguageFrom, LanguageTo = LanguageTo, TextFrom = string.Empty, TextTo = "test" + maxId.ToString(), UpdateDate = DateTime.Now });
            maxId++;
            return maxId;
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
            listObj.Add(new Chat() { ID = 3, UpdateDate = DateTime.Now.Add(new TimeSpan(-10, 0, 0, 0)), LanguageFrom = 1, LanguageTo = 4 });//ru->sp
            return listObj;
        }
    }
}
