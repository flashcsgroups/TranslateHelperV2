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
using System.Threading.Tasks;
using PortableCore.Helpers;
using PortableCore.DAL;
using PortableCore.WS;
using PortableCore.BL.Contracts;
using PortableCore.BL;
using PortableCore.BL.Managers;
using TranslateHelper.Droid.Activities;

namespace TranslateHelper.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainScreenActivity)));
        }

    }
}