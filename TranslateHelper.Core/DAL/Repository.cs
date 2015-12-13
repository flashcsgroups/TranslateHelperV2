using System;
using System.Collections.Generic;
using System.IO;
using TranslateHelper.Core.BL;

namespace TranslateHelper.Core.DAL
{
	class Repository<T> where T:BL.Contracts.IBusinessEntity, new()
	{
		public Repository ()
		{
        
		}

		public T GetItem (int id)
		{
			return SqlLiteInstance.DB.GetItem <T> (id);
		}

		public IEnumerable<T> GetItems ()
		{

			return SqlLiteInstance.DB.GetItems<T> ();
		}

		public int Save (T item)
		{
			return SqlLiteInstance.DB.SaveItem<T> (item);
		}

        public void DeleteAllDataInTable()
        {
            SqlLiteInstance.DB.DeleteDataInTable<T>();
        }

		public void SaveItemsInTransaction (IEnumerable<T> items)
		{		

			//SqlLiteInstance.DB.SaveItem<T> (items[0])
			try {
				//SqlLiteInstance.DB.BeginTransaction();
				foreach (var item in items) {
					SqlLiteInstance.DB.SaveItem<T> (item);
				}
				//SqlLiteInstance.DB.Commit();
			} catch (Exception E) {
				//SqlLiteInstance.DB.Rollback ();
				throw new Exception (E.Message, E.InnerException);
			}
		}

        public void AddItemsInTransaction(IEnumerable<T> items)
        {
            try
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
            }
        }

        public int Delete (int id)
		{
			return SqlLiteInstance.DB.DeleteItem<T> (id);
		}

	}
}

