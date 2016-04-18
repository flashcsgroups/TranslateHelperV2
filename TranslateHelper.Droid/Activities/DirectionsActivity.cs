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

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class DirectionsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            base.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            addTab("Recent", Resource.Layout.DirectionsRecent, initLayerRecent);
            addTab("All languages", Resource.Layout.DirectionsAll, initLayerAllLanguages);

        }

        private void addTab(string caption, int layout, Action initLayer)
        {
            ActionBar.Tab tab = ActionBar.NewTab();
            tab.SetText(caption);
            tab.TabSelected += (sender, args) =>
            {
                SetContentView(layout);
                initLayer();
            };
            base.ActionBar.AddTab(tab);
        }

        private void initLayerRecent()
        {
            /*var listView = FindViewById<ListView>(Resource.Id.listFavoritesListView);

            listView.FastScrollEnabled = true;

            translateResultIdxCollection = getItemsForFavoritesList();

            var sortedContacts = translateResultIdxCollection.GetSortedData();
            var adapter = CreateAdapter(sortedContacts);
            listView.Adapter = adapter;
            listView.ItemLongClick += adapter.ListItemLongClick;*/
        }

        private void initLayerAllLanguages()
        {
            var listView = FindViewById<ListView>(Resource.Id.listAllDirections);

            listView.FastScrollEnabled = true;

            //translateResultIdxCollection = getItemsForFavoritesList();

            var sortedContacts = translateResultIdxCollection.GetSortedData();
            var adapter = CreateAdapter(sortedContacts);
            listView.Adapter = adapter;
            listView.ItemLongClick += adapter.ListItemLongClick;
        }
    }
}