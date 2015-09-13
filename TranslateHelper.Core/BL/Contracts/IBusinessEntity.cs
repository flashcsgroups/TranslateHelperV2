using System;

namespace TranslateHelper.Core.BL.Contracts
{
	public interface IBusinessEntity
	{
		int ID { get; set; }

		int DeleteMark { get; set; }
	}
}

