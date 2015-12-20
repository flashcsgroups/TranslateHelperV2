using System;

namespace PortableCore.BL.Contracts
{
	public interface IBusinessEntity
	{
		int ID { get; set; }
		int DeleteMark { get; set; }
	}
}

