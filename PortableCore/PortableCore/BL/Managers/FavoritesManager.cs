using System;
using System.Collections.Generic;
using System.Linq;
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
            DAL.Repository<Favorites> repos = new PortableCore.DAL.Repository<Favorites>();
            Favorites result = repos.GetItem(id);
            return result;
        }

        public Favorites GetItemForTranslatedExpressionId(int translatedExpressionId)
        {
            throw new Exception("Not realized");
            //Favorites transExprItem = Core.DAL.SqlLiteInstance.DB.Table<Favorites>().ToList().Where(item => item.TranslatedExpressionID == translatedExpressionId).FirstOrDefault();
            //return transExprItem;
        }

        private void AddWord(int translatedExpressionId)
        {
            DAL.Repository<Favorites> reposFavorites = new PortableCore.DAL.Repository<Favorites>();
            Favorites itemFavorites = new Favorites();
            itemFavorites.TranslatedExpressionID = translatedExpressionId;
            reposFavorites.Save(itemFavorites);
        }

        public List<Favorites> GetItems()
        {
            DAL.Repository<Favorites> repos = new PortableCore.DAL.Repository<Favorites>();
            return new List<Favorites>(repos.GetItems());
        }

		/*public IndexedCollection<TranslateResultView> GetItemsForFavoritesList()
		{
			IndexedCollection<TranslateResultView> result = new IndexedCollection<TranslateResultView> ();
			TranslatedExpressionManager transExprManager = new TranslatedExpressionManager (db);
			DefinitionTypesManager defTypesManager = new DefinitionTypesManager (db);
            SourceExpressionManager sourceManager = new SourceExpressionManager(db);
			var test = defTypesManager.GetItemForId (0);
			var transExprItems = transExprManager.GetItems ();
			//result.AddList (resultView.ToList ());
			return result;
		}*/

        public void AddWordToFavorites(string sourceText, TranslateResultView result)
        {
            SourceExpressionManager sourceExprManager = new SourceExpressionManager(db);
            IEnumerable<SourceExpression> sourceEnumerator = sourceExprManager.GetSourceExpressionCollection(sourceText);
            List<SourceExpression> listSourceExpr = sourceEnumerator.ToList<SourceExpression>();
            if (listSourceExpr.Count > 0)
            {
                int sourceId = listSourceExpr[0].ID;
                TranslatedExpressionManager transExprManager = new TranslatedExpressionManager(db);
                throw new Exception("not realized");
                /*IEnumerable<TranslatedExpression> transEnumerator = transExprManager.GetTranslateResultFromLocalCache(sourceId);
                var transExprItem = transEnumerator.Where(item => item.TranslatedText == result.TranslatedText).Single<TranslatedExpression>();
                FavoritesManager favoritesManager = new FavoritesManager();
                favoritesManager.AddWord(transExprItem.ID);*/
                /*Android.Widget.Toast.MakeText(this, "Элемент добавлен в избранное", Android.Widget.ToastLength.Short).Show();

                IRequestTranslateString translaterFromCache = new LocalDatabaseCache();
                EditText editSourceText = FindViewById<EditText>(Resource.Id.textSourceString);
                var resultFromCache = await translaterFromCache.Translate(sourceText, "en-ru");
                if (resultFromCache.translateResult.Collection.Count > 0)
                {
                    UpdateListResults(sourceText, resultFromCache, false);
                }*/

            }
        }


    }
}

