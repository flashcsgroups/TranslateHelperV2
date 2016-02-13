using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortableCore.BL.Contracts
{
    public abstract class DataManager<T> where T:BL.Contracts.IBusinessEntity
    {
        public abstract void CreateDefaultData();
        public List<T> GetItems()
        {
            throw new Exception("not used");
            //DAL.Repository<T> repos = new PortableCoreDAL.Repository<T>();
            //return new List<T>(repos.GetItems());
        }
    }
}