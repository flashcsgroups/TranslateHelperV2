using System.Collections.Generic;

namespace TranslateHelper.Core.Bl.Contracts
{
    interface IDataManager<T>
    {
        void InitDefaultData();
        T GetItemForId(int Id);
    }
}