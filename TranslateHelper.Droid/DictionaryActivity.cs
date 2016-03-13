
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
using Android.Content;
using Java.Util;
using Android.Views.InputMethods;
using PortableCore.BL.Managers;
using System.Text;

namespace TranslateHelper.Droid
{
    [Activity (Label = "@string/app_name", MainLauncher = false, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class DictionaryActivity : Activity
	{
        public const string HOCKEYAPP_APPID = "e9137253ae304b09ae2ffbb2016f8eda";
        TranslateDirection direction = new TranslateDirection(SqlLiteInstance.DB, new DirectionManager(SqlLiteInstance.DB));
        delegate Task goTranslateRequest(string originalText);
        goTranslateRequest RequestReference;
        string preparedTextForRequest = string.Empty;

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

            RequestReference = new goTranslateRequest(translateRequest);

            base.ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
            SetContentView(Resource.Layout.Dictionary);

            string directionName = Intent.GetStringExtra("directionName");
            if(string.IsNullOrEmpty(directionName))
                direction.SetDefaultDirection();
            else
                direction.SetDirection(directionName);

            updateDestinationCaption();

            EditText editSourceText = FindViewById<EditText> (Resource.Id.textSourceString);
			ImageButton buttonNew = FindViewById<ImageButton> (Resource.Id.buttonNew);
			ImageButton buttonTranslateTop = FindViewById<ImageButton> (Resource.Id.buttonTranslateTop);
            ImageButton buttonTranslateBottom = FindViewById<ImageButton>(Resource.Id.buttonTranslateBottom);

            buttonNew.Click += (object sender, EventArgs e) => {
				{
					editSourceText.Text = string.Empty;
					clearTraslatedRegion();
                    TogglesSoftKeyboard.Show(this);
                }
            };

            buttonTranslateTop.Click += (object sender, EventArgs e) =>
            {
                startRequestWithValidation(editSourceText.Text);
            };

            buttonTranslateBottom.Click += (object sender, EventArgs e) =>
            {
                startRequestWithValidation(editSourceText.Text);
            };

            editSourceText.TextChanged += (object sender, Android.Text.TextChangedEventArgs e) => 
            {
                
                if ((editSourceText.Text.Length > 0) && (iSSymbolForStartTranslate (editSourceText.Text.Last ())))
                {
                    startRequestWithValidation(editSourceText.Text);
                }
            };

			clearTraslatedRegion ();
		}

        private async void startRequestWithValidation(string originalText)
        {
            preparedTextForRequest = prepareTextForRequest(originalText);
            if (!string.IsNullOrEmpty(preparedTextForRequest))
            {
                DetectInputLanguage detect = new DetectInputLanguage(originalText);
                DetectInputLanguage.Language result = detect.Detect();
                if ((result!=DetectInputLanguage.Language.Unknown) && !direction.IsFrom(result))
                {
                    ShowChangeDestinationDialog();
                }
                else await RequestReference(preparedTextForRequest);
            }
        }

        private string prepareTextForRequest(string originalText)
        {
            return ConvertStrings.StringToOneLowerLineWithTrim(originalText);
        }

        private async Task translateRequest(string originalText)
        {
            if (!string.IsNullOrEmpty(originalText))
            {
                TranslateRequestRunner reqRunner = getRequestRunner(direction);
                TranslateRequestResult reqResult = await reqRunner.GetDictionaryResult(originalText, direction);
                if (string.IsNullOrEmpty(reqResult.errorDescription) && (reqResult.TranslatedData.Definitions.Count == 0))
                {
                    reqResult = await reqRunner.GetTranslationResult(originalText, direction);
                }

                if (string.IsNullOrEmpty(reqResult.errorDescription))
                {
                    updateListResults(reqResult);
                    TogglesSoftKeyboard.Hide(this);
                }
                else
                {
                    Toast.MakeText(this, reqResult.errorDescription, ToastLength.Long).Show();
                }
            }
        }

        private TranslateRequestRunner getRequestRunner(TranslateDirection translateDirection)
        {
            TranslateRequestRunner reqRunner = new TranslateRequestRunner( 
                SqlLiteInstance.DB,
                new CachedResultReader(translateDirection, SqlLiteInstance.DB),
                new TranslateRequest(TypeTranslateServices.YandexDictionary, translateDirection),
                new TranslateRequest(TypeTranslateServices.YandexTranslate, translateDirection));
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
                    var intentFavorites = new Intent(this, typeof(FavoritesActivity));
                    intentFavorites.PutExtra("directionName", direction.GetCurrentDirectionName());
                    StartActivity(intentFavorites);
                    return true;
                case Resource.Id.menu_dest_selector:
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
            direction.Invert();
            updateDestinationCaption();
        }

        private void updateDestinationCaption()
        {
            var destinationTextView = FindViewById<TextView>(Resource.Id.destinationTextView);
            destinationTextView.Text = direction.GetCurrentDirectionNameFull();
        }

        private void ShowChangeDestinationDialog()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(Resource.String.msg_warning);
            alert.SetMessage(Resource.String.act_autochangedestination);
            alert.SetPositiveButton(Resource.String.msg_ok, (senderAlert, args) => 
            {
                swapDestination();
                RequestReference(preparedTextForRequest);
            });
            alert.SetNegativeButton(Resource.String.msg_cancel, (senderAlert, args) => 
            {
                RequestReference(preparedTextForRequest);
            });
            Dialog dialog = alert.Create();
            dialog.Show();
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

