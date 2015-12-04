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

		public void CreateDefaultData ()
		{
			DAL.Repository<DefinitionTypes> repos = new TranslateHelper.Core.DAL.Repository<DefinitionTypes> ();
            DefinitionTypes[] defTypesList = new DefinitionTypes[] {
				new DefinitionTypes (){ Name = "noun" }, 
				new DefinitionTypes (){ Name = "verb" },
				new DefinitionTypes (){ Name = "adjective" },
                new DefinitionTypes (){ Name = "adverb" },
            };
			repos.SaveItemsInTransaction (defTypesList);
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
    }
}

