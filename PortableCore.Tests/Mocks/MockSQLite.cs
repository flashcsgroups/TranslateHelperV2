using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using PortableCore.DL;
using Ploeh.AutoFixture;

namespace PortableCore.Tests.Mocks
{
    public class MockSQLite : ISQLiteTesting
    {
        public List<ChatHistory> ItemsChatHistory;

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
                listItems = ItemsChatHistory == null? getMockedDataForChatHistory() as List<T>:ItemsChatHistory as List<T>;
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

            //данные для проверки выборки сообщений одного направления, специально с дублями чтобы убедиться в тесте что они отбрасываются
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

    abstract class ChatHistoryBuilder
    {
        public ChatHistoryObject ChatHistoryInstance { get; private set; }
        internal ChatHistoryBuilder CreateChatHistory()
        {
            ChatHistoryInstance = new ChatHistoryObject();
            return this;
        }
        internal List<ChatHistory> AddItems(int startFromId)
        {
            Fixture fixture = new Fixture();
            if (ChatHistoryInstance.ChatHistoryList == null)
                ChatHistoryInstance.ChatHistoryList = new List<ChatHistory>();
            int maxId = startFromId;
            for(int i=0;i<ChatHistoryInstance.CountOfFixture;i++)
            {
                string textfrom = ChatHistoryInstance.TextFrom;
                if (string.IsNullOrEmpty(textfrom))
                {
                    textfrom = fixture.Create<string>();
                }
                string textto = ChatHistoryInstance.TextTo;
                if(string.IsNullOrEmpty(textto))
                {
                    textto = fixture.Create<string>();
                }
                ChatHistoryInstance.ChatHistoryList.Add(new ChatHistory() { ID = maxId, ChatID = ChatHistoryInstance.ChatId, InFavorites = false, LanguageFrom = ChatHistoryInstance.LanguageIdFrom, LanguageTo = ChatHistoryInstance.LanguageIdTo, ParentRequestID = 0, TextFrom = textfrom, TextTo = string.Empty, DeleteMark = 0, Transcription = string.Empty, UpdateDate = DateTime.Now});
                maxId++;
                ChatHistoryInstance.ChatHistoryList.Add(new ChatHistory() { ID = maxId, ChatID = ChatHistoryInstance.ChatId, InFavorites = ChatHistoryInstance.FavoriteState, LanguageFrom = ChatHistoryInstance.LanguageIdFrom, LanguageTo = ChatHistoryInstance.LanguageIdTo, ParentRequestID = maxId - 1, TextFrom = string.Empty, TextTo = textto, DeleteMark = 0, Transcription = string.Empty, UpdateDate = DateTime.Now });
                maxId++;
            }
            return ChatHistoryInstance.ChatHistoryList;
        }
        public abstract ChatHistoryBuilder SetLanguageFrom(int id);
        public abstract ChatHistoryBuilder SetLanguageTo(int id);
        public abstract ChatHistoryBuilder SetChatId(int id);
        public abstract ChatHistoryBuilder SetParentRequestId(int id);
        public abstract ChatHistoryBuilder SetTextFrom(string textFrom);
        public abstract ChatHistoryBuilder SetTextTo(string textTo);
        public abstract ChatHistoryBuilder SetFavoriteState(bool favorite);
        public abstract ChatHistoryBuilder SetCount(int countOfFixture);

    }

    internal class ConcreteChatHistoryBuilder : ChatHistoryBuilder
    {
        public override ChatHistoryBuilder SetChatId(int id)
        {
            this.ChatHistoryInstance.ChatId = id;
            return this;
        }

        public override ChatHistoryBuilder SetCount(int countOfFixture)
        {
            this.ChatHistoryInstance.CountOfFixture = countOfFixture;
            return this;
        }

        public override ChatHistoryBuilder SetFavoriteState(bool favorite)
        {
            this.ChatHistoryInstance.FavoriteState = favorite;
            return this;
        }

        public override ChatHistoryBuilder SetLanguageFrom(int id)
        {
            this.ChatHistoryInstance.LanguageIdFrom = id;
            return this;
        }

        public override ChatHistoryBuilder SetLanguageTo(int id)
        {
            this.ChatHistoryInstance.LanguageIdTo = id;
            return this;
        }

        public override ChatHistoryBuilder SetParentRequestId(int id)
        {
            //this.ChatHistoryInstance.ParentRequestId = id;
            return this;
        }

        public override ChatHistoryBuilder SetTextFrom(string textFrom)
        {
            this.ChatHistoryInstance.TextFrom = textFrom;
            return this;
        }

        public override ChatHistoryBuilder SetTextTo(string textTo)
        {
            this.ChatHistoryInstance.TextTo = textTo;
            return this;
        }
    }
}
