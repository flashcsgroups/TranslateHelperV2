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
using HockeyApp.Android;
using Android;
using PortableCore.BL.Managers;

namespace TranslateHelper.App
{
    [Application]
    public class TranslateHelperApplication : Application
    {
        public static TranslateHelperApplication CurrentInstance { get; private set; }
        public string LastStringForTranslateFromClipboard = string.Empty;

        private static string sqliteFilename = "TranslateHelperV22.db3";
        private readonly bool initTablesInNewDB = true;

        protected TranslateHelperApplication(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
            CurrentInstance = this;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            CrashManager.Register(this, "1fa12db7cc804215bdd1a7542b3d1c96");
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(libraryPath, sqliteFilename);
            SqlLiteHelper sqlConnection = new SqlLiteHelper(path);
            SqlLiteInstance sqlInstance = new SqlLiteInstance(sqlConnection);
            if(initTablesInNewDB)
                sqlInstance.InitTables(libraryPath);

            string anecdoteFileName = "anecdotesEN_RUv1.txt";

            string fullPath = Path.Combine(libraryPath, anecdoteFileName);
            copyAnecdotesFile(fullPath, anecdoteFileName);
            AnecdoteManager managerAnecdotes = new AnecdoteManager(sqlConnection);
            managerAnecdotes.LoadDataFromString(File.ReadAllText(fullPath));

        }

        private static void copyAnecdotesFile(string fullPath, string filename)
        {
            if (!File.Exists(fullPath))
            {
                Assembly assembly = Assembly.GetExecutingAssembly();
                string[] resources = assembly.GetManifestResourceNames();                
                foreach (string resource in resources)
                {
                    if (resource.Contains(filename))
                    {
                        Stream stream = assembly.GetManifestResourceStream(resource);
                        using (FileStream fileStream = File.Create(fullPath))
                        {
                            stream.Seek(0, SeekOrigin.Begin);
                            stream.CopyTo(fileStream);
                        }
                    }
                }
            }
        }

        /*private static void copyInitDbFromResourceToFileSystem(string path, Assembly assembly, string resource)
        {
            Stream stream = assembly.GetManifestResourceStream(resource);
            using (FileStream fileStream = File.Create(path))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
        }*/
        /*private static void copyInitialDB(string path)
        {
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
        }

        private static void copyInitDbFromResourceToFileSystem(string path, Assembly assembly, string resource)
        {
            Stream stream = assembly.GetManifestResourceStream(resource);
            using (FileStream fileStream = File.Create(path))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
        }*/

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            string tag = "TranslateHelperV2";
            Log.Error(tag, e.Exception.Message);
        }
    }
}