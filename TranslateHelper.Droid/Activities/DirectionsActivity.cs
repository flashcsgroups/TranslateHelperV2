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
        FunDirectionsAdapter adapterFunDirections;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            base.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            var languageManager = new LanguageManager(SqlLiteInstance.DB);
            var chatHistoryManager = new ChatHistoryManager(SqlLiteInstance.DB);
            var anecdoteManager = new AnecdoteManager(SqlLiteInstance.DB, languageManager);
            presenter = new DirectionsPresenter(this, new ChatManager(SqlLiteInstance.DB, languageManager, chatHistoryManager), languageManager, anecdoteManager);

            addTab(Resources.GetString(Resource.String.tab_recent_chats), Resource.Layout.DirectionsRecent, new Action(()=> presenter.SelectedRecentLanguagesEvent()));
            addTab(Resources.GetString(Resource.String.tab_all_languages), Resource.Layout.DirectionsAll, new Action(()=> presenter.SelectedAllLanguagesEvent(Locale.Default.Language)));
            addTab(Resources.GetString(Resource.String.tab_fun), Resource.Layout.FunDirectionsAll, new Action(() => presenter.SelectedListFunStoriesEvent()));

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
        public void updateListDirectionsOfStoryes(List<StoryWithTranslateItem> listDirectionsOfStories)
        {
            var listView = FindViewById<ListView>(Resource.Id.listFunAllDirections);
            listView.FastScrollEnabled = true;

            adapterFunDirections = new FunDirectionsAdapter(this, listDirectionsOfStories);
            listView.Adapter = adapterFunDirections;
            listView.ItemClick += ListView_StoryItemClick;

        }

        private void ListView_StoryItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            StoryWithTranslateItem selectedStory = adapterFunDirections.GetStoryItem(e.Position);
            startAnecdotesActivityBySelectedStory(selectedStory);
        }

        private void ListViewAllDirections_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Language selectedRobotLanguage = adapterAllDirections.GetLanguageItem(e.Position);
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
        private void startAnecdotesActivityBySelectedStory(StoryWithTranslateItem selectedStory)
        {
            var intent = new Intent(this, typeof(AnecdotesActivity));
            intent.PutExtra("languageFromId", selectedStory.LanguageFrom.ID);
            intent.PutExtra("languageToId", selectedStory.LanguageTo.ID);
            StartActivity(intent);
        }
    }
}