using System;
using System.Collections.Generic;
using System.Linq;
using TranslateHelper.Core.Bl.Contracts;
using TranslateHelper.Core.BL.Contracts;
using TranslateHelper.Core.DAL;

namespace TranslateHelper.Core
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
            DAL.Repository<SourceExpression> repos = new TranslateHelper.Core.DAL.Repository<SourceExpression>();
            SourceExpression result = repos.GetItem(id);
            return result;
        }

        public IEnumerable<SourceExpression> GetItemsForText(string text)
        {
            return SqlLiteInstance.DB.Table<SourceExpression>().ToList().Where(item => item.Text == text);
        }
    }
}

