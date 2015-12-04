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

		public void CreateDefaultData ()
		{
			DAL.Repository<TranslateProvider> repos = new TranslateHelper.Core.DAL.Repository<TranslateProvider> ();
			TranslateProvider[] providersList = new TranslateProvider[] {
				new TranslateProvider (){ Name = "Yandex" }, 
				new TranslateProvider (){ Name = "Google" },
				new TranslateProvider (){ Name = "Offline" }, 
			};
			repos.SaveItemsInTransaction (providersList);
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
	}
}

