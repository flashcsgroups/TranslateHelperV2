using System.Collections.Generic;

namespace TranslateHelper.Core.Bl.Contracts
{
    interface IDataManager<T>
    {
        void CreateDefaultData();
        T GetItemForId(int Id);
    }
}