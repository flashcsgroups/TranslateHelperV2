using System;
using System.Collections.Generic;
using PortableCore.BL.Contracts;
using PortableCore.DL;

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
        }

        public IEnumerable<T> GetItems ()
		{
            return db.GetItems<T> ();
        }

        public int Save (T item)
		{
            return db.SaveItem<T> (item);
        }

        public int Insert(T item)
        {
            return db.InsertOrReplace(item);
        }

        public void DeleteAllDataInTable()
        {
            db.DeleteAll<T>();
        }

		public void SaveItemsInTransaction (IEnumerable<T> items)
		{
            try
            {
				foreach (var item in items) {
                    db.SaveItem<T> (item);
                }
            } catch (Exception E) {
				throw new Exception (E.Message, E.InnerException);
			}
		}

        public void AddItemsInTransaction(IEnumerable<T> items)
        {
            try
            {
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
        }

        public int Count()
        {
            return db.Count<T>();
        }

    }
}

