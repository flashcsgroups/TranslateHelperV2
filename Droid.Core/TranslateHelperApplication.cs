using System;
using Android.App;
using Android.Runtime;
using PortableCore.DL;
using PortableCore.DAL;
using System.Net;
using System.Net.Security;
using Android.Util;

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
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            var sqliteFilename = "TranslateHelperV21.db3";
            string libraryPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = System.IO.Path.Combine(libraryPath, sqliteFilename);
            SqlLiteHelper sqlConnection = new SqlLiteHelper(path);          
            SqlLiteInstance sqlInstance = new SqlLiteInstance(sqlConnection);
            sqlInstance.InitTables();
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; }); 
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            string tag = "TranslateHelper";
            Log.Error(tag, e.Exception.Message);
        }
    }
}