using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.DL
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
		public int TranslatedExpressionID { get; set; }
        [Indexed]
        public int DirectionID { get; set; }
        [Indexed]
        public int ChatHistoryID { get; set; }//ссылка на ид в истории чата, избранное только из него добавляется
    }
}
