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
            /*database = conn;
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
            */
        }

        /*public static void InitConnection(SQLiteConnection conn)
        {
            database = conn;
        }*/

        /*public void InitTables()
        {
            CreateTable<Language>();
            CreateTable<TranslateProvider>();
            CreateTable<Direction>();
            CreateTable<TranslateProvider>();
            CreateTable<Favorites>();
            CreateTable<SourceExpression>();
            CreateTable<TranslatedExpression>();
            CreateTable<DefinitionTypes>();

            DefinitionTypesManager managerTypes = new DefinitionTypesManager();
            managerTypes.InitDefaultData();

            TranslateProviderManager managerProvider = new TranslateProviderManager();
            managerProvider.InitDefaultData();
        }*/

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
                return Delete<T>(new T() { ID = id });
            }
        }

        IEnumerable<T> ISQLiteTesting.Table<T>() 
        {
            return Table<T>();
        }
    }
}