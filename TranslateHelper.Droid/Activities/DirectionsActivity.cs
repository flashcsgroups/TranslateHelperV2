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
using PortableCore.BL.Managers;
using Java.Util;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class DirectionsActivity : Activity, IDirectionsView
    {
        DirectionsPresenter presenter;
        DirectionsAllAdapter adapterAllDirections;
        DirectionsRecentAdapter adapterRecentDirections;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            base.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            var languageManager = new LanguageManager(SqlLiteInstance.DB);
            var chatHistoryManager = new ChatHistoryManager(SqlLiteInstance.DB);
            presenter = new DirectionsPresenter(this, new ChatManager(SqlLiteInstance.DB, languageManager, chatHistoryManager), languageManager);

            addTab("Recent", Resource.Layout.DirectionsRecent, new Action(()=> presenter.SelectedRecentLanguagesEvent()));
            addTab("All languages", Resource.Layout.DirectionsAll, new Action(()=> presenter.SelectedAllLanguagesEvent(Locale.Default.Language)));

        }

        protected override void OnStart()
        {
            base.OnStart();
            presenter.ShowRecentOrFullListLanguages(Locale.Default.Language);
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

        public void updateListAllLanguages(List<Language> listLanguage)
        {
            base.ActionBar.SelectTab(base.ActionBar.GetTabAt(1));
            var listView = FindViewById<ListView>(Resource.Id.listAllDirections);

            listView.FastScrollEnabled = true;

            adapterAllDirections = new DirectionsAllAdapter(this, listLanguage);
            listView.Adapter = adapterAllDirections;
            listView.ItemClick += ListViewAllDirections_ItemClick;
        }

        private void ListViewAllDirections_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Language selectedRobotLanguage = adapterAllDirections.GetLanguageItem(e.Position);
            //LanguageManager languageManager = new LanguageManager();
            //Chat chat = presenter.FoundExistingOrCreateChat(selectedRobotLanguage, presenter. languageManager.GetItemForShortName(Locale.Default.Language));
            //startChatActivityByChatId(chat.ID);
            int chatId = presenter.GetIdForExistOrCreatedChat(Locale.Default.Language, selectedRobotLanguage);
            startChatActivityByChatId(chatId);
        }

        public void updateListRecentDirections(List<DirectionsRecentItem> listDirectionsRecent)
        {
            base.ActionBar.SelectTab(base.ActionBar.GetTabAt(0));
            var listView = FindViewById<ListView>(Resource.Id.listRecentDirections);

            listView.FastScrollEnabled = true;

            adapterRecentDirections = new DirectionsRecentAdapter(this, listDirectionsRecent);
            listView.Adapter = adapterRecentDirections;
            listView.ItemClick += ListViewRecentDirections_ItemClick;
        }

        private void ListViewRecentDirections_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var recentChat = adapterRecentDirections.GetLanguageItem(e.Position);
            startChatActivityByChatId(recentChat.ChatId);
        }

        private void startChatActivityByChatId(int chatId)
        {
            var intent = new Intent(this, typeof(DictionaryChatActivity));
            intent.PutExtra("currentChatId", chatId);
            StartActivity(intent);
        }
    }
}