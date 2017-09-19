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
        IdiomDirectionsAdapter adapterIdiomDirections;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            var languageManager = new LanguageManager(SqlLiteInstance.DB);
            var chatHistoryManager = new ChatHistoryManager(SqlLiteInstance.DB);
            var anecdoteManager = new AnecdoteManager(SqlLiteInstance.DB, languageManager);
            var idiomManager = new IdiomManager(SqlLiteInstance.DB, languageManager);
            presenter = new DirectionsPresenter(this, new ChatManager(SqlLiteInstance.DB, languageManager, chatHistoryManager), languageManager, anecdoteManager, idiomManager);
        }

        protected override void OnStart()
        {
            base.OnStart();
            var directionsLayoutType = Intent.GetIntExtra("DirectionLayoutType", -1);
            switch(directionsLayoutType)
            {
                case (int)DirectionsLayoutTypes.AllChats:
                    {
                        this.SetTitle(Resources.GetIdentifier("act_select_direction", "string", PackageName));
                        SetViewToFullListLanguages();
                    };break;
                case (int)DirectionsLayoutTypes.RecentChat:
                    {
                        SetContentView(Resource.Layout.DirectionsRecent);
                        this.SetTitle(Resources.GetIdentifier("act_select_direction", "string", PackageName));
                        presenter.ShowRecentListLanguages(Locale.Default.Language);
                    }
                    break;
                case (int)DirectionsLayoutTypes.Anecdotes:
                    {
                        SetContentView(Resource.Layout.FunDirectionsAll);
                        this.SetTitle(Resources.GetIdentifier("act_select_direction", "string", PackageName));
                        presenter.SelectedListFunStoriesEvent();
                    }
                    break;
                case (int)DirectionsLayoutTypes.Idioms:
                    {
                        SetContentView(Resource.Layout.IdiomsDirectionsAll);
                        this.SetTitle(Resources.GetIdentifier("act_select_direction", "string", PackageName));
                        presenter.SelectedIdiomsDirectionsListEvent();
                    }
                    break;
                default:
                    {
                        goToMainScreen();

                    }; break;
            }
        }

        private void goToMainScreen()
        {
            var intent = new Intent(this, typeof(MainScreenActivity));
            StartActivity(intent);
        }

        public void UpdateListAllLanguages(List<Language> listLanguage)
        {
            var listView = FindViewById<ListView>(Resource.Id.listAllDirections);

            listView.FastScrollEnabled = true;

            adapterAllDirections = new DirectionsAllAdapter(this, listLanguage);
            listView.Adapter = adapterAllDirections;
            listView.ItemClick += ListViewAllDirections_ItemClick;
        }
        public void UpdateListDirectionsOfStoryes(List<DirectionAnecdoteItem> listDirectionsOfStories)
        {
            var listView = FindViewById<ListView>(Resource.Id.listFunAllDirections);
            listView.FastScrollEnabled = true;

            adapterFunDirections = new FunDirectionsAdapter(this, listDirectionsOfStories);
            listView.Adapter = adapterFunDirections;
            listView.ItemClick += ListView_StoryItemClick;

        }
        public void UpdateListDirectionsOfIdioms(List<DirectionIdiomItem> listDirectionsOfIdioms)
        {
            var listView = FindViewById<ListView>(Resource.Id.listIdiomsAllDirections);
            listView.FastScrollEnabled = true;

            adapterIdiomDirections = new IdiomDirectionsAdapter(this, listDirectionsOfIdioms);
            listView.Adapter = adapterIdiomDirections;
            listView.ItemClick += ListView_IdiomDirectionItemClick; ;

        }

        private void ListView_IdiomDirectionItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            DirectionIdiomItem selectedItem = adapterIdiomDirections.GetListItem(e.Position);
            startIdiomsActivityByDirection(selectedItem);
        }

        private void ListView_StoryItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            DirectionAnecdoteItem selectedStory = adapterFunDirections.GetListItem(e.Position);
            startAnecdotesActivityBySelectedStory(selectedStory);
        }

        private void ListViewAllDirections_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Language selectedRobotLanguage = adapterAllDirections.GetLanguageItem(e.Position);
            int chatId = presenter.GetIdForExistOrCreatedChat(Locale.Default.Language, selectedRobotLanguage);
            StartChatActivityByChatId(chatId);
        }

        public void UpdateListRecentDirections(List<DirectionsRecentItem> listDirectionsRecent)
        {
            var listView = FindViewById<ListView>(Resource.Id.listRecentDirections);

            listView.FastScrollEnabled = true;

            adapterRecentDirections = new DirectionsRecentAdapter(this, listDirectionsRecent);
            listView.Adapter = adapterRecentDirections;
            listView.ItemClick += ListViewRecentDirections_ItemClick;
        }
        public void SetViewToFullListLanguages()
        {
            SetContentView(Resource.Layout.DirectionsAll);
            presenter.ShowFullListLanguages(Locale.Default.Language);
        }

        private void ListViewRecentDirections_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            var recentChat = adapterRecentDirections.GetLanguageItem(e.Position);
            StartChatActivityByChatId(recentChat.ChatId);
        }

        public void StartChatActivityByChatId(int chatId)
        {
            var intent = new Intent(this, typeof(DictionaryChatActivity));
            intent.PutExtra("currentChatId", chatId);
            StartActivity(intent);
        }
        private void startAnecdotesActivityBySelectedStory(DirectionAnecdoteItem selectedStory)
        {
            var intent = new Intent(this, typeof(AnecdotesActivity));
            intent.PutExtra("languageFromId", selectedStory.LanguageFrom.ID);
            intent.PutExtra("languageToId", selectedStory.LanguageTo.ID);
            StartActivity(intent);
        }
        private void startIdiomsActivityByDirection(DirectionIdiomItem directionItem)
        {
            var intent = new Intent(this, typeof(IdiomsActivity));
            intent.PutExtra("languageFromId", directionItem.LanguageFrom.ID);
            intent.PutExtra("languageToId", directionItem.LanguageTo.ID);
            StartActivity(intent);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    goToMainScreen();
                    return true;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}