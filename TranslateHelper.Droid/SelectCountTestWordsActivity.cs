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
    public class SelectCountTestWordsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.SelectCountTestWords);
            ImageButton button10w = FindViewById<ImageButton>(Resource.Id.buttonSelect10Words);
            ImageButton button20w = FindViewById<ImageButton>(Resource.Id.buttonSelect20Words);
            ImageButton button50w = FindViewById<ImageButton>(Resource.Id.buttonSelect50Words);
            ImageButton button100w = FindViewById<ImageButton>(Resource.Id.buttonSelect100Words);
            button10w.Click += (object sender, EventArgs e) => {
                {
                    StartActivity(typeof(SelectWordsActivity));
                }
            };
            button20w.Click += (object sender, EventArgs e) => {
                {
                    StartActivity(typeof(SelectWordsActivity));
                }
            };
            button50w.Click += (object sender, EventArgs e) => {
                {
                    StartActivity(typeof(SelectWordsActivity));
                }
            };
            button100w.Click += (object sender, EventArgs e) => {
                {
                    StartActivity(typeof(SelectWordsActivity));
                }
            };

        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_SelectCountWords, menu);
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