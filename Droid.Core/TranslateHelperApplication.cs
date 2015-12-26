using System;
using Android.App;
using Android.Runtime;
using PortableCore.Core.DL;
using PortableCore.Core.DAL;

namespace TranslateHelper.App
{
    [Application]
    public class TranslateHelperApplication : Application
    {
        public static TranslateHelperApplication CurrentInstance { get; private set; }

        protected TranslateHelperApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            CurrentInstance = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            var sqliteFilename = "TranslateHelperV21.db3";
            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = System.IO.Path.Combine(libraryPath, sqliteFilename);
            SqlLiteHelper sqlConnection = new SqlLiteHelper(path);          
            SqlLiteInstance sqlInstance = new SqlLiteInstance(sqlConnection);
            sqlInstance.InitTables();
        }
    }
}