using System.Linq;
using System.Collections.Generic;
using PortableCore.BL.Contracts;
using System;
using PortableCore.DL;
using PortableCore.BL.Managers;
using PortableCore.DL.SQLiteBase;

namespace PortableCore.Core.DL
{
	/// <summary>
	/// TaskDatabase builds on SQLite.Net and represents a specific database, in our case, the Task DB.
	/// It contains methods for retrieval and persistance as well as db creation, all based on the 
	/// underlying ORM.
	/// </summary>
	public static class SqlLiteHelper 
	{
		static object locker = new object ();
        public static SQLiteConnection database;
        /// <summary>
        /// Initializes a new instance of the <see cref="Tasky.DL.TaskDatabase"/> TaskDatabase. 
        /// if the database doesn't exist, it will create the database and all the tables.
        /// </summary>
        /// <param name='path'>
        /// Path.
        /// </param>
         /*static SqlLiteHelper (SQLiteConnection conn)
		{
            database = conn;
            database.CreateTable<Language>();
            CreateTable<TranslateProvider> ();
            CreateTable<Direction>();
            CreateTable<TranslateProvider> ();
			CreateTable<Favorites> ();
			CreateTable<SourceExpression> ();
            CreateTable<TranslatedExpression>();
            CreateTable<DefinitionTypes>();
            

            DefinitionTypesManager managerTypes = new DefinitionTypesManager();
            managerTypes.InitDefaultData ();

            TranslateProviderManager managerProvider = new TranslateProviderManager ();
            managerProvider.InitDefaultData ();

        }*/

        public static void InitConnection(SQLiteConnection conn)
        {
            database = conn;
        }

        public static void InitTables()
        {
            database.CreateTable<Language>();
            database.CreateTable<TranslateProvider>();
            database.CreateTable<Direction>();
            database.CreateTable<TranslateProvider>();
            database.CreateTable<Favorites>();
            database.CreateTable<SourceExpression>();
            database.CreateTable<TranslatedExpression>();
            database.CreateTable<DefinitionTypes>();
        }

        public static IEnumerable<T> GetItems<T>() where T : BL.Contracts.IBusinessEntity, new()
        {
            lock (locker)
            {
                return (from i in database.Table<T>() select i).ToList();
            }
        }

        public static T GetItem<T>(int id) where T : BL.Contracts.IBusinessEntity, new()
        {
            lock (locker)
            {
                return database.Table<T>().FirstOrDefault(x => x.ID == id);
                // Following throws NotSupportedException - thanks aliegeni
                //return (from i in Table<T> ()
                //        where i.ID == id
                //        select i).FirstOrDefault ();
            }
        }

        public static int SaveItem<T>(T item) where T : BL.Contracts.IBusinessEntity
        {
            lock (locker)
            {
                if (item.ID != 0)
                {
                    database.Update(item);
                    return item.ID;
                }
                else {
                    return database.Insert(item);
                }
            }
        }

        public static int DeleteItem<T>(int id) where T : BL.Contracts.IBusinessEntity, new()
        {
            lock (locker)
            {
                return database.Delete<T>(new T() { ID = id });
            }
        }
    }
}