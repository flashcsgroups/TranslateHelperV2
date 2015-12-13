using System;
using TranslateHelper.Core.BL.Contracts;
using SQLite;

namespace TranslateHelper.Core
{
	public class TranslateProvider : IBusinessEntity
	{
		public TranslateProvider ()
		{
		}
		[PrimaryKey]
		public int ID { get; set; }
		public int DeleteMark { get; set; }
        [Indexed]
		public string Name { get; set; }
	}
}
