using System;
using Android.App;
using Android.Runtime;
using PortableCore.DL;
using PortableCore.DAL;
using System.Net;
using System.Net.Security;
using Android.Util;
using System.IO;
using System.Reflection;

namespace TranslateHelper.App
{
    [Application]
    public class TranslateHelperApplication : Application
    {
        public static TranslateHelperApplication CurrentInstance { get; private set; }
        private string sqliteFilename = "TranslateHelperV21.db3";

        protected TranslateHelperApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            CurrentInstance = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            initDb(sqliteFilename);
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });
        }

        private static void initDb(string sqliteFilename)
        {
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(libraryPath, sqliteFilename);
            if (!File.Exists(path))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string[] resources = assembly.GetManifestResourceNames();
                foreach (string resource in resources)
                {
                    if (resource.EndsWith(sqliteFilename))
                    {
                        copyInitDbFromResourceToFileSystem(path, assembly, resource);
                    }
                }
            }
            SqlLiteHelper sqlConnection = new SqlLiteHelper(path);
            SqlLiteInstance sqlInstance = new SqlLiteInstance(sqlConnection);
        }

        private static void copyInitDbFromResourceToFileSystem(string path, Assembly assembly, string resource)
        {
            Stream stream = assembly.GetManifestResourceStream(resource);
            using (FileStream fileStream = File.Create(path))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            string tag = "TranslateHelper";
            Log.Error(tag, e.Exception.Message);
        }
    }
}