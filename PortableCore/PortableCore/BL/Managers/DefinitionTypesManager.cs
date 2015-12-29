using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
	public class DefinitionTypesManager : IDataManager<DefinitionTypes>
    {
		public DefinitionTypesManager()
		{
		}

		public void InitDefaultData ()
		{
			DAL.Repository<DefinitionTypes> repos = new PortableCore.DAL.Repository<DefinitionTypes> ();
			DefinitionTypes[] data = getDefaultData ();
			var currentData = GetItems ();
			if (currentData.Count != data.Length) 
			{
                //repos.DeleteAllDataInTable();
				//repos.AddItemsInTransaction (data);
			}
		}
			
		public List<DefinitionTypes> GetItems()
		{
			DAL.Repository<DefinitionTypes> repos = new PortableCore.DAL.Repository<DefinitionTypes> ();
			return new List<DefinitionTypes> (repos.GetItems ());
		}

        public DefinitionTypes GetItemForId(int id)
        {
            DAL.Repository<DefinitionTypes> repos = new PortableCore.DAL.Repository<DefinitionTypes>();
            DefinitionTypes result = repos.GetItem(id);
            return result;
        }

        public IEnumerable<DefinitionTypes> GetItemsForName(string name)
        {
            throw new Exception("not realized");
            //return SqlLiteInstance.DB.Table<DefinitionTypes>().ToList().Where(item => item.Name == name);
        }

		private DefinitionTypes[] getDefaultData()
		{
			DefinitionTypes[] defTypesList = new DefinitionTypes[] {
				new DefinitionTypes (){ Name = "noun", ID = (int)DefinitionTypesEnum.noun}, 
				new DefinitionTypes (){ Name = "verb", ID = (int)DefinitionTypesEnum.verb},
				new DefinitionTypes (){ Name = "adjective",ID = (int)DefinitionTypesEnum.adjective},
				new DefinitionTypes (){ Name = "adverb",ID =  (int)DefinitionTypesEnum.adverb},
                new DefinitionTypes (){ Name = "participle",ID =  (int)DefinitionTypesEnum.participle},
            };
			return defTypesList;
		}
    }
}

