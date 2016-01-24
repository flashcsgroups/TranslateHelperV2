using System.Collections.Generic;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using PortableCore.DAL;

namespace PortableCore.BL.Managers
{
    public class FavoritesManager : IDataManager<Favorites>
    {
        ISQLiteTesting db;

        public FavoritesManager(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

		public void InitDefaultData ()
		{
		}

		public bool NeedUpdateDefaultData()
		{
			return false;
		}

        public Favorites GetItemForId(int id)
        {
            Repository<Favorites> repos = new Repository<Favorites>();
            Favorites result = repos.GetItem(id);
            return result;
        }

        private void AddWord(int translatedExpressionId)
        {
            Repository<Favorites> reposFavorites = new DAL.Repository<Favorites>();
            Favorites itemFavorites = new Favorites();
            itemFavorites.TranslatedExpressionID = translatedExpressionId;
            reposFavorites.Save(itemFavorites);
        }

        public List<Favorites> GetItems()
        {
            Repository<Favorites> repos = new Repository<Favorites>();
            return new List<Favorites>(repos.GetItems());
        }

        public void AddWordToFavorites(int translatedExpressionId)
        {
            var view = from favItem in SqlLiteInstance.DB.Table<Favorites>()
                       where favItem.TranslatedExpressionID == translatedExpressionId
                       select new int[] { favItem.ID };
            if(view.Count() == 0)
            {
                FavoritesManager favoritesManager = new FavoritesManager(db);
                favoritesManager.AddWord(translatedExpressionId);
            }
        }
    }
}

