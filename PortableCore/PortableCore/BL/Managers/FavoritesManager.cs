using System.Collections.Generic;
using PortableCore.BL.Contracts;
using PortableCore.DL;

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
            DAL.Repository<Favorites> repos = new DAL.Repository<Favorites>();
            Favorites result = repos.GetItem(id);
            return result;
        }

        private void AddWord(int translatedExpressionId)
        {
            DAL.Repository<Favorites> reposFavorites = new DAL.Repository<Favorites>();
            Favorites itemFavorites = new Favorites();
            itemFavorites.TranslatedExpressionID = translatedExpressionId;
            reposFavorites.Save(itemFavorites);
        }

        public List<Favorites> GetItems()
        {
            DAL.Repository<Favorites> repos = new DAL.Repository<Favorites>();
            return new List<Favorites>(repos.GetItems());
        }

        public void AddWordToFavorites(int translatedExpressionId)
        {
            FavoritesManager favoritesManager = new FavoritesManager(db);
            favoritesManager.AddWord(translatedExpressionId);
        }
    }
}

