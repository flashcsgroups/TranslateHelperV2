using System;
using System.Linq;
using ScrumHelper.BL;
using System.Collections.Generic;
using ScrumHelper.DL.SQLite;

namespace TranslateHelper.DL
{
    /// <summary>
    /// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the Task DB.
    /// It contains methods for retrieval and persistance as well as db creation, all based on the 
    /// underlying ORM.
    /// </summary>
    public class SqlLiteHelper : SQLiteConnection
    {
        static object locker = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
        /// if the database doesn't exist, it will create the database and all the tables.
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
		public SqlLiteHelper(string path) : base(path)
        {
            // create the tables
            CreateTable<Project>();
            //CreateTable<Iteration> ();
            CreateTable<Employee>();
        }

        public IEnumerable<T> GetItems<T>() where T : BL.Contracts.IBusinessEntity, new()
        {
            lock (locker)
            {
                //return (from i in Table<T> ().Where(i=>i.ID > 8) select i).ToList ();
                return Table<T>().ToList().Where(item => item.DeleteMark == 0);
            }
        }

        public T GetItem<T>(int id) where T : BL.Contracts.IBusinessEntity, new()
        {
            lock (locker)
            {
                return Table<T>().FirstOrDefault(x => x.ID == id);
                // Following throws NotSupportedException - thanks aliegeni
                //return (from i in Table<T> ()
                //        where i.ID == id
                //        select i).FirstOrDefault ();
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
                else
                {
                    return Insert(item);
                }
            }
        }

        public int DeleteItem<T>(int id) where T : BL.Contracts.IBusinessEntity, new()
        {
            lock (locker)
            {
                var item = Table<T>().FirstOrDefault(x => x.ID == id);
                item.DeleteMark = 1;
                if (item.ID != 0)
                {
                    Update(item);
                    return item.ID;
                }
                else
                {
                    return Insert(item);
                }
            }
        }
    }
}