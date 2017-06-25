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
using Android;
using PortableCore.BL.Managers;
using System.Collections.Generic;
using Droid.Core.Helpers;

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
            var timeStart = DateTime.Now;
            HockeyAppCrashHelper.Register(this);
            HockeyAppMetricsHelper.Register(this);
            ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback((sender, certificate, chain, policyErrors) => { return true; });
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
            string libraryPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(libraryPath, sqliteFilename);
            SqlLiteHelper sqlConnection = new SqlLiteHelper(path);
            SqlLiteInstance sqlInstance = new SqlLiteInstance(sqlConnection);
            if(initTablesInNewDB)
                sqlInstance.InitTables(libraryPath);

            LanguageManager languageManager = new LanguageManager(sqlConnection);
            AnecdoteManager anecdoteManager = new AnecdoteManager(sqlConnection, languageManager);
            anecdoteManager.ClearAnecdoteTable();
            var listStories = anecdoteManager.GetListDirectionsForStories();
            foreach(var item in listStories)
            {
                string fullPath = Path.Combine(libraryPath, item.SourceFileName);
                copyAnecdotesFile(fullPath, item.SourceFileName);
                anecdoteManager.LoadDataFromString(item, File.ReadAllText(fullPath));
            }
            int onCreateDelay = (DateTime.Now - timeStart).Milliseconds;
            HockeyAppMetricsHelper.TrackOperationDurationEvent("onCreate", onCreateDelay);
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

        private void AndroidEnvironment_UnhandledExceptionRaiser(object sender, RaiseThrowableEventArgs e)
        {
            string tag = "TranslateHelperV2";
            Log.Error(tag, e.Exception.Message);
        }
    }
}