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

namespace Droid.Core.Helpers
{
    public class HockeyAppMetricsHelper
    {
        static string HockeyAppId = "1fa12db7cc804215bdd1a7542b3d1c96";
        public static void Register(Application appContext)
        {
#if !DEBUG
            MetricsManager.Register(appContext, HockeyAppId);
#endif
        }

        public static void TrackOperationDurationEvent(string operationName, int durationMSec)
        {
#if !DEBUG
            MetricsManager.TrackEvent("OperationsDelay", new Dictionary<string, string> { { "property", "value" } }, new Dictionary<string, Java.Lang.Double> { { operationName, new Java.Lang.Double(durationMSec) } });
#endif
        }

        public static void TrackEvent(string operationName)
        {
#if DEBUG
            MetricsManager.TrackEvent(operationName);
#endif
        }
    }
}