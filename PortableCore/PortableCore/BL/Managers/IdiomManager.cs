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
    public class IdiomManager : IInitDataTable<Idiom>, IIdiomManager
    {
        readonly ISQLiteTesting db;
        private ILanguageManager languageManager;


        public IdiomManager(ISQLiteTesting dbHelper, ILanguageManager languageManager)
        {
            this.db = dbHelper;
            this.languageManager = languageManager;
        }

        public void ClearTable()
        {
            DAL.Repository<Idiom> repos = new DAL.Repository<Idiom>();
            repos.DeleteAllDataInTable();
        }

        public void InitDefaultData()
        {
            ClearTable();
            Repository<Idiom> repos = new Repository<Idiom>();
            Idiom[] data = GetDefaultData();
            if (repos.Count() != data.Length)
            {
                repos.DeleteAllDataInTable();
                repos.AddItemsInTransaction(data);
            }
        }

        internal IndexedCollection<IdiomItem> GetIdiomsForDirections(int languageFromId, int languageToId, string searchString)
        {
            var dataView = db.Table<Idiom>().Where(item => ((
            (item.LanguageFrom == languageToId && item.LanguageTo == languageFromId) 
            || (item.LanguageFrom == languageFromId && item.LanguageTo == languageToId)
            ) 
            && (item.TextFrom.Contains(searchString) || item.TextTo.ToLower().Contains(searchString) || item.ExampleTextFrom.Contains(searchString) || item.ExampleTextTo.Contains(searchString))
            && item.DeleteMark == 0));
            var dataCategoryView = db.Table<IdiomCategory>().Where(item=>item.DeleteMark == 0);
            IndexedCollection<IdiomItem> indexedCollection = new IndexedCollection<IdiomItem>();
            foreach (var item in dataView)
            {
                var idiom = new IdiomItem() { Id = item.ID, TextFrom = item.TextFrom, TextTo = item.TextTo, ExampleTextFrom = item.ExampleTextFrom, ExampleTextTo = item.ExampleTextTo };
                var category = dataCategoryView.Where(i=>i.ID == item.CategoryID).Single();
                idiom.CategoryTextFrom = category.TextFrom;
                indexedCollection.Add(idiom);
            }
            return indexedCollection;
        }

        /*internal List<int> Search(int languageFromId, int languageToId, string searchString)
        {
            List<int> iDs = new List<int>();
            var dataView = db.Table<Idiom>().Where(item => );
            foreach (var item in dataView)
            {
                iDs.Add(item.ID);
            }
            return iDs;
        }*/

        private Idiom[] GetDefaultData()
        {
            var eng = languageManager.GetItemForShortName("en").ID;
            var rus = languageManager.GetItemForShortName("ru").ID;
            Idiom[] list = new Idiom[] {
                new Idiom (){ ID=1, LanguageFrom = eng, LanguageTo = rus, CategoryID = 1, DescriptionTextFrom = "Common phrases", DescriptionTextTo = "Общие фразы", TextFrom = "all in all", TextTo = "в конечном счёте, с учётом всех обстоятельств / в целом", ExampleTextFrom=string.Empty, ExampleTextTo=string.Empty},
                new Idiom (){ ID=2, LanguageFrom = eng, LanguageTo = rus, CategoryID = 1, DescriptionTextFrom = "Common phrases", DescriptionTextTo = "Общие фразы", TextFrom = "all the way", TextTo = "от начала до конца", ExampleTextFrom=string.Empty, ExampleTextTo=string.Empty},
                new Idiom (){ ID=3, LanguageFrom = eng, LanguageTo = rus, CategoryID = 1, DescriptionTextFrom = "Common phrases", DescriptionTextTo = "Общие фразы", TextFrom = "for a change", TextTo = "для разнообразия", ExampleTextFrom=string.Empty, ExampleTextTo=string.Empty},
                new Idiom (){ ID=4, LanguageFrom = eng, LanguageTo = rus, CategoryID = 2, DescriptionTextFrom = "Begining, Ending", DescriptionTextTo = "Начало, конец", TextFrom = "shelf life", TextTo = "срок хранения/предельная дата гарантированного качества хранении/срок существования", ExampleTextFrom = "There was an interesting article 'All marriages have a shelf life' in yesterday’s paper.", ExampleTextTo = "Во вчерашней газете была опубликована интересная статья «Все брачные союзы имеют свой срок»."},
                new Idiom (){ ID=5, LanguageFrom = eng, LanguageTo = rus, CategoryID = 2, DescriptionTextFrom = "Begining, Ending", DescriptionTextTo = "Начало, конец", TextFrom = "from the word go", TextTo = "с самого начала/с начала до конца", ExampleTextFrom = "Right from the word go, many of the players looked out of breath and out of their depth.", ExampleTextTo = "С самого начала игры многие из игроков выглядели усталыми и растерянными."},
                new Idiom (){ ID=6, LanguageFrom = eng, LanguageTo = rus, CategoryID = 3, DescriptionTextFrom = "Many", DescriptionTextTo = "Много", TextFrom = "to give chapter and verse", TextTo = "давать во всех подробностях", ExampleTextFrom = "The book gives chapter and verse on how to select a product, advertising, distribution and finances.", ExampleTextTo = "В книге дается подробная информация о том, как выбрать продукт для изготовления, о рекламе, реализации и финансировании предприятия."},
                new Idiom (){ ID=7, LanguageFrom = eng, LanguageTo = rus, CategoryID = 3, DescriptionTextFrom = "Many", DescriptionTextTo = "Много", TextFrom = "to have smth. coming out of one’s ears", TextTo = "иметься в изобилии", ExampleTextFrom = "Everyone who wants to talk to me is talking about football. I can’t get away from it. I have had football coming out of my ears.", ExampleTextTo = "Каждый, кто начинает разговор со мной, говорит о футболе. Мне от этого некуда деться. Я сыт футболом по горло."},
                new Idiom (){ ID=8, LanguageFrom = eng, LanguageTo = rus, CategoryID = 4, DescriptionTextFrom = "Count", DescriptionTextTo = "Количество", TextFrom = "to be up for grabs", TextTo = "доступный каждому желающему", ExampleTextFrom="Sixty-five seats are up for grabs in this speciality at the university.", ExampleTextTo="По этой специальности в университете проводится конкурс на шестьдесят пять мест, доступных каждому."},
                new Idiom (){ ID=9, LanguageFrom = eng, LanguageTo = rus, CategoryID = 4, DescriptionTextFrom = "Count", DescriptionTextTo = "Количество", TextFrom = "to have smth. enough and to spare", TextTo = "иметь чего-то более чем достаточно/ хватит за глаза", ExampleTextFrom="We have had patience enough and to spare.", ExampleTextTo="Терпения у нас было более чем достаточно."},
                new Idiom (){ ID=10, LanguageFrom = eng, LanguageTo = rus, CategoryID = 5, DescriptionTextFrom = "Information", DescriptionTextTo = "Информация", TextFrom = "to chew the fat", TextTo = "болтать, беседовать о том, о сем", ExampleTextFrom="We were chewing the fat for a couple of hours.", ExampleTextTo="Мы провели пару часов, болтая о том, о сем."},
                new Idiom (){ ID=11, LanguageFrom = eng, LanguageTo = rus, CategoryID = 5, DescriptionTextFrom = "Information", DescriptionTextTo = "Информация", TextFrom = "in full flow", TextTo = "увлеченно", ExampleTextFrom="She was in full flow, telling me how she had managed to bring the furniture home.", ExampleTextTo="Она с увлечением рассказала мне, как ей удалось доставить мебель домой."},
                new Idiom (){ ID=11, LanguageFrom = eng, LanguageTo = rus, CategoryID = 6, DescriptionTextFrom = "Intellect, mind", DescriptionTextTo = "Интеллект, мышление", TextFrom = "live and learn", TextTo = "век живи — век учись.", ExampleTextFrom="Nowadays people change their trade or occupation many times in their life. Live and learn.", ExampleTextTo="В наши дни люди меняют профессию и род занятий много раз в течение жизни. Век живи — век учись."},
                new Idiom (){ ID=12, LanguageFrom = eng, LanguageTo = rus, CategoryID = 6, DescriptionTextFrom = "Intellect, mind", DescriptionTextTo = "Интеллект, мышление", TextFrom = "experience is the mother of wisdom", TextTo = "опыт — источник/основа мудрости.", ExampleTextFrom="Не has twenty-year experience in teaching, and experience is the mother of wisdom.", ExampleTextTo="У него двадцатилетний опыт преподавания, а опыт — источник мудрости."},
                new Idiom (){ ID=13, LanguageFrom = eng, LanguageTo = rus, CategoryID = 6, DescriptionTextFrom = "Intellect, mind", DescriptionTextTo = "Интеллект, мышление", TextFrom = "to know smth. like the back of one’s hand", TextTo = "знать что-либо как свои пять пальцев", ExampleTextFrom="He knows the city like the back of his hand.", ExampleTextTo="Он знает город как свои пять пальцев."}
            };

            return list;
        }

        public Idiom GetItemForId(int id)
        {
            Repository<Idiom> repos = new Repository<Idiom>();
            return repos.GetItem(id);
        }

        public int SaveItem(Idiom item)
        {
            Repository<Idiom> repo = new Repository<Idiom>();
            return repo.Save(item);
        }

        public List<DirectionIdiomItem> GetListDirections()
        {
            List<DirectionIdiomItem> resultList = new List<DirectionIdiomItem>
            {
                new DirectionIdiomItem(languageManager.GetItemForShortName("en"), languageManager.GetItemForShortName("ru"), "")
            };
            return resultList;
        }
    }
}
