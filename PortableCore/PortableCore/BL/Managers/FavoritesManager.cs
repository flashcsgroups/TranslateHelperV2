using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
	public class FavoritesManager : IDataManager<Favorites>
    {
		public FavoritesManager()
		{
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

		public TranslateResultIndexedCollection<TranslateResult> GetItemsForFavoritesList()
		{
			TranslateResultIndexedCollection<TranslateResult> result = new TranslateResultIndexedCollection<TranslateResult> ();
			TranslatedExpressionManager transExprManager = new TranslatedExpressionManager ();
			DefinitionTypesManager defTypesManager = new DefinitionTypesManager ();
            SourceExpressionManager sourceManager = new SourceExpressionManager();
			var test = defTypesManager.GetItemForId (0);
			var transExprItems = transExprManager.GetItems ();
            var resultView = from favItem in GetItems()
                             join transExprItem in transExprItems on favItem.TranslatedExpressionID
                             equals transExprItem.ID 
				select new TranslateResult("")
                {
				    //OriginalText = sourceManager.GetItemForId(transExprItem.SourceExpressionID).Text, 
				    TranslatedText = transExprItem.TranslatedText, 
				    Ts = transExprItem.TranscriptionText, 
				    Pos = defTypesManager.GetItemForId(transExprItem.DefinitionTypeID).Name,
				    TranslatedExpressionId = transExprItem.ID,
				    FavoritesId = favItem.ID
			    };
			result.AddList (resultView.ToList ());
			return result;
		}

        public void AddWordToFavorites(string sourceText, TranslateResult result)
        {
            SourceExpressionManager sourceExprManager = new SourceExpressionManager();
            IEnumerable<SourceExpression> sourceEnumerator = sourceExprManager.GetItemsForText(sourceText);
            List<SourceExpression> listSourceExpr = sourceEnumerator.ToList<SourceExpression>();
            if (listSourceExpr.Count > 0)
            {
                int sourceId = listSourceExpr[0].ID;
                TranslatedExpressionManager transExprManager = new TranslatedExpressionManager();
                IEnumerable<TranslatedExpression> transEnumerator = transExprManager.GetTranslateResultFromLocalCache(sourceId);
                var transExprItem = transEnumerator.Where(item => item.TranslatedText == result.TranslatedText).Single<TranslatedExpression>();
                FavoritesManager favoritesManager = new FavoritesManager();
                favoritesManager.AddWord(transExprItem.ID);
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

