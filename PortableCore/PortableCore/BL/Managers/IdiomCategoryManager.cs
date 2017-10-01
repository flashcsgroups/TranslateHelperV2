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
    public class IdiomCategoryManager : IInitDataTable<IdiomCategory>, IIdiomCategoryManager
    {
        readonly ISQLiteTesting db;
        private ILanguageManager languageManager;


        public IdiomCategoryManager(ISQLiteTesting dbHelper, ILanguageManager languageManager)
        {
            this.db = dbHelper;
            this.languageManager = languageManager;
        }

        public void ClearTable()
        {
            DAL.Repository<IdiomCategory> repos = new DAL.Repository<IdiomCategory>();
            repos.DeleteAllDataInTable();
        }

        public void InitDefaultData()
        {
            Repository<IdiomCategory> repos = new Repository<IdiomCategory>();
            IdiomCategory[] data = GetDefaultData();
            int hashOriginalData = data.Sum(i=>i.ID);
            int hashRepositoryData = repos.GetHashForItems();
            if(hashOriginalData != hashRepositoryData)
            {
                repos.DeleteAllDataInTable();
                repos.AddItemsInTransaction(data);
            }
        }

        private IdiomCategory[] GetDefaultData()
        {
            var eng = languageManager.GetItemForShortName("en").ID;
            var rus = languageManager.GetItemForShortName("ru").ID;
            IdiomCategory[] list = new IdiomCategory[] {
                new IdiomCategory (){ ID=1, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Frequently used phrases", TextTo="Часто используемые фразы"},
                new IdiomCategory (){ ID=2, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Time", TextTo="Время"},
                new IdiomCategory (){ ID=3, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Count", TextTo="Наличие"},
                new IdiomCategory (){ ID=4, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Count", TextTo="Количество"},
                new IdiomCategory (){ ID=5, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Information", TextTo="Информация"},
                new IdiomCategory (){ ID=6, LanguageFrom = eng, LanguageTo = rus, TextFrom = "Intellect, mind", TextTo="Интеллект, мышление"},
            };

            return list;
        }

        private int getHash()
        {
            return 0;
        }
        public IdiomCategory GetItemForId(int id)
        {
            Repository<IdiomCategory> repos = new Repository<IdiomCategory>();
            return repos.GetItem(id);
        }

        public int SaveItem(IdiomCategory item)
        {
            Repository<IdiomCategory> repo = new Repository<IdiomCategory>();
            return repo.Save(item);
        }
    }
}
