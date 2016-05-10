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
            /*Repository<SourceExpression> repos = new Repository<SourceExpression>();
            if (repos.Count() == 0)
                repos.AddItemsInTransaction(getDefaultData());*/
        }

        /*private SourceExpression[] getDefaultData()
        {
            SourceExpression[] defTypesList = new SourceExpression[] {
                new SourceExpression (){DirectionID=0, Text="this", ID=1},
                new SourceExpression (){DirectionID=0, Text="test", ID=2},
                new SourceExpression (){DirectionID=0, Text="as", ID=3},
                new SourceExpression (){DirectionID=0, Text="you", ID=4},
                new SourceExpression (){DirectionID=0, Text="can", ID=5},
                new SourceExpression (){DirectionID=0, Text="see", ID=6},
                new SourceExpression (){DirectionID=0, Text="that", ID=7},
                new SourceExpression (){DirectionID=0, Text="here", ID=8},
                new SourceExpression (){DirectionID=0, Text="I", ID=9},
                new SourceExpression (){DirectionID=0, Text="write", ID=10},
            };
            return defTypesList;
        }*/

        public SourceExpression GetItemForId(int id)
        {
            Repository<SourceExpression> repos = new Repository<SourceExpression>();
            SourceExpression result = repos.GetItem(id);
            return result;
        }

        public IEnumerable<SourceExpression> GetSourceExpressionCollection(string sourceText, TranslateDirection direction)
        {
            //return db.Table<SourceExpression>().ToList().Where(item => item.Text == sourceText && item.DirectionID == direction.GetCurrentDirectionId());
            return db.Table<SourceExpression>().ToList().Where(item => item.Text == sourceText && item.LanguageFromID == direction.LanguageFrom.ID && item.LanguageToID == direction.LanguageTo.ID);
        }

    }
}

