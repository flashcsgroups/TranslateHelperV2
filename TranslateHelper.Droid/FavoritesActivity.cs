
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
using PortableCore;
using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using PortableCore.DAL;
using PortableCore.DL;

namespace TranslateHelper.Droid
{
	[Activity (Label = "@string/act_favorites_caption", Theme = "@style/MyTheme")]
	public class FavoritesActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.SetHomeButtonEnabled (true);
			SetContentView (Resource.Layout.Favorites);

			var listView = FindViewById<ListView> (Resource.Id.listFavoritesListView);

			listView.FastScrollEnabled = true;

			var translateResultIdxCollection = getItemsForFavoritesList ();

			var sortedContacts = translateResultIdxCollection.GetSortedData ();
            var adapter = CreateAdapter(sortedContacts);
            listView.Adapter = adapter;
            listView.ItemClick += adapter.ListItemClick;


		}

        private IndexedCollection<FavoritesItem> getItemsForFavoritesList ()
		{
			//ToDo:Полный отстой. Во-первых, SQLite не поддерживает join - придется переписывать на обычный запрос, во вторых - запросы в цикле, в-третьих - выбирается вся таблица избранного, а не порция
			IndexedCollection<FavoritesItem> result = new IndexedCollection<FavoritesItem> ();
			var view = from favItem in SqlLiteInstance.DB.Table<Favorites> ()
			           join trItem in SqlLiteInstance.DB.Table<TranslatedExpression> () on favItem.TranslatedExpressionID equals trItem.ID into transExpr
			           from subTransExpr in transExpr.DefaultIfEmpty ()
			           select new Tuple<TranslatedExpression, Favorites> (subTransExpr, favItem);
			SourceExpressionManager sourceExprManager = new SourceExpressionManager (SqlLiteInstance.DB);
			SourceDefinitionManager sourceDefManager = new SourceDefinitionManager (SqlLiteInstance.DB);

			foreach (var item in view) {
				var sourceDefItem = sourceDefManager.GetItemForId (item.Item1.SourceDefinitionID);
				var sourceExprItem = sourceExprManager.GetItemForId (sourceDefItem.SourceExpressionID);
				result.Add (new FavoritesItem () {
					OriginalText = sourceExprItem.Text,
					TranslatedText = item.Item1.TranslatedText,
					TranslatedExpressionId = item.Item1.ID,
					DefinitionType = (DefinitionTypesEnum)item.Item1.DefinitionTypeID,
					OriginalTranscription = sourceDefItem.TranscriptionText,
					FavoritesId = item.Item2.ID
				});
			}
			return result;
		}

		FavoritesAdapter CreateAdapter<T> (Dictionary<string, List<T>> sortedObjects) where T : IHasLabel, IComparable<T>
		{
			var adapter = new FavoritesAdapter (this);
			foreach (var e in sortedObjects.OrderBy(de => de.Key)) {
				var section = e.Value;
				var label = e.Key.Trim ().Length > 0 ? e.Key.ToUpper () : "Error:" + " (" + section.Count.ToString () + ")";
				adapter.AddSection (label, new ArrayAdapter<T> (this, Resource.Layout.FavoritesSectionListItem, Resource.Id.SourceTextView, section));
			}
			return adapter;
		}

		public override bool OnCreateOptionsMenu (IMenu menu)
		{
			//MenuInflater.Inflate(Resource.Menu.menu_FavoritesScreen, menu);

			/*IMenuItem menuItem = menu.FindItem(Resource.Id.menu_delete_task);
            menuItem.SetTitle(task.ID == 0 ? "Cancel" : "Delete");
            */
			return true;
		}

		public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId) {
			/*case Resource.Id.menu_save_task:
                    Save();
                    return true;

                case Resource.Id.menu_delete_task:
                    CancelDelete();
                    return true;
                    */
			default:
				Finish ();
				return base.OnOptionsItemSelected (item);
			}
		}
	}
}

