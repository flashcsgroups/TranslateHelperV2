using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.BL.Contracts;
using PortableCore.DAL;

namespace PortableCore
{
	public class SourceExpressionManager : IDataManager<SourceExpression>
    {
		public SourceExpressionManager()
		{
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

        public IEnumerable<SourceExpression> GetItemsForText(string text)
        {
            throw new Exception("not realized");
            //return SqlLiteInstance.DB.Table<SourceExpression>().ToList().Where(item => item.Text == text);
        }
    }
}

