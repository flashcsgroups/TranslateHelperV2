using System.Collections.Generic;

namespace PortableCore.DL
{
    public interface ISQLiteTesting
    {
        IEnumerable<T> Table<T>() where T : BL.Contracts.IBusinessEntity, new();
    }
}