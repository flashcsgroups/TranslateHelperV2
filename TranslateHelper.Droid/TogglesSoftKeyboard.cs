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
using Android.Views.InputMethods;

namespace TranslateHelper.Droid
{
    public static class TogglesSoftKeyboard
    {
        public static void Hide(Activity activity)
        {
            if(activity!=null)
            {
                InputMethodManager inputManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
                var currentFocus = activity.CurrentFocus;
                inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
            }
        }

        public static void Show(Activity activity)
        {
            if (activity != null)
            {
                InputMethodManager inputManager = (InputMethodManager)activity.GetSystemService(Context.InputMethodService);
                var currentFocus = activity.CurrentFocus;
                inputManager.ShowSoftInput(currentFocus, ShowFlags.Implicit);  
            }
        }
    }
}