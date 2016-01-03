using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using PortableCore.Core.DAL;

namespace PortableCore.BL.Managers
{
	public class SourceDefinitionManager : IDataManager<SourceDefinition>
    {
		public SourceDefinitionManager()
		{
		}

        public void InitDefaultData()
        {
        }

		public bool NeedUpdateDefaultData()
		{
			return false;
		}

        public SourceDefinition GetItemForId(int id)
        {
            DAL.Repository<SourceDefinition> repos = new PortableCore.DAL.Repository<SourceDefinition>();
            SourceDefinition result = repos.GetItem(id);
            return result;
        }

		public List<SourceDefinition> GetItems()
		{
			DAL.Repository<SourceDefinition> repos = new PortableCore.DAL.Repository<SourceDefinition> ();
			return new List<SourceDefinition> (repos.GetItems ());
		}

        public List<SourceDefinition> GetDefinitions(int sourceId)
        {
            //DAL.Repository<SourceDefinition> repos = new PortableCore.DAL.Repository<SourceDefinition>();
            var definitionsView = from defItem in SqlLiteInstance.DB.Table<SourceDefinition>() where defItem.SourceExpressionID == sourceId select defItem;
            return definitionsView.ToList();
        }
    }
}

