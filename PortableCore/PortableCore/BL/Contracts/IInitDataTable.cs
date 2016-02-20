using System.Collections.Generic;

namespace PortableCore.BL.Contracts
{
    interface IInitDataTable<T>
    {
        /// <summary>
        /// Метод инициализации дефолтных данных для таблицы
        /// </summary>
        void InitDefaultData();
        //T GetItemForId(int Id);
    }
}