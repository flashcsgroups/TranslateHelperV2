using System;
using System.Collections.Generic;
using System.IO;

namespace PortableCore.Core.DAL
{
    class SqlLiteInstance
    {
        public static PortableCore.Core.DL.SqlLiteHelper DB
        {
            get
            {
                return db;
            }
        }

        private static PortableCore.Core.DL.SqlLiteHelper db = null;
        protected static string dbLocation;

        static SqlLiteInstance()
        {
            // set the db location
            dbLocation = DatabaseFilePath;

            // instantiate the database 
            db = new PortableCore.Core.DL.SqlLiteHelper(dbLocation);

            /*Core.DefinitionTypesManager managerTypes = new Core.DefinitionTypesManager();
            managerTypes.InitDefaultData ();

            Core.TranslateProviderManager managerProvider = new Core.TranslateProviderManager ();
            managerProvider.InitDefaultData ();*/
        }

        public static string DatabaseFilePath
        {
            get
            {
                var sqliteFilename = "TranslateHelper.db3";
                /*
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
                */
                //return path;
                return sqliteFilename;
            }
        }
    }
}