using System;
using System.Collections.Generic;
using System.Linq;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using PortableCore.DAL;

namespace PortableCore.BL.Managers
{
	public class SourceExpressionManager : IInitDataTable<SourceExpression>, ISourceExpressionManager
    {
        ISQLiteTesting db;

        public SourceExpressionManager(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

		public void InitDefaultData ()
		{
            //нечего заполнять
        }

        public SourceExpression GetItemForId(int id)
        {
            Repository<SourceExpression> repos = new Repository<SourceExpression>();
            SourceExpression result = repos.GetItem(id);
            return result;
        }

        public IEnumerable<SourceExpression> GetSourceExpressionCollection(string sourceText, TranslateDirection direction)
        {
            return db.Table<SourceExpression>().ToList().Where(item => item.Text == sourceText && item.LanguageFromID == direction.LanguageFrom.ID && item.LanguageToID == direction.LanguageTo.ID);
        }

    }
}

