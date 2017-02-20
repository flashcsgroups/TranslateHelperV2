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
        private int currentChatId;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			ActionBar.SetDisplayHomeAsUpEnabled (true);
			ActionBar.SetHomeButtonEnabled (true);
			SetContentView (Resource.Layout.Favorites);
		}
        protected override void OnStart()
        {
            base.OnStart();
            currentChatId = Intent.GetIntExtra("currentChatId", -1);
            if (currentChatId >= 0)
            {
                presenter = new FavoritesPresenter(this, SqlLiteInstance.DB, currentChatId);
                presenter.Init();
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
        
        public override bool OnOptionsItemSelected (IMenuItem item)
		{
			switch (item.ItemId)
            {
                case Resource.Id.selectTestLevel:
                    var intent = new Intent(this, typeof(SelectTestLevelActivity));
                    intent.PutExtra("currentChatId", currentChatId);
                    StartActivity(intent);
                    return true;
                case global::Android.Resource.Id.Home:
                    var intentDictActivity = new Intent(this, typeof(DictionaryChatActivity));
                    intentDictActivity.PutExtra("currentChatId", currentChatId);
                    StartActivity(intentDictActivity);
                    return true;
                default:
                    break;
			}
            return base.OnOptionsItemSelected(item);
        }
    }
}

