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
using HockeyApp.Android;

namespace Droid.Core.Helpers
{
    public class HockeyAppCrashHelper
    {
        static string HockeyAppId = "1fa12db7cc804215bdd1a7542b3d1c96";
        public static void Register(Application appContext)
        {
#if !DEBUG
            CrashManager.Register(appContext, HockeyAppId);
#endif
        }
    }
}