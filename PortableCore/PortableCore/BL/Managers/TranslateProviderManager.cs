using System;
using System.Collections.Generic;
using PortableCore.BL.Contracts;

namespace PortableCore
{
	public class TranslateProviderManager : IDataManager<TranslateProvider>
    {
		public TranslateProviderManager()
		{
		}

		public void InitDefaultData ()
		{
			DAL.Repository<TranslateProvider> repos = new PortableCore.DAL.Repository<TranslateProvider> ();
			TranslateProvider[] data = getDefaultData ();
			if(GetItems().Count != data.Length)
            {
                repos.DeleteAllDataInTable();
                repos.AddItemsInTransaction(data);
            }			
		}

        public TranslateProvider GetItemForId(int Id)
        {
            DAL.Repository<TranslateProvider> repos = new PortableCore.DAL.Repository<TranslateProvider>();
            TranslateProvider result = repos.GetItem(Id);
            return result;
        }

        public List<TranslateProvider> GetItems()
		{
			DAL.Repository<TranslateProvider> repos = new PortableCore.DAL.Repository<TranslateProvider> ();
			return new List<TranslateProvider> (repos.GetItems ());
		}

		private TranslateProvider[] getDefaultData()
		{
			TranslateProvider[] providersList = new TranslateProvider[] {
                new TranslateProvider (){ Name = "Offline", ID=0}, 
				new TranslateProvider (){ Name = "Google", ID=10},
				new TranslateProvider (){ Name = "Yandex", ID=11},
                new TranslateProvider (){ Name = "Bing", ID=12},
            };

			return providersList;
		}
	}
}

