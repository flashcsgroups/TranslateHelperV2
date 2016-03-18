using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using PortableCore.DAL;

namespace PortableCore.BL.Managers
{
	public class SourceDefinitionManager : IInitDataTable<SourceDefinition>
    {
        ISQLiteTesting db;

		public SourceDefinitionManager(ISQLiteTesting dbHelper)
		{
            db = dbHelper;
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
            Repository<SourceDefinition> repos = new Repository<SourceDefinition>();
            SourceDefinition result = repos.GetItem(id);
            return result;
        }

		public List<SourceDefinition> GetItems()
		{
            Repository<SourceDefinition> repos = new Repository<SourceDefinition>();
			return new List<SourceDefinition> (repos.GetItems ());
		}

        public List<SourceDefinition> GetDefinitionCollection(int sourceId)
        {
            //var definitionsView = from defItem in SqlLiteInstance.DB.Table<SourceDefinition>() where defItem.SourceExpressionID == sourceId select defItem;
            var definitionsView = from defItem in db.Table<SourceDefinition>() where defItem.SourceExpressionID == sourceId select defItem;
            return definitionsView.ToList();
        }
    }
}

