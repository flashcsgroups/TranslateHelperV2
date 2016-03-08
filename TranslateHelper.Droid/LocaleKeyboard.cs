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
using Android.InputMethodServices;
using Android.Views.InputMethods;

namespace TranslateHelper.Droid
{
    public class LocaleKeyboard
    {
        private string inputMethodService = string.Empty;
        private Activity context;

        public LocaleKeyboard(Activity context, string inputMethodService)
        {
            this.context = context;
            this.inputMethodService = inputMethodService;
        }

        public string GetCurrentKeyboardLocale()
        {
            string result = string.Empty;
            InputMethodManager inputManager = (InputMethodManager)context.GetSystemService(inputMethodService);
            if (inputManager.CurrentInputMethodSubtype != null)
            {
                if (inputManager.CurrentInputMethodSubtype.Locale != null)
                {
                    result = inputManager.CurrentInputMethodSubtype.Locale;
                }
            }
            return result;
        }
    }
}