using System;
using System.Collections.Generic;
using PortableCore.BL.Contracts;
using PortableCore.DL;
using PortableCore.DAL;

namespace PortableCore.DAL
{
	class Repository<T> where T: IBusinessEntity, new()
	{
        SqlLiteHelper db;

        public Repository()
		{
            db = SqlLiteInstance.DB;
        }

		public T GetItem (int id)
		{
            return db.GetItem <T> (id);
            //return SqlLiteHelper.GetItem<T>(id);
        }

        public IEnumerable<T> GetItems ()
		{
            return db.GetItems<T> ();
            //return SqlLiteHelper.GetItems<T>();
        }

        public int Save (T item)
		{
            return db.SaveItem<T> (item);
            //return SqlLiteHelper.SaveItem<T>(item);
        }

        public void DeleteAllDataInTable()
        {
            //SqlLiteInstance.DB.DeleteDataInTable<T>();
        }

		public void SaveItemsInTransaction (IEnumerable<T> items)
		{
            try
            {
				foreach (var item in items) {
                    db.SaveItem<T> (item);
                    //SqlLiteInstance.DB.SaveItem<T>(item);
                }
            } catch (Exception E) {
				throw new Exception (E.Message, E.InnerException);
			}
		}

        public void AddItemsInTransaction(IEnumerable<T> items)
        {
            //throw new Exception("Not realized");
            try
            {
                //SqlLiteInstance.DB.BeginTransaction();
                db.BeginTransaction();
                foreach (var item in items)
                {
                    db.InsertItem<T>(item);
                }
                db.Commit();
            }
            catch (Exception E)
            {
                db.Rollback ();
                throw new Exception(E.Message, E.InnerException);
            }
        }

        public int Delete (int id)
		{
            return db.DeleteItem<T> (id);
            //return SqlLiteHelper.DeleteItem<T>(id);
        }

    }
}

