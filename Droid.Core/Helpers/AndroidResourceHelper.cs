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

namespace Droid.Core.Helpers
{
    public static class AndroidResourceHelper
    {
        public static int GetImageResource(Activity context, string imageName)
        {
            return context.Resources.GetIdentifier(imageName.ToLower(), "drawable", context.PackageName); 
        }
    }
}