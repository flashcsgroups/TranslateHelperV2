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

		public void InitDefaultData ()
		{
		}

		public bool NeedUpdateDefaultData()
		{
			return false;
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
				select new TranslateResult()
                {
				    OriginalText = sourceManager.GetItemForId(transExprItem.SourceExpressionID).Text, 
				    TranslatedText = transExprItem.TranslatedText, 
				    Ts = transExprItem.TranscriptionText, 
				    Pos = defTypesManager.GetItemForId(transExprItem.DefinitionTypeID).Name,
				    TranslatedExpressionId = transExprItem.ID,
				    FavoritesId = favItem.ID
			    };
			result.AddList (resultView.ToList ());
			return result;
		}
	}
}

