using System;
using TranslateHelper.Core.BL.Contracts;
using SQLite;

namespace TranslateHelper.Core
{
	public class Favorites : IBusinessEntity
	{
		public Favorites ()
		{
		}
		[PrimaryKey, AutoIncrement, Indexed]
		public int ID { get; set; }
		public int DeleteMark { get; set; }
		[Indexed]
		public int SourceExpressionID { get; set; }
	}
}
