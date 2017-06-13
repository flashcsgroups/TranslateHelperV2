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
using HockeyApp.Android.Metrics;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/act_idioms_caption", Theme = "@style/MyTheme")]
    public class IdiomsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            MetricsManager.Register(Application, "1fa12db7cc804215bdd1a7542b3d1c96");
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.Idioms);
            MetricsManager.TrackEvent("Open idioms");
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