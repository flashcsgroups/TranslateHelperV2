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
				new DefinitionTypes (){ Name = "noun", ID = (int)GetEnumDefinitionTypeFromName("noun")}, 
				new DefinitionTypes (){ Name = "verb", ID = (int)GetEnumDefinitionTypeFromName("verb")},
				new DefinitionTypes (){ Name = "adjective",ID = (int)GetEnumDefinitionTypeFromName("adjective")},
				new DefinitionTypes (){ Name = "adverb",ID =  (int)GetEnumDefinitionTypeFromName("adverb")},
                new DefinitionTypes (){ Name = "participle",ID =  (int)GetEnumDefinitionTypeFromName("participle")},
            };
			return defTypesList;
		}

        public static DefinitionTypesEnum GetEnumDefinitionTypeFromName(string name)
        {
            DefinitionTypesEnum result = DefinitionTypesEnum.unknown;

            switch(name.ToLower())
            {
                case "существительное":
                case "noun":
                    {
                        result = DefinitionTypesEnum.noun;
                    }; break;
                case "глагол":
                case "verb":
                    {
                        result = DefinitionTypesEnum.verb;
                    }; break;                    
                case "прилагательное":
                case "adjective":
                    {
                        result = DefinitionTypesEnum.adjective;
                    }; break;
                case "наречие":
                case "adverb":
                    {
                        result = DefinitionTypesEnum.adverb;
                    }; break;
                case "частица":
                case "причастие":
                case "participle":
                    {
                        result = DefinitionTypesEnum.participle;
                    }; break;
                default:
                    {
                    }; break;
            }
            return result;
        }
    }
}

