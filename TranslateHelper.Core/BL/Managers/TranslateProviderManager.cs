using System;
using System.Collections.Generic;
using TranslateHelper.Core.Bl.Contracts;

namespace TranslateHelper.Core
{
	public class TranslateProviderManager : IDataManager<TranslateProvider>
    {
		public TranslateProviderManager()
		{
		}

		public void InitDefaultData ()
		{
			DAL.Repository<TranslateProvider> repos = new TranslateHelper.Core.DAL.Repository<TranslateProvider> ();
			TranslateProvider[] data = getDefaultData ();
			if(GetItems().Count != data.Length)
				repos.SaveItemsInTransaction (data);

		}

		public bool NeedUpdateDefaultData()
		{
			return true;
		}

        public TranslateProvider GetItemForId(int Id)
        {
            DAL.Repository<TranslateProvider> repos = new TranslateHelper.Core.DAL.Repository<TranslateProvider>();
            TranslateProvider result = repos.GetItem(Id);
            return result;
        }

        public List<TranslateProvider> GetItems()
		{
			DAL.Repository<TranslateProvider> repos = new TranslateHelper.Core.DAL.Repository<TranslateProvider> ();
			return new List<TranslateProvider> (repos.GetItems ());
		}

		private TranslateProvider[] getDefaultData()
		{
			TranslateProvider[] providersList = new TranslateProvider[] {
				new TranslateProvider (){ Name = "Yandex", ID = 1 }, 
				new TranslateProvider (){ Name = "Google", ID = 2 },
				new TranslateProvider (){ Name = "Offline", ID = 3 }, 
			};

			return providersList;
		}
	}
}

