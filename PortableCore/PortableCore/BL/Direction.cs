using System;
using PortableCore.BL.Contracts;
using SQLite;

namespace PortableCore
{
	public class Direction : IBusinessEntity
	{
		public Direction ()
		{
		}
		[PrimaryKey, AutoIncrement, Indexed]
		public int ID { get; set; }
		public int DeleteMark { get; set; }
		public string Name { get; set; }
		public int ProviderID { get; set; }
	}
}
