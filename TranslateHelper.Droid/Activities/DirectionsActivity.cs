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
using PortableCore.DAL;
using PortableCore.BL.Models;
using TranslateHelper.Droid.Adapters;
using PortableCore.BL.Presenters;
using PortableCore.BL.Views;
using PortableCore.DL;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class DirectionsActivity : Activity, IDirectionsView
    {
        DirectionsPresenter presenter;
        DirectionsAllAdapter adapterAllDirections;
        //RecentDirectionsPresenter recentDirectionsPresenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            base.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            presenter = new DirectionsPresenter(this, SqlLiteInstance.DB);

            addTab("Recent", Resource.Layout.DirectionsRecent, new Action(()=> presenter.SelectedRecentLanguagesEvent()));
            addTab("All languages", Resource.Layout.DirectionsAll, new Action(()=> presenter.SelectedAllLanguagesEvent()));

        }

        protected override void OnStart()
        {
            base.OnStart();
            base.ActionBar.SelectTab(base.ActionBar.GetTabAt(1));
        }

        private void addTab(string caption, int layout, Action tabSelectedAction)
        {
            ActionBar.Tab tab = ActionBar.NewTab();
            tab.SetText(caption);
            tab.TabSelected += (sender, args) =>
            {
                SetContentView(layout);
                tabSelectedAction();
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

        public void updateListAllLanguages(List<Language> listLanguage)
        {
            var listView = FindViewById<ListView>(Resource.Id.listAllDirections);

            listView.FastScrollEnabled = true;

            adapterAllDirections = new DirectionsAllAdapter(this, listLanguage);
            listView.Adapter = adapterAllDirections;
            listView.ItemClick += ListViewAllDirections_ItemClick;
        }

        private void ListViewAllDirections_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Language selectedLanguageItem = adapterAllDirections.GetLanguageItem(e.Position);
            Chat chat = presenter.FoundExistingOrCreateChat(selectedLanguageItem);
            var intent = new Intent(this, typeof(DictionaryChatActivity));
            intent.PutExtra("SelectedChatID", chat.ID);
            StartActivity(intent);
        }

        /*private void ListViewAllLang_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Toast.MakeText(this, e.Id.ToString(), ToastLength.Long).Show();
            
        }*/

        public void updateListRecentDirections(List<Tuple<Language, Language>> listDirections)
        {
            var listView = FindViewById<ListView>(Resource.Id.listRecentDirections);

            listView.FastScrollEnabled = true;

            listView.Adapter = new DirectionsRecentAdapter(this, listDirections);
        }
    }
}