using System;

namespace TranslateHelper.Core
{
	public static class TranslateProviderManager
	{
		static TranslateProviderManager ()
		{
		}

		public static void CreateDefaultData ()
		{
			DAL.Repository<TranslateProvider> repos = new TranslateHelper.Core.DAL.Repository<TranslateProvider> ();
			TranslateProvider[] providersList = new TranslateProvider[] {
				new TranslateProvider (){ Name = "Yandex" }, 
				new TranslateProvider (){ Name = "Google" },
				new TranslateProvider (){ Name = "Offline" }, 
			};

			repos.SaveItemsInTransaction (providersList);
		}

		public static bool RequiredInitDefaultData ()
		{
			//todo:   сделать проверку на необходимость обновления
			return true;
		}
	}
}

