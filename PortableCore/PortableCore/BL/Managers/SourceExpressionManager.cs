using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using PortableCore.DAL;

namespace PortableCore.BL.Managers
{
	public class SourceExpressionManager : IDataManager<SourceExpression>, ISourceExpressionManager
    {
        ISQLiteTesting db;

        public SourceExpressionManager(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

		public void InitDefaultData ()
		{                                                                                                                                                                                                                                                       
		}
			
        public SourceExpression GetItemForId(int id)
        {
            DAL.Repository<SourceExpression> repos = new PortableCore.DAL.Repository<SourceExpression>();
            SourceExpression result = repos.GetItem(id);
            return result;
        }

        public IEnumerable<SourceExpression> GetSourceExpressionCollection(string sourceText)
        {
            return db.Table<SourceExpression>().ToList().Where(item => item.Text == sourceText);
        }

    }
}

