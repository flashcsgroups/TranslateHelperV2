using System;
using TranslateHelper.Core.BL.Contracts;
using SQLite;

namespace TranslateHelper.Core
{
	public class SourceExpression : IBusinessEntity
	{
		public SourceExpression ()
		{
		}
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public int DeleteMark { get; set; }
		[Indexed]
		public string Expression { get; set; }
		[Indexed]
		public int DirectionID { get; set; }
	}
}
