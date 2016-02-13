using System.Collections.Generic;

namespace PortableCore.BL.Contracts
{
    interface IDataManager<T>
    {
        void InitDefaultData();
        T GetItemForId(int Id);
    }
}