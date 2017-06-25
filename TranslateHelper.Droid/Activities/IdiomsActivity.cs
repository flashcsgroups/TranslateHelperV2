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
using Droid.Core.Helpers;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/act_idioms_caption", Theme = "@style/MyTheme")]
    public class IdiomsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            HockeyAppMetricsHelper.Register(Application);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.Idioms);
            HockeyAppMetricsHelper.TrackEvent("Open idioms");
            //http://polyidioms.narod.ru/index/0-128
            //http://catchenglish.ru/frazy-i-vyrazheniya.html
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    var intent = new Intent(this, typeof(DirectionsActivity));
                    intent.AddFlags(ActivityFlags.ClearTop);
                    StartActivity(intent);
                    return true;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}