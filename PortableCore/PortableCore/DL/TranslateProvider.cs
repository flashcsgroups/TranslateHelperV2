using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore.DL
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
