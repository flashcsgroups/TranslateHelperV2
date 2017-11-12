using System.Linq;
using System.Collections.Generic;
using PortableCore.DL;
using SQLite;
using PortableCore.BL.Managers;
using System;

namespace PortableCore.DL
{
    public class SqlLiteHelper: SQLiteConnection, ISQLiteTesting
    {
		static object locker = new object ();

        public SqlLiteHelper (string PathToDBfile) : base (PathToDBfile)
		{
        }

        public IEnumerable<T> GetItems<T>() where T : BL.Contracts.IBusinessEntity, new()
        {
            lock (locker)
            {
                return (from i in Table<T>() select i).ToList();
            }
        }

        public T GetItem<T>(int id) where T : BL.Contracts.IBusinessEntity, new()
        {
            lock (locker)
            {
                var lst = (from i in Table<T>() select i).ToList().Where(i=>i.ID>=id&&i.ID<id+1);//грязный обход ошибки Xamarin
                //var ttt = 1;
                return lst.FirstOrDefault<T>();
                //Ошибка Xamarin 4.7, лучше не использовать данный метод совсем
                //return Table<T>().FirstOrDefault(x => x.ID == id);
            }
        }

        public int SaveItem<T>(T item) where T : BL.Contracts.IBusinessEntity
        {
            lock (locker)
            {
                if (item.ID != 0)
                {
                    Update(item);
                    return item.ID;
                }
                else {
                    return Insert(item);
                }
            }
        }

        public int InsertItem<T>(T item) where T : BL.Contracts.IBusinessEntity
        {
            lock (locker)
            {
                return Insert(item);
            }
        }

        public int DeleteItem<T>(int id) where T : BL.Contracts.IBusinessEntity, new()
        {
            lock (locker)
            {
                return Delete<T>(id);
            }
        }

        public int Count<T>() where T : BL.Contracts.IBusinessEntity, new()
        {
            lock (locker)
            {
                return Table<T>().Count();
            }
        }

        IEnumerable<T> ISQLiteTesting.Table<T>() 
        {
            return Table<T>();
        }
    }
}