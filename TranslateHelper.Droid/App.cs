using System;

using Android.App;
using Android.Runtime;
using PortableCore.Core.DL;

namespace TranslateHelper.Droid
{
    [Application]
    class App : Application
    {
        //SqlLiteHelper conn;

        protected App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            var sqliteFilename = "TranslateHelperV2.db3";
            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = System.IO.Path.Combine(libraryPath, sqliteFilename);
            //SqlLiteInstance dbconn = new SqlLiteInstance(new PortableCore.DL.SQLite.Connection(path));
            //SqlLiteHelper dbconn = new SqlLiteHelper(new PortableCore.DL.SQLite.Connection(path));
            SqlLiteHelper.InitConnection(new PortableCore.DL.SQLite.Connection(path));
            SqlLiteHelper.InitTables();
            //SqlLiteInstance sqlInstance = new SqlLiteInstance(new PortableCore.DL.SQLite.Connection(path));
            //SqlLiteInstance.DB.CreateTable<Language>();
            //var sql = new SqlLiteHelper(path);
            /*var sql = new SQLiteConnection(path);
            sql.CreateTable<Language>();*/
        }
    }
}