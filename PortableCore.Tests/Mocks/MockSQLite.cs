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
            int maxId = 2000;
            for (int x = 1; x<=maxId; x++)
            {
                listObj.Add(new ChatHistory() { ID = x, ChatID = 1, InFavorites = true, LanguageFrom = 1, LanguageTo = 2, TextFrom = "test" + x.ToString(), TextTo = String.Format("тест{0}, проверка{1}, тестирование{2}, экзамен{3}", x,x,x,x) });
            }

            //данные для проверки порядка выборки
            maxId++;
            listObj.Add(new ChatHistory() { ID = maxId, ChatID = 2, InFavorites = true, LanguageFrom = 3, LanguageTo = 1, TextFrom = "testFrance" + maxId.ToString(), TextTo = String.Format("тестFrance{0}, проверкаFrance{1}, тестированиеFrance{2}, экзаменFrance{3}", maxId, maxId, maxId, maxId) });

            //данные для проверки выборки сообщений одного направления
            maxId++;
            listObj.Add(new ChatHistory() { ID = maxId, ChatID = 3, InFavorites = true, LanguageFrom = 1, LanguageTo = 4, TextFrom = "testRu" + maxId.ToString(), TextTo = "TestSp" });
            maxId++;
            listObj.Add(new ChatHistory() { ID = maxId, ChatID = 3, InFavorites = true, LanguageFrom = 1, LanguageTo = 4, TextFrom = "testRu" + maxId.ToString(), TextTo = "TestSp" });
            maxId++;
            listObj.Add(new ChatHistory() { ID = maxId, ChatID = 3, InFavorites = true, LanguageFrom = 4, LanguageTo = 1, TextFrom = "testSp" + maxId.ToString(), TextTo = "TestRu" });

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
            listObj.Add(new Chat() { ID = 3, UpdateDate = DateTime.Now.Add(new TimeSpan(-10, 0, 0, 0)), LanguageFrom = 1, LanguageTo = 4 });//ru->sp
            return listObj;
        }
    }
}
