using System;
using System.Collections.Generic;
using System.IO;
using PortableCore.BL.Contracts;

namespace PortableCore.DAL
{
	class Repository<T> where T: IBusinessEntity, new()
	{
		public Repository ()
		{
        
		}

		public T GetItem (int id)
		{
            throw new Exception("not realized");
            //return SqlLiteInstance.DB.GetItem <T> (id);
		}

		public IEnumerable<T> GetItems ()
		{
            throw new Exception("not realized");
            //return SqlLiteInstance.DB.GetItems<T> ();
		}

		public int Save (T item)
		{
            throw new Exception("not realized");
            //return SqlLiteInstance.DB.SaveItem<T> (item);
		}

        public void DeleteAllDataInTable()
        {
            throw new Exception("not realized");
            //SqlLiteInstance.DB.DeleteDataInTable<T>();
        }

		public void SaveItemsInTransaction (IEnumerable<T> items)
		{
            throw new Exception("not realized");

            /*try
            {
				foreach (var item in items) {
					SqlLiteInstance.DB.SaveItem<T> (item);
				}
			} catch (Exception E) {
				throw new Exception (E.Message, E.InnerException);
			}*/
		}

        public void AddItemsInTransaction(IEnumerable<T> items)
        {
            throw new Exception("not realized");
            /*try
            {
                SqlLiteInstance.DB.BeginTransaction();
                foreach (var item in items)
                {
                    SqlLiteInstance.DB.InsertItem<T>(item);
                }
                SqlLiteInstance.DB.Commit();
            }
            catch (Exception E)
            {
                SqlLiteInstance.DB.Rollback ();
                throw new Exception(E.Message, E.InnerException);
            }*/
        }

        public int Delete (int id)
		{
            throw new Exception("not realized");
            //return SqlLiteInstance.DB.DeleteItem<T> (id);
		}

	}
}

