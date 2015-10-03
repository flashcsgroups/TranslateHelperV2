using System;
using System.Collections.Generic;
using System.IO;
using TranslateHelper.Core.BL;

namespace TranslateHelper.Core.DAL
{
	class SqlLiteInstance
	{
		public static TranslateHelper.Core.DL.SqlLiteHelper DB {
			get {
				return db;
			}
		}

		private static TranslateHelper.Core.DL.SqlLiteHelper db = null;
		protected static string dbLocation;

		static SqlLiteInstance ()
		{
			// set the db location
			dbLocation = DatabaseFilePath;

			// instantiate the database 
			db = new TranslateHelper.Core.DL.SqlLiteHelper (dbLocation);

		}

		public static string DatabaseFilePath {
			get { 
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
				string libraryPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				#else
                // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                string documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = Path.Combine (documentsPath, "../Library/"); // Library folder
				#endif
				var path = Path.Combine (libraryPath, sqliteFilename);
				#endif      

				#endif
				return path;    
			}
		}
	}

	class Repository<T> where T:BL.Contracts.IBusinessEntity, new()
	{
		public Repository ()
		{
        
		}

		public T GetItem (int id)
		{
			return SqlLiteInstance.DB.GetItem <T> (id);
		}

		public IEnumerable<T> GetItems ()
		{

			return SqlLiteInstance.DB.GetItems<T> ();
		}

		public int Save (T item)
		{
			return SqlLiteInstance.DB.SaveItem<T> (item);
		}

		public void SaveItemsInTransaction (IEnumerable<T> items)
		{		

			//SqlLiteInstance.DB.SaveItem<T> (items[0])
			try {
				//SqlLiteInstance.DB.BeginTransaction();
				foreach (var item in items) {
					SqlLiteInstance.DB.SaveItem<T> (item);
				}
				//SqlLiteInstance.DB.Commit();
			} catch (Exception E) {
				//SqlLiteInstance.DB.Rollback ();
				throw new Exception (E.Message, E.InnerException);
			}
		}

		public int Delete (int id)
		{
			return SqlLiteInstance.DB.DeleteItem<T> (id);
		}
	}
}

