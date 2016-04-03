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

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class DictionaryChatActivity : Activity, IDictionaryChatView
    {
        DictionaryChatPresenter presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.DictionaryChat);
            EditText editSourceText = FindViewById<EditText>(Resource.Id.textSourceString);
            ImageButton buttonTranslate = FindViewById<ImageButton>(Resource.Id.buttonTranslate);
            buttonTranslate.Click += (object sender, EventArgs e) =>
            {
                presenter.StartRequestWithValidation(editSourceText.Text);
            };
        }

        protected override void OnStart()
        {
            base.OnStart();
            presenter = new DictionaryChatPresenter(this, SqlLiteInstance.DB);
        }

        public void UpdateChat(List<BubbleItem> listBubbles)
        {
            var listView = FindViewById<ListView>(Resource.Id.forms_centralfragments_chat_chat_listView);
            var newAdapter = new BubbleAdapter(this, listBubbles);
            listView.Adapter = newAdapter;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //currentMenu = menu;
            MenuInflater.Inflate(Resource.Menu.menu_DictionaryScreen, menu);
            return true;
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            Java.Lang.JavaSystem.Exit(0);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_favorites:
                    var intentFavorites = new Intent(this, typeof(FavoritesActivity));
                    intentFavorites.PutExtra("directionName", presenter.GetCurrentDirectionName());
                    StartActivity(intentFavorites);
                    return true;
                case Resource.Id.menu_dest_selector:
                    /*direction.Invert();
                    updateDestinationCaption();*/
                    break;
                case Resource.Id.menu_start_test:
                    var intentTests = new Intent(this, typeof(SelectTestLevelActivity));
                    intentTests.PutExtra("directionName", presenter.GetCurrentDirectionName());
                    StartActivity(intentTests);
                    return true;
                case global::Android.Resource.Id.Home:
                    return true;
                default:
                    break;
            }
            return true;
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

    }
}