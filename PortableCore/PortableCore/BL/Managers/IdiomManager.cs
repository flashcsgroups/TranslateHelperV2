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

        private Idiom[] GetDefaultData()
        {
            var eng = languageManager.GetItemForShortName("en").ID;
            var rus = languageManager.GetItemForShortName("ru").ID;
            Idiom[] list = new Idiom[] {
                new Idiom (){ ID=1, LanguageFrom = eng, LanguageTo = rus, CategoryID = 1, DescriptionTextFrom = "Common phrases", DescriptionTextTo = "Общие фразы", TextFrom = "all in all", TextTo = "в конечном счёте, с учётом всех обстоятельств / в целом"},
                new Idiom (){ ID=2, LanguageFrom = eng, LanguageTo = rus, CategoryID = 1, DescriptionTextFrom = "Common phrases", DescriptionTextTo = "Общие фразы", TextFrom = "all the way", TextTo = "от начала до конца"},
                new Idiom (){ ID=3, LanguageFrom = eng, LanguageTo = rus, CategoryID = 1, DescriptionTextFrom = "Common phrases", DescriptionTextTo = "Общие фразы", TextFrom = "for a change", TextTo = "для разнообразия"},
                new Idiom (){ ID=4, LanguageFrom = eng, LanguageTo = rus, CategoryID = 2, DescriptionTextFrom = "Begining, Ending", DescriptionTextTo = "Начало, конец", TextFrom = "shelf life", TextTo = "срок хранения/предельная дата гарантированного качества хранении/срок существования", ExampleTextFrom = "There was an interesting article 'All marriages have a shelf life' in yesterday’s paper.", ExampleTextTo = "Во вчерашней газете была опубликована интересная статья «Все брачные союзы имеют свой срок»."},
                new Idiom (){ ID=5, LanguageFrom = eng, LanguageTo = rus, CategoryID = 2, DescriptionTextFrom = "Begining, Ending", DescriptionTextTo = "Начало, конец", TextFrom = "from the word go", TextTo = "с самого начала/с начала до конца", ExampleTextFrom = "Right from the word go, many of the players looked out of breath and out of their depth.", ExampleTextTo = "С самого начала игры многие из игроков выглядели усталыми и растерянными."},
                new Idiom (){ ID=6, LanguageFrom = eng, LanguageTo = rus, CategoryID = 3, DescriptionTextFrom = "Many", DescriptionTextTo = "Много", TextFrom = "to give chapter and verse", TextTo = "давать во всех подробностях", ExampleTextFrom = "The book gives chapter and verse on how to select a product, advertising, distribution and finances.", ExampleTextTo = "В книге дается подробная информация о том, как выбрать продукт для изготовления, о рекламе, реализации и финансировании предприятия."},
                new Idiom (){ ID=7, LanguageFrom = eng, LanguageTo = rus, CategoryID = 3, DescriptionTextFrom = "Many", DescriptionTextTo = "Много", TextFrom = "to have smth. coming out of one’s ears", TextTo = "иметься в изобилии", ExampleTextFrom = "Everyone who wants to talk to me is talking about football. I can’t get away from it. I have had football coming out of my ears.", ExampleTextTo = "Каждый, кто начинает разговор со мной, говорит о футболе. Мне от этого некуда деться. Я сыт футболом по горло."},
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
    }
}
