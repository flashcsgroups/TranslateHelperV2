using System;
using System.Collections.Generic;

namespace TranslateHelper.Core
{
	public class TranslateProviderManager
	{
		public TranslateProviderManager ()
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

		public List<TranslateProvider> GetItems()
		{
			DAL.Repository<TranslateProvider> repos = new TranslateHelper.Core.DAL.Repository<TranslateProvider> ();
			return new List<TranslateProvider> (repos.GetItems ());
		}
	}
}

