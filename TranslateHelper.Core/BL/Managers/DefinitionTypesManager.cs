using System;
using System.Collections.Generic;
using System.Linq;
using TranslateHelper.Core.Bl.Contracts;
using TranslateHelper.Core.BL.Contracts;
using TranslateHelper.Core.DAL;

namespace TranslateHelper.Core
{
	public class DefinitionTypesManager : IDataManager<DefinitionTypes>
    {
		public DefinitionTypesManager()
		{
		}

		public void InitDefaultData ()
		{
			DAL.Repository<DefinitionTypes> repos = new TranslateHelper.Core.DAL.Repository<DefinitionTypes> ();
			DefinitionTypes[] data = getDefaultData ();
			var currentData = GetItems ();
			if (currentData.Count != data.Length) 
			{
                repos.DeleteAllDataInTable();
				repos.AddItemsInTransaction (data);
			}
		}
			
		public List<DefinitionTypes> GetItems()
		{
			DAL.Repository<DefinitionTypes> repos = new TranslateHelper.Core.DAL.Repository<DefinitionTypes> ();
			return new List<DefinitionTypes> (repos.GetItems ());
		}

        public DefinitionTypes GetItemForId(int id)
        {
            DAL.Repository<DefinitionTypes> repos = new TranslateHelper.Core.DAL.Repository<DefinitionTypes>();
            DefinitionTypes result = repos.GetItem(id);
            return result;
        }

        public IEnumerable<DefinitionTypes> GetItemsForName(string name)
        {
            return SqlLiteInstance.DB.Table<DefinitionTypes>().ToList().Where(item => item.Name == name);
        }

		private DefinitionTypes[] getDefaultData()
		{
			DefinitionTypes[] defTypesList = new DefinitionTypes[] {
				new DefinitionTypes (){ Name = "noun", ID = 0}, 
				new DefinitionTypes (){ Name = "verb", ID = 1},
				new DefinitionTypes (){ Name = "adjective", ID = 2},
				new DefinitionTypes (){ Name = "adverb", ID = 3},
			};
			return defTypesList;
		}
    }
}

