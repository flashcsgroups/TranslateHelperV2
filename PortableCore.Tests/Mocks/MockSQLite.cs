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
            if (type == typeof(Idiom))
            {
                listItems = getMockedDataForIdiom() as List<T>;
            }
            if (type == typeof(IdiomCategory))
            {
                listItems = getMockedDataForIdiomCategory() as List<T>;
            }

            return listItems;
        }

        private List<IdiomCategory> getMockedDataForIdiomCategory()
        {
            var eng = 2;
            var rus = 1;
            List<IdiomCategory> list = new List<IdiomCategory>
            {
                new IdiomCategory() { ID = 1, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Frequently used phrases", TextTo = "Часто используемые фразы" },
                new IdiomCategory() { ID = 2, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Time", TextTo = "Время" },
                new IdiomCategory() { ID = 3, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Count", TextTo = "Наличие" },
                new IdiomCategory() { ID = 4, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Count", TextTo = "Количество" },
                new IdiomCategory() { ID = 5, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Information", TextTo = "Информация" },
                new IdiomCategory() { ID = 6, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Intellect, mind", TextTo = "Интеллект, мышление" }
            };

            return list;
        }

        private List<Idiom> getMockedDataForIdiom()
        {
            List<Idiom> listObj = new List<Idiom>();
            var eng = 2;
            var rus = 1;
            listObj.Add(new Idiom() { ID = 1, UpdateDate = DateTime.Parse("2017-10-13"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 1, DescriptionTextFrom = "Common phrases", DescriptionTextTo = "Общие фразы", TextFrom = "all in all", TextTo = "в конечном счёте, с учётом всех обстоятельств / в целом", ExampleTextFrom = string.Empty, ExampleTextTo = string.Empty });
            listObj.Add(new Idiom (){ ID=2, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 1, DescriptionTextFrom = "Common phrases", DescriptionTextTo = "Общие фразы", TextFrom = "all the way", TextTo = "от начала до конца", ExampleTextFrom = string.Empty, ExampleTextTo = string.Empty });
            listObj.Add(new Idiom (){ ID=3, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 1, DescriptionTextFrom = "Common phrases", DescriptionTextTo = "Общие фразы", TextFrom = "for a change", TextTo = "для разнообразия", ExampleTextFrom = string.Empty, ExampleTextTo = string.Empty });
            listObj.Add(new Idiom (){ ID=4, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 2, DescriptionTextFrom = "Begining, Ending", DescriptionTextTo = "Начало, конец", TextFrom = "shelf life", TextTo = "срок хранения/предельная дата гарантированного качества хранении/срок существования", ExampleTextFrom = "There was an interesting article 'All marriages have a shelf life' in yesterday’s paper.", ExampleTextTo = "Во вчерашней газете была опубликована интересная статья «Все брачные союзы имеют свой срок»."});
            listObj.Add(new Idiom (){ ID=5, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 2, DescriptionTextFrom = "Begining, Ending", DescriptionTextTo = "Начало, конец", TextFrom = "from the word go", TextTo = "с самого начала/с начала до конца", ExampleTextFrom = "Right from the word go, many of the players looked out of breath and out of their depth.", ExampleTextTo = "С самого начала игры многие из игроков выглядели усталыми и растерянными."});
            listObj.Add(new Idiom (){ ID=6, UpdateDate = DateTime.Parse("2017-10-13"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 3, DescriptionTextFrom = "Many", DescriptionTextTo = "Много", TextFrom = "to give chapter and verse", TextTo = "давать во всех подробностях", ExampleTextFrom = "The book gives chapter and verse on how to select a product, advertising, distribution and finances.", ExampleTextTo = "В книге дается подробная информация о том, как выбрать продукт для изготовления, о рекламе, реализации и финансировании предприятия."});
            listObj.Add(new Idiom (){ ID=7, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 3, DescriptionTextFrom = "Many", DescriptionTextTo = "Много", TextFrom = "to have smth. coming out of one’s ears", TextTo = "иметься в изобилии", ExampleTextFrom = "Everyone who wants to talk to me is talking about football. I can’t get away from it. I have had football coming out of my ears.", ExampleTextTo = "Каждый, кто начинает разговор со мной, говорит о футболе. Мне от этого некуда деться. Я сыт футболом по горло."});
            listObj.Add(new Idiom (){ ID=8, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 4, DescriptionTextFrom = "Count", DescriptionTextTo = "Количество", TextFrom = "to be up for grabs", TextTo = "доступный каждому желающему", ExampleTextFrom="Sixty-five seats are up for grabs in this speciality at the university.", ExampleTextTo="По этой специальности в университете проводится конкурс на шестьдесят пять мест, доступных каждому."});
            listObj.Add(new Idiom (){ ID=9, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 4, DescriptionTextFrom = "Count", DescriptionTextTo = "Количество", TextFrom = "to have smth. enough and to spare", TextTo = "иметь чего-то более чем достаточно/ хватит за глаза", ExampleTextFrom="We have had patience enough and to spare.", ExampleTextTo="Терпения у нас было более чем достаточно."});
            listObj.Add(new Idiom (){ ID=10, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 5, DescriptionTextFrom = "Information", DescriptionTextTo = "Информация", TextFrom = "to chew the fat", TextTo = "болтать, беседовать о том, о сем", ExampleTextFrom="We were chewing the fat for a couple of hours.", ExampleTextTo="Мы провели пару часов, болтая о том, о сем."});
            listObj.Add(new Idiom (){ ID=11, UpdateDate = DateTime.Parse("2017-10-13"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 5, DescriptionTextFrom = "Information", DescriptionTextTo = "Информация", TextFrom = "in full flow", TextTo = "увлеченно", ExampleTextFrom="She was in full flow, telling me how she had managed to bring the furniture home.", ExampleTextTo="Она с увлечением рассказала мне, как ей удалось доставить мебель домой."});
            listObj.Add(new Idiom (){ ID=11, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 6, DescriptionTextFrom = "Intellect, mind", DescriptionTextTo = "Интеллект, мышление", TextFrom = "Live and learn", TextTo = "Век живи — век учись.", ExampleTextFrom="Nowadays people change their trade or occupation many times in their life. Live and learn.", ExampleTextTo="В наши дни люди меняют профессию и род занятий много раз в течение жизни. Век живи — век учись."});
            listObj.Add(new Idiom (){ ID=12, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 6, DescriptionTextFrom = "Intellect, mind", DescriptionTextTo = "Интеллект, мышление", TextFrom = "Experience is the mother of wisdom", TextTo = "Опыт — источник/основа мудрости.", ExampleTextFrom="Не has twenty-year experience in teaching, and experience is the mother of wisdom.", ExampleTextTo="У него двадцатилетний опыт преподавания, а опыт — источник мудрости."});
            listObj.Add(new Idiom (){ ID=13, UpdateDate = DateTime.Parse("2017-10-12"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 6, DescriptionTextFrom = "Intellect, mind", DescriptionTextTo = "Интеллект, мышление", TextFrom = "to know smth. like the back of one’s hand", TextTo = "знать что-либо как свои пять пальцев", ExampleTextFrom="He knows the city like the back of his hand.", ExampleTextTo="Он знает город как свои пять пальцев."});
            return listObj;
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
