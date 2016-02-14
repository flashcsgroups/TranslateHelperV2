﻿using System.Collections.Generic;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using PortableCore.DAL;
using System;

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

        private void AddLinkWithTranslateExpression(int translatedExpressionId)
        {
            Repository<Favorites> reposFavorites = new Repository<Favorites>();
            Favorites itemFavorites = new Favorites();
            itemFavorites.TranslatedExpressionID = translatedExpressionId;
            reposFavorites.Save(itemFavorites);
        }

        public List<Favorites> GetItems()
        {
            Repository<Favorites> repos = new Repository<Favorites>();
            return new List<Favorites>(repos.GetItems());
        }

        public int AddWord(int translatedExpressionId)
        {
            int FavoriteId = getFavoritId(translatedExpressionId);
            if (FavoriteId == 0)
            {
                AddLinkWithTranslateExpression(translatedExpressionId);
                FavoriteId = getFavoritId(translatedExpressionId);
            }
            return FavoriteId;
        }

        public void DeleteWord(int favoritesId)
        {
            Repository<Favorites> reposFavorites = new Repository<Favorites>();
            reposFavorites.Delete(favoritesId);
        }

        private int getFavoritId(int translatedExpressionId)
        {
            int id = 0;
            var view = from favItem in SqlLiteInstance.DB.Table<Favorites>()
                   where favItem.TranslatedExpressionID == translatedExpressionId
                   select new Favorites { ID = favItem.ID, TranslatedExpressionID = translatedExpressionId, DeleteMark = favItem.DeleteMark };
            if(view.Count() > 0)
            {
                var firstItem = view.FirstOrDefault();
                if (firstItem != null)
                    id = firstItem.ID;
            }
            return id;
        }

    }
}

