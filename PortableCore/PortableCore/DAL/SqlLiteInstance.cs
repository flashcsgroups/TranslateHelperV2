using PortableCore.Core.DL;
using PortableCore.DL;
using PortableCore.DL.SQLiteBase;
using System;
using System.Collections.Generic;
using System.IO;

namespace PortableCore.Core.DAL
{
    public class SqlLiteInstance
    {
        public static PortableCore.Core.DL.SqlLiteHelper DB
        {
            get
            {
                return db;
            }
        }

        private static PortableCore.Core.DL.SqlLiteHelper db = null;
        //protected static string dbLocation;

        public SqlLiteInstance(SqlLiteHelper sqlInstanceHelper)
        {
            db = sqlInstanceHelper;

        /*Core.DefinitionTypesManager managerTypes = new Core.DefinitionTypesManager();
        managerTypes.InitDefaultData ();

        Core.TranslateProviderManager managerProvider = new Core.TranslateProviderManager ();
        managerProvider.InitDefaultData ();*/
    }

    }
}