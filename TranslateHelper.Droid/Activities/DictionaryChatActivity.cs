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
using TranslateHelper.Droid.Adapters;
using PortableCore.BL.Presenters;
using PortableCore.BL.Views;
using PortableCore.DAL;
using PortableCore.BL.Models;
using Java.Util;
using Droid.Core.Helpers;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "Translate helper", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class DictionaryChatActivity : Activity, IDictionaryChatView
    {
        DictionaryChatPresenter presenter;
        private BubbleAdapter bubbleAdapter;
        private AlertDialog actionContextWindow;
        private int selectedMsgIndex = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            // Create your application here
            SetContentView(Resource.Layout.DictionaryChat);
            EditText editSourceText = FindViewById<EditText>(Resource.Id.textSourceString);

            ImageButton buttonTranslate = FindViewById<ImageButton>(Resource.Id.buttonTranslate);
            buttonTranslate.Click += (object sender, EventArgs e) =>
            {
                presenter.UserAddNewTextEvent(editSourceText.Text);
                editSourceText.Text = string.Empty;
            };

            ImageButton buttonSwapDirection = FindViewById<ImageButton>(Resource.Id.buttonSwapDirection);
            buttonSwapDirection.Click += (object sender, EventArgs e) =>
            {
                presenter.UserSwapDirection();
            };

            editSourceText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) =>
            {
                if ((e.Text.Count() > 0)&&(e.Text.Last() == '\n'))
                {
                    presenter.UserAddNewTextEvent(editSourceText.Text.TrimEnd());
                    editSourceText.Text = string.Empty;
                }
            };
           
            var listView = FindViewById<ListView>(Resource.Id.forms_centralfragments_chat_chat_listView);
            listView.ItemClick += (sender, args) =>
            {
                if ((actionContextWindow != null) && (actionContextWindow.IsShowing))
                {
                    actionContextWindow.Dismiss();
                }
                else
                {
                    selectedMsgIndex = args.Position;
                    actionContextWindow = CreateActionContextMenu(args.Position);
                    actionContextWindow.Show();
                }
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            int selectedChatID = Intent.GetIntExtra("currentChatId", -1);
            if(selectedChatID >= 0)
            {
                presenter = new DictionaryChatPresenter(this, SqlLiteInstance.DB, selectedChatID);
                presenter.InitDirection();
                presenter.InitChat(Locale.Default.Language);
                presenter.UpdateOldSuspendedRequests();
            }
            else
            {
                throw new Exception("Chat not found");
            }
        }

        public void UpdateChat(List<BubbleItem> listBubbles, int setPositionItemIndex)
        {
            ListView listView = getListItemView(listBubbles);
            listView.SetSelection(setPositionItemIndex);
        }

        private ListView getListItemView(List<BubbleItem> listBubbles)
        {
            var metrics = Resources.DisplayMetrics;
            var listView = FindViewById<ListView>(Resource.Id.forms_centralfragments_chat_chat_listView);
            bubbleAdapter = new BubbleAdapter(this, listBubbles, metrics);
            listView.Adapter = bubbleAdapter;
            listView.ChoiceMode = ChoiceMode.Single;
            return listView;
        }

        private AlertDialog CreateActionContextMenu(int positionOfSelectedItem)
        {
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleDropDownItem1Line, new string[] { "Favorite", "Delete", "Copy" });
            var builder = new AlertDialog.Builder(this);
            builder.SetAdapter(adapter, onSelectContextMenuItem);
            builder.SetCancelable(true);
            actionContextWindow = builder.Create();
            return actionContextWindow;
        }

        private void onSelectContextMenuItem(object sender, DialogClickEventArgs args)
        {
            switch (args.Which)
            {
                case 0:
                    {
                        actionContextWindow.Dismiss();
                        var item = bubbleAdapter.GetBubbleItemByIndex(selectedMsgIndex);
                        presenter.InvertFavoriteState(item, selectedMsgIndex);
                    }; break;
                case 1:
                    {
                        actionContextWindow.Dismiss();
                        var item = bubbleAdapter.GetBubbleItemByIndex(selectedMsgIndex);
                        int newItemIndex = selectedMsgIndex > 0 ? selectedMsgIndex - 1 : 0;
                        presenter.DeleteBubbleFromChat(item, newItemIndex);
                    }; break;
                case 2:
                    {
                        actionContextWindow.Dismiss();
                        var item = bubbleAdapter.GetBubbleItemByIndex(selectedMsgIndex);
                        var clipboard = (ClipboardManager)GetSystemService(ClipboardService);
                        ClipData clip = ClipData.NewPlainText("TranslateHelper", item.IsRobotResponse?item.TextTo: item.TextFrom);
                        clipboard.PrimaryClip = clip;
                    }; break;
                default:
                    {

                    }; break;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_ChatScreen, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_favorites:
                    var intentFavorites = new Intent(this, typeof(FavoritesActivity));
                    intentFavorites.PutExtra("currentChatId", presenter.currentChatId);
                    StartActivity(intentFavorites);
                    return true;
                case Resource.Id.menu_start_test:
                    var intentTests = new Intent(this, typeof(SelectTestLevelActivity));
                    intentTests.PutExtra("currentChatId", presenter.currentChatId);
                    StartActivity(intentTests);
                    return true;
                case global::Android.Resource.Id.Home:
                    var intentDirections = new Intent(this, typeof(DirectionsActivity));
                    StartActivity(intentDirections);
                    return true;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void UpdateBackground(string resourceBackgroundName)
        {
            ListView layoutChat = FindViewById<ListView>(Resource.Id.forms_centralfragments_chat_chat_listView);
            int resourceId = AndroidResourceHelper.GetImageResource(this, resourceBackgroundName);
            layoutChat.SetBackgroundResource(resourceId);
        }

        public void ShowToast(string messageText)
        {
            Toast.MakeText(this, messageText, ToastLength.Short).Show();
        }

        public void HideButtonForSwapLanguage()
        {
            ImageButton buttonSwapDirection = FindViewById<ImageButton>(Resource.Id.buttonSwapDirection);
            buttonSwapDirection.Visibility = ViewStates.Gone;
        }
    }
}