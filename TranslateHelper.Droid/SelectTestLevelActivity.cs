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

namespace TranslateHelper.Droid
{
    [Activity(Label = "@string/act_selectcountwords_caption", Theme = "@style/MyTheme")]
    public class SelectTestLevelActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.SelectTestLevel);
            ImageButton button10w = FindViewById<ImageButton>(Resource.Id.buttonSelect10Words);
            ImageButton button20w = FindViewById<ImageButton>(Resource.Id.buttonSelect20Words);
            ImageButton button50w = FindViewById<ImageButton>(Resource.Id.buttonSelect50Words);
            ImageButton button100w = FindViewById<ImageButton>(Resource.Id.buttonSelect100Words);
            button10w.Click += (object sender, EventArgs e) => {
                {
                    StartTest_SelectRightWords(10);
                }
            };
            button20w.Click += (object sender, EventArgs e) => {
                {
                    StartTest_SelectRightWords(20);
                }
            };
            button50w.Click += (object sender, EventArgs e) => {
                {
                    StartTest_SelectRightWords(50);
                }
            };
            button100w.Click += (object sender, EventArgs e) => {
                {
                    StartTest_SelectRightWords(100);
                }
            };

        }

        private void StartTest_SelectRightWords(int countOfWords)
        {
            var intentTest = new Intent(this, typeof(TestSelectWordsActivity));
            intentTest.PutExtra("countOfWords", countOfWords);
            StartActivity(intentTest);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_SelectTestLevelScreen, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    StartActivity(typeof(FavoritesActivity));
                    return true;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}