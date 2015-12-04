using System;
using System.Collections.Generic;
using System.Linq;
using TranslateHelper.Core.Bl.Contracts;
using TranslateHelper.Core.BL.Contracts;
using TranslateHelper.Core.DAL;

namespace TranslateHelper.Core
{
	public class FavoritesManager : IDataManager<Favorites>
    {
		public FavoritesManager()
		{
		}

		public void CreateDefaultData ()
		{
		}

        public Favorites GetItemForId(int id)
        {
            DAL.Repository<Favorites> repos = new TranslateHelper.Core.DAL.Repository<Favorites>();
            Favorites result = repos.GetItem(id);
            return result;
        }

        public Favorites GetItemForTranslatedExpressionId(int translatedExpressionId)
        {
            Favorites transExprItem = SqlLiteInstance.DB.Table<Favorites>().ToList().Where(item => item.TranslatedExpressionID == translatedExpressionId).FirstOrDefault();
            return transExprItem;
        }

        public void AddNewWord(int translatedExpressionId)
        {
            DAL.Repository<Favorites> reposFavorites = new TranslateHelper.Core.DAL.Repository<Favorites>();
            Favorites itemFavorites = new Favorites();
            itemFavorites.TranslatedExpressionID = translatedExpressionId;
            reposFavorites.Save(itemFavorites);
        }

        public List<Favorites> GetItems()
        {
            DAL.Repository<Favorites> repos = new TranslateHelper.Core.DAL.Repository<Favorites>();
            return new List<Favorites>(repos.GetItems());
        }
    }
}

