using System;
using System.Collections.Generic;
using System.IO;
using ScrumHelper.BL;

namespace TranslateHelper.DAL
{
    class SqlLiteInstance
    {
        public static DL.HelperDatabase DB
        {
            get
            {
                return db;
            }
        }

        private static DL.HelperDatabase db = null;
        protected static string dbLocation;

        static SqlLiteInstance()
        {
            // set the db location
            dbLocation = DatabaseFilePath;

            // instantiate the database 
            db = new ScrumHelper.DL.HelperDatabase(dbLocation);

        }

        public static string DatabaseFilePath
        {
            get
            { 
                var sqliteFilename = "TranslateHelper.db3";

                #if NETFX_CORE
                var path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, sqliteFilename);
                #else

                #if SILVERLIGHT
                // Windows Phone expects a local path, not absolute
                var path = sqliteFilename;
                #else

                #if __ANDROID__
                // Just use whatever directory SpecialFolder.Personal returns
                string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                #else
                // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = Path.Combine (documentsPath, "../Library/"); // Library folder
                #endif
                var path = Path.Combine(libraryPath, sqliteFilename);
                #endif      

                #endif
                return path;    
            }
        }
    }

    class Repository<T> where T:BL.Contracts.IBusinessEntity, new()
    {
        public Repository()
        {
        
        }

        public T GetItem(int id)
        {
            return SqlLiteInstance.DB.GetItem <T>(id);
        }

        public IEnumerable<T> GetItems()
        {

            return SqlLiteInstance.DB.GetItems<T>();
        }

        public int Save(T item)
        {
            return SqlLiteInstance.DB.SaveItem<T>(item);
        }

        public int Delete(int id)
        {
            return SqlLiteInstance.DB.DeleteItem<T>(id);
        }
    }
}

