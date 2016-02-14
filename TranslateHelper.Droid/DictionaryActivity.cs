
using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using PortableCore.BL.Contracts;
using PortableCore.WS;
using PortableCore.Helpers;
using System.Threading.Tasks;
using PortableCore.DAL;
using Android.Runtime;
using PortableCore.BL;

namespace TranslateHelper.Droid
{
    [Activity (Label = "@string/app_name", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class DictionaryActivity : Activity
	{
        public const string HOCKEYAPP_APPID = "e9137253ae304b09ae2ffbb2016f8eda";
        TranslateDirection direction = new TranslateDirection();

        protected override async void OnCreate (Bundle bundle)
		{
            base.OnCreate (bundle);

            HockeyApp.CrashManager.Register(this, HOCKEYAPP_APPID);
            HockeyApp.UpdateManager.Register(this, HOCKEYAPP_APPID);
            HockeyApp.TraceWriter.Initialize();
            AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
            {
                HockeyApp.TraceWriter.WriteTrace(args.Exception);
                HockeyApp.TraceWriter.WriteTrace(args.Exception.Message);
                args.Handled = true;
            };

            AppDomain.CurrentDomain.UnhandledException +=
                (sender, args) => HockeyApp.TraceWriter.WriteTrace(args.ExceptionObject);

            TaskScheduler.UnobservedTaskException +=
                (sender, args) => HockeyApp.TraceWriter.WriteTrace(args.Exception);


            base.ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
            SetContentView(Resource.Layout.Dictionary);

            //запрос для установки соединения еще до того, как оно понадобится пользователю, для ускорения
            //await callTestRequest();

            EditText editSourceText = FindViewById<EditText> (Resource.Id.textSourceString);
			ImageButton buttonNew = FindViewById<ImageButton> (Resource.Id.buttonNew);
			ImageButton buttonTranslateTop = FindViewById<ImageButton> (Resource.Id.buttonTranslateTop);
            ImageButton buttonTranslateBottom = FindViewById<ImageButton>(Resource.Id.buttonTranslateBottom);
            //ImageButton buttonChangeDest = FindViewById<ImageButton>(Resource.Id.buttonChangeDest);



            buttonNew.Click += (object sender, EventArgs e) => {
				{
					editSourceText.Text = string.Empty;
					clearTraslatedRegion();
                    TogglesSoftKeyboard.Show(this);
                }
            };

            buttonTranslateTop.Click += async (object sender, EventArgs e) =>
            {
                await translate(editSourceText.Text);
            };

            buttonTranslateBottom.Click += async (object sender, EventArgs e) =>
            {
                await translate(editSourceText.Text);
            };

            editSourceText.TextChanged += async (object sender, Android.Text.TextChangedEventArgs e) => {
                
                if ((editSourceText.Text.Length > 0) && (iSSymbolForStartTranslate (editSourceText.Text.Last ())))
                {
                    await translate(editSourceText.Text);
                }
            };

			clearTraslatedRegion ();
		}

        private async Task translate(string originalText)
        {
            string convertedText = ConvertStrings.StringToOneLowerLineWithTrim(originalText);
            if(!string.IsNullOrEmpty(convertedText))
            {
                TranslateRequestRunner reqRunner = getRequestRunner();
                //string lang = "en-ru";
                //lang = "ru-en";
                //TranslateDirection direction = n
                TranslateRequestResult reqResult = await reqRunner.GetDictionaryResult(convertedText, direction);
                if (reqResult.TranslatedData.Definitions.Count == 0)
                {
                    reqResult = await reqRunner.GetTranslationResult(convertedText, direction);
                }
                updateListResults(reqResult);
                TogglesSoftKeyboard.Hide(this);
            }
        }

        /// <summary>
        /// Пустой запрос выполняется чтобы установить связь с сервером до того, как пользователь захочет выполнить реальный запрос - это для ускорения выполнения первого запроса
        /// </summary>
        /// <returns></returns>
        private async Task callTestRequest()
        {
            TranslateRequestRunner reqRunner = getRequestRunner();
            TranslateRequestResult reqResult = await reqRunner.GetDictionaryResult(string.Empty, direction);
        }

        private TranslateRequestRunner getRequestRunner()
        {
            TranslateRequestRunner reqRunner = new TranslateRequestRunner( 
                SqlLiteInstance.DB,
                new CachedResultReader(direction, SqlLiteInstance.DB),
                new TranslateRequest(TypeTranslateServices.YandexDictionary, direction),
                new TranslateRequest(TypeTranslateServices.YandexTranslate, direction));
            return reqRunner;
        }

        private void clearTraslatedRegion()
		{
            var scrollViewTranslateResultsDefs = FindViewById<ScrollView>(Resource.Id.scrollViewTranslateResultsDefs);
            scrollViewTranslateResultsDefs.RemoveAllViews();
            //scrollViewTranslateResultsDefs.Visibility = ViewStates.Invisible;
            //TextView splash = FindViewById<TextView>(Resource.Id.splashTextView);
            //splash.Visibility = ViewStates.Visible;
        }

        private bool iSSymbolForStartTranslate (char p)
		{
			return ((p == ' ') || (p == '\n'));
        }

        void updateListResults(TranslateRequestResult requestResult)
        {
            TranslateResultView resultView = requestResult.TranslatedData;
            if (resultView.Definitions.Count > 0)
            {
                //TextView splash = FindViewById<TextView>(Resource.Id.splashTextView);
                //splash.Visibility = ViewStates.Invisible;
                var scrollViewTranslateResultsDefs = FindViewById<ScrollView>(Resource.Id.scrollViewTranslateResultsDefs);
                scrollViewTranslateResultsDefs.RemoveAllViews();
                scrollViewTranslateResultsDefs.AddView(new DynamicResultViewLayout(this, resultView.Definitions));
                scrollViewTranslateResultsDefs.Visibility = ViewStates.Visible;
            }
            else
            {
                Toast.MakeText(this, Resource.String.msg_unknown_expression, ToastLength.Long).Show();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_DictionaryScreen, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_favorites:
                    StartActivity(typeof(FavoritesActivity));
                    return true;
                case Resource.Id.menu_dest_selector:
                    /*AlertDialog.Builder dlgBuilder = new AlertDialog.Builder(this);
                    dlgBuilder.SetTitle(Resource.String.caption_translate_destination);
                    dlgBuilder.SetMessage("Английский > Русский");
                    Dialog dialog = dlgBuilder.Create();
                    dialog.Show();
                    return true;*/
                    swapDestination();
                    break;
                case global::Android.Resource.Id.Home:
                    StartActivity(typeof(SettingsActivity));
                    return true;
                default:
                    break;
            }
            return true;
        }

        private void swapDestination()
        {
            switch (direction.GetCurrentDirectionName())
            {
                case "en-ru":
                    {
                        direction.SetDirection("ru-en");
                    }; break;
                case "ru-en":
                    {
                        direction.SetDirection("en-ru");
                    }; break;
            }
            var destinationTextView = FindViewById<TextView>(Resource.Id.destinationTextView);
            destinationTextView.Text = direction.GetCurrentDirectionNameFull();
        }

        void menuItemClicked(string item)
        {
            Console.WriteLine(item + " option menuitem clicked");
            var t = Toast.MakeText(this, "Options Menu '" + item + "' clicked", ToastLength.Short);
            t.SetGravity(GravityFlags.Center, 0, 0);
            t.Show();
        }

    }
}

