using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TranslateHelper.Core.BL.Contracts
{
    public abstract class DataManager<T> where T:BL.Contracts.IBusinessEntity
    {
        public abstract void CreateDefaultData();
        public List<T> GetItems()
        {
            throw new Exception("not used");
            //DAL.Repository<T> repos = new TranslateHelper.Core.DAL.Repository<T>();
            //return new List<T>(repos.GetItems());
        }
    }
}