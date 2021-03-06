﻿using System;
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
        }

        internal IndexedCollection<IdiomItem> GetIdiomsForDirections(int languageFromId, int languageToId, string searchString)
        {
            IEnumerable<Idiom> dataView = getIdiomsBySearchString(languageFromId, languageToId, searchString);
            var dataCategoryView = db.Table<IdiomCategory>().Where(item => item.DeleteMark == 0);
            IndexedCollection<IdiomItem> indexedCollection = getIdiomIndexedCollection(dataView, dataCategoryView);
            return indexedCollection;
        }

        private static IndexedCollection<IdiomItem> getIdiomIndexedCollection(IEnumerable<Idiom> dataView, IEnumerable<IdiomCategory> dataCategoryView)
        {
            IndexedCollection<IdiomItem> indexedCollection = new IndexedCollection<IdiomItem>();
            foreach (var item in dataView)
            {
                var idiom = new IdiomItem() { Id = item.ID, TextFrom = item.TextFrom, TextTo = item.TextTo, ExampleTextFrom = item.ExampleTextFrom, ExampleTextTo = item.ExampleTextTo };
                var category = dataCategoryView.Where(i => i.ID == item.CategoryID).Single();
                idiom.CategoryTextFrom = category.TextFrom;
                indexedCollection.Add(idiom);
            }

            return indexedCollection;
        }

        private IEnumerable<Idiom> getIdiomsBySearchString(int languageFromId, int languageToId, string searchString)
        {
            string loweredSearchString = searchString.ToLower();
            return db.Table<Idiom>().Where(item => ((
            (item.LanguageFrom == languageToId && item.LanguageTo == languageFromId)
            || (item.LanguageFrom == languageFromId && item.LanguageTo == languageToId)
            )
            && (item.TextFrom.ToLower().Contains(loweredSearchString) || item.TextTo.ToLower().Contains(loweredSearchString))
            && item.DeleteMark == 0));
        }

        private Idiom[] GetDefaultData()
        {
            Idiom[] list = new Idiom[] {
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
        public void InsertItemsInTransaction(IEnumerable<Idiom> items)
        {
            Repository<Idiom> repo = new Repository<Idiom>();
            repo.DeleteAndAddItemsInTransaction(items);
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
