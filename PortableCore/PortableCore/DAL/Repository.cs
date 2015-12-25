using System;
using System.Collections.Generic;
using PortableCore.BL.Contracts;
using PortableCore.Core.DL;

namespace PortableCore.DAL
{
	class Repository<T> where T: IBusinessEntity, new()
	{
		public Repository()
		{
        
		}

		public T GetItem (int id)
		{
            //return SqlLiteInstance.DB.GetItem <T> (id);
            return SqlLiteHelper.GetItem<T>(id);
        }

        public IEnumerable<T> GetItems ()
		{
            //return SqlLiteInstance.DB.GetItems<T> ();
            return SqlLiteHelper.GetItems<T>();
        }

        public int Save (T item)
		{
            //return SqlLiteInstance.DB.SaveItem<T> (item);
            return SqlLiteHelper.SaveItem<T>(item);
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
                    SqlLiteHelper.SaveItem<T> (item);
                    //SqlLiteInstance.DB.SaveItem<T>(item);
                }
            } catch (Exception E) {
				throw new Exception (E.Message, E.InnerException);
			}
		}

        public void AddItemsInTransaction(IEnumerable<T> items)
        {
            throw new Exception("Not realized");
            /*try
            {
                //SqlLiteInstance.DB.BeginTransaction();
                SqlLiteHelper.BeginTransaction();
                foreach (var item in items)
                {
                    SqlLiteHelper.InsertItem<T>(item);
                }
                SqlLiteHelper.Commit();
            }
            catch (Exception E)
            {
                SqlLiteHelper.Rollback ();
                throw new Exception(E.Message, E.InnerException);
            }*/
        }

        public int Delete (int id)
		{
            //return SqlLiteInstance.DB.DeleteItem<T> (id);
            return SqlLiteHelper.DeleteItem<T>(id);
        }

    }
}

