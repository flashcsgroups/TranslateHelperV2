using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using PortableCore.DAL;

namespace PortableCore.BL.Managers
{
	public class SourceDefinitionManager : IDataManager<SourceDefinition>
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
            DAL.Repository<SourceDefinition> repos = new PortableCore.DAL.Repository<SourceDefinition>();
            SourceDefinition result = repos.GetItem(id);
            return result;
        }

		public List<SourceDefinition> GetItems()
		{
			DAL.Repository<SourceDefinition> repos = new PortableCore.DAL.Repository<SourceDefinition> ();
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

