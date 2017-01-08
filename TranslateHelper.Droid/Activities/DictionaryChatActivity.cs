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
        }

        protected override void OnStart()
        {
            base.OnStart();
            int selectedChatID = Intent.GetIntExtra("SelectedChatID", -1);
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

        public void UpdateChat(List<BubbleItem> listBubbles)
        {
            var metrics = Resources.DisplayMetrics;
            var listView = FindViewById<ListView>(Resource.Id.forms_centralfragments_chat_chat_listView);            
            bubbleAdapter = new BubbleAdapter(this, listBubbles, metrics);
            listView.Adapter = bubbleAdapter;
            listView.ChoiceMode = ChoiceMode.Single;
            listView.SetSelection(listView.Count + 1);
            listView.ItemLongClick += ListView_ItemLongClick;
            listView.ItemClick += ListView_ItemClick;
        }

        private void ListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            presenter.InvertFavoriteState(bubbleAdapter.GetBubbleItemByIndex(e.Position));
        }

        private void ListView_ItemLongClick(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            var listView = FindViewById<ListView>(Resource.Id.forms_centralfragments_chat_chat_listView);
            listView.CancelLongPress();
            DeleteRowByUserAction(e.Position);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //currentMenu = menu;
            //MenuInflater.Inflate(Resource.Menu.menu_DictionaryScreen, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                /*case Resource.Id.menu_favorites:
                    var intentFavorites = new Intent(this, typeof(FavoritesActivity));
                    intentFavorites.PutExtra("directionName", presenter.GetCurrentDirectionName());
                    StartActivity(intentFavorites);
                    return true;
                case Resource.Id.menu_start_test:
                    var intentTests = new Intent(this, typeof(SelectTestLevelActivity));
                    intentTests.PutExtra("directionName", presenter.GetCurrentDirectionName());
                    StartActivity(intentTests);
                    return true;*/
                case global::Android.Resource.Id.Home:
                    var intentDirections = new Intent(this, typeof(DirectionsActivity));
                    StartActivity(intentDirections);
                    return true;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        public void DeleteRowByUserAction(int elementPositionIndex)
        {
            //presenter.DeleteBubbleFromChat(bubbleAdapter.GetBubbleItemByIndex(elementPositionIndex));
            //bubbleAdapter.MarkBubbleItemAsDeleted(elementPositionIndex);
            /*var t = Toast.MakeText(this, "test:" + elementPositionIndex.ToString(), ToastLength.Short);
            t.SetGravity(GravityFlags.Center, 0, 0);
            t.Show();*/

            /*AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("");
            alert.SetMessage("Delete row?");
            alert.SetPositiveButton("Delete", (senderAlert, args) => {
                presenter.DeleteBubbleFromChat(bubbleAdapter.GetBubbleItemByIndex(elementPositionIndex));
                bubbleAdapter.MarkBubbleItemAsDeleted(elementPositionIndex);
            });
            alert.SetNegativeButton("Cancel", (senderAlert, args) => {
            });
            Dialog dialog = alert.Create();
            dialog.Show();*/
        }

        private void updateDestinationCaption()
        {
            /*if (currentMenu != null)
            {
                IMenuItem item = currentMenu.FindItem(Resource.Id.menu_dest_selector);
                switch (direction.GetCurrentDirectionId())
                {
                    case 1:
                        {
                            item.SetIcon(Resource.Drawable.EngRus);
                        }; break;
                    case 2:
                        {
                            item.SetIcon(Resource.Drawable.RusEng);
                        }; break;
                    default:
                        { }; break;
                }
            }*/
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