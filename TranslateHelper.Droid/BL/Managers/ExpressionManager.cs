using System;
using System.Collections.Generic;

namespace TranslateHelper.Core
{
	public class ExpressionManager
	{
		public ExpressionManager ()
		{
		}
		public void SaveTranslatedWord(string originalWord, string translatedWord)
		{
			DAL.Repository<SourceExpression> repos = new TranslateHelper.Core.DAL.Repository<SourceExpression> ();
			SourceExpression item = new SourceExpression ();
			item.Expression = originalWord;
			int savedSourceItem = repos.Save (item);

			DAL.Repository<TranslatedExpression> reposTranslated = new TranslateHelper.Core.DAL.Repository<TranslatedExpression> ();
			TranslatedExpression translatedItem = new TranslatedExpression ();
			translatedItem.Value = originalWord + " : " + translatedWord;
			translatedItem.SourceExpressionID = savedSourceItem;
			reposTranslated.Save (translatedItem);
		}

		public List<TranslatedExpression> GetTranslatedItems()
		{
			DAL.Repository<TranslatedExpression> repos = new TranslateHelper.Core.DAL.Repository<TranslatedExpression> ();
			return new List<TranslatedExpression> (repos.GetItems ());
		}
	}
}

