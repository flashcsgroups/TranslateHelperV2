using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using PortableCore.BL.Contracts;
using PortableCore.BL.Managers;
using PortableCore.DAL;
using TranslateHelper.Droid.Adapters;
using PortableCore.BL.Presenters;
using PortableCore.BL.Views;
using PortableCore.BL.Models;
using PortableCore.BL;

namespace TranslateHelper.Droid.Activities
{
    [Activity (Label = "@string/act_favorites_caption", Theme = "@style/MyTheme")]
	public class FavoritesActivity : Activity, IFavoritesView
	{
        FavoritesPresenter presenter;
        //TranslateDirection direction = new TranslateDirection(SqlLiteInstance.DB);
        //IndexedCollection<FavoritesItem> translateResultIdxCollection;
        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.SetHomeButtonEnabled (true);
			SetContentView (Resource.Layout.Favorites);

            //direction.SetDirection(Intent.GetStringExtra("directionName"));
            //throw new NotImplementedException("Нет больше Dictionary! Реализовать.");

            //var listView = FindViewById<ListView> (Resource.Id.listFavoritesListView);

			//listView.FastScrollEnabled = true;

			//translateResultIdxCollection = getItemsForFavoritesList ();

			//var sortedContacts = translateResultIdxCollection.GetSortedData ();
            //var adapter = CreateAdapter(sortedContacts);
            //listView.Adapter = adapter;
            //listView.ItemLongClick += adapter.ListItemLongClick;
		}
        protected override void OnStart()
        {
            base.OnStart();
            int selectedChatID = Intent.GetIntExtra("currentChatId", -1);
            if (selectedChatID >= 0)
            {
                presenter = new FavoritesPresenter(this, SqlLiteInstance.DB, selectedChatID);
                presenter.Init();
                //presenter.InitChat(Locale.Default.Language);
                //presenter.UpdateOldSuspendedRequests();
            }
            else
            {
                throw new Exception("Chat not found");
            }
        }
        public void UpdateFavorites(IndexedCollection<FavoriteItem> listFavorites)
        {
            var adapter = CreateAdapter(listFavorites.GetSortedData());
            var listView = FindViewById<ListView>(Resource.Id.listFavoritesListView);
            listView.FastScrollEnabled = true;
            listView.Adapter = adapter;
        }

        /*private IndexedCollection<FavoritesItem> getItemsForFavoritesList ()
		{
			//ToDo:Полный отстой. Во-первых, SQLite не поддерживает join - придется переписывать на обычный запрос, во вторых - запросы в цикле, в-третьих - выбирается вся таблица избранного, а не порция
			IndexedCollection<FavoritesItem> result = new IndexedCollection<FavoritesItem> ();
            var view = from favItem in SqlLiteInstance.DB.Table<Favorites> ()
			           join trItem in SqlLiteInstance.DB.Table<TranslatedExpression> () on favItem.TranslatedExpressionID equals trItem.ID into transExpr
			           from subTransExpr in transExpr.DefaultIfEmpty ()
			           select new Tuple<TranslatedExpression, Favorites> (subTransExpr, favItem);
			SourceExpressionManager sourceExprManager = new SourceExpressionManager (SqlLiteInstance.DB);
			SourceDefinitionManager sourceDefManager = new SourceDefinitionManager (SqlLiteInstance.DB);

			foreach (var item in view)
            {
                if((item.Item1!=null)&(item.Item2!=null))
                {
                    var sourceDefItem = sourceDefManager.GetItemForId(item.Item1.SourceDefinitionID);
                    var sourceExprItem = sourceExprManager.GetItemForId(sourceDefItem.SourceExpressionID);
                    if ((sourceDefItem != null) && (sourceExprItem != null))
                    {
                        result.Add(new FavoritesItem()
                        {
                            OriginalText = sourceExprItem.Text,
                            TranslatedText = item.Item1.TranslatedText,
                            TranslatedExpressionId = item.Item1.ID,
                            DefinitionType = (DefinitionTypesEnum)item.Item1.DefinitionTypeID,
                            OriginalTranscription = sourceDefItem.TranscriptionText,
                            FavoritesId = item.Item2.ID
                        });
                    }
                }
            }
            ChatHistoryManager chatHistoryManager = new ChatHistoryManager(dbHelper);
            result.Add(new FavoritesItem()
            {
                OriginalText = "original",
                TranslatedText = "translated",
                TranslatedExpressionId = 0,
                DefinitionType = 0,
                OriginalTranscription = "transcription",
                FavoritesId = 0
            });

            return result;
		}*/

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
            MenuInflater.Inflate(Resource.Menu.menu_FavoritesScreen, menu);
            return true;
		}

        /*
        public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
            {
                case Resource.Id.selectTestLevel:
                    var intent = new Intent(this, typeof(SelectTestLevelActivity));
                    intent.PutExtra("directionName", direction.GetCurrentDirectionName());
                    StartActivity(intent);
                    return true;
                case global::Android.Resource.Id.Home:
                    var intentDictActivity = new Intent(this, typeof(DictionaryChatActivity));
                    intentDictActivity.PutExtra("directionName", direction.GetCurrentDirectionName());
                    StartActivity(intentDictActivity);
                    return true;
                default:
                    break;
			}
            return base.OnOptionsItemSelected(item);
        }*/
    }
}

