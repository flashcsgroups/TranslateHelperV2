using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PortableCore.Core.DL;

namespace TranslateHelper.Droid
{
    /*class App : Application
    {
        SqlLiteHelper conn;

        protected App(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            var sqliteFilename = "TranslateHelper.db3";
            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = System.IO.Path.Combine(libraryPath, sqliteFilename);
            conn = new SqlLiteHelper(path);
        }
    }*/
}