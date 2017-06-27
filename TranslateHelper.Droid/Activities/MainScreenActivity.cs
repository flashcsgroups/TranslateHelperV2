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
using Droid.Core.Helpers;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/app_name", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class MainScreenActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            HockeyAppMetricsHelper.Register(Application);
            HockeyAppMetricsHelper.TrackEvent("Main screen");
            SetContentView(Resource.Layout.MainScreen);
            var layoutRecent = FindViewById<LinearLayout>(Resource.Id.layoutRecent);
            layoutRecent.Click += LayoutRecent_Click;
            var layoutLanguages = FindViewById<LinearLayout>(Resource.Id.layoutLanguages);
            layoutLanguages.Click += LayoutLanguages_Click;
            var layoutAnecdotes = FindViewById<LinearLayout>(Resource.Id.layoutAnecdotes);
            layoutAnecdotes.Click += LayoutAnecdotes_Click;
            var layoutIdioms = FindViewById<LinearLayout>(Resource.Id.layoutIdioms);
            layoutIdioms.Click += LayoutIdioms_Click;
            var layoutFeedback = FindViewById<LinearLayout>(Resource.Id.layoutFeedback);
            layoutFeedback.Click += LayoutFeedback_Click;
        }

        private void LayoutFeedback_Click(object sender, EventArgs e)
        {
            string appPackageName = this.PackageName;
            try
            {
                StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("market://details?id=" + appPackageName)));
            }
            catch (ActivityNotFoundException)
            {
                StartActivity(new Intent(Intent.ActionView, Android.Net.Uri.Parse("https://play.google.com/store/apps/details?id=" + appPackageName)));
            }
        }

        private void LayoutIdioms_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Application.Context, typeof(DirectionsActivity));
            intent.PutExtra("DirectionLayoutType", (int)DirectionsLayoutTypes.Idioms);
            StartActivity(intent);
        }

        private void LayoutAnecdotes_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Application.Context, typeof(DirectionsActivity));
            intent.PutExtra("DirectionLayoutType", (int)DirectionsLayoutTypes.Anecdotes);
            StartActivity(intent);
        }

        private void LayoutLanguages_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Application.Context, typeof(DirectionsActivity));
            intent.PutExtra("DirectionLayoutType", (int)DirectionsLayoutTypes.AllChats);
            StartActivity(intent);
        }

        private void LayoutRecent_Click(object sender, EventArgs e)
        {
            var intent = new Intent(Application.Context, typeof(DirectionsActivity));
            intent.PutExtra("DirectionLayoutType", (int)DirectionsLayoutTypes.RecentChat);
            StartActivity(intent);
        }

        protected override void OnStart()
        {
            base.OnStart();
        }

    }
}