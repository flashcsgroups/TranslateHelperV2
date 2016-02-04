
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

namespace TranslateHelper.Droid
{
    [Activity (Label = "Translate helper", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/MyTheme")]
    public class DictionaryActivity : Activity
	{
        protected override async void OnCreate (Bundle bundle)
		{
            base.OnCreate (bundle);
			base.ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
            SetContentView(Resource.Layout.Dictionary);
            //запрос для установки соединения еще до того, как оно понадобится пользователю, для ускорения
            await callTestRequest();

            EditText editSourceText = FindViewById<EditText> (Resource.Id.textSourceString);
			ImageButton buttonNew = FindViewById<ImageButton> (Resource.Id.buttonNew);
			ImageButton buttonTranslate = FindViewById<ImageButton> (Resource.Id.buttonTranslate);

            

			buttonNew.Click += (object sender, EventArgs e) => {
				{
					editSourceText.Text = string.Empty;
					clearTraslatedRegion();
				}
			};

            buttonTranslate.Click += async (object sender, EventArgs e) =>
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
                TranslateRequestResult reqResult = await reqRunner.GetDictionaryResult(convertedText, "en-ru");
                if (reqResult.TranslatedData.Definitions.Count == 0)
                {
                    reqResult = await reqRunner.GetTranslationResult(convertedText, "en-ru");
                }
                updateListResults(reqResult);
            }
        }

        /// <summary>
        /// Пустой запрос выполняется чтобы установить связь с сервером до того, как пользователь захочет выполнить реальный запрос - это для ускорения выполнения первого запроса
        /// </summary>
        /// <returns></returns>
        private async Task callTestRequest()
        {
            TranslateRequestRunner reqRunner = getRequestRunner();
            TranslateRequestResult reqResult = await reqRunner.GetDictionaryResult(string.Empty, "en-ru");
        }

        private TranslateRequestRunner getRequestRunner()
        {
            TranslateRequestRunner reqRunner = new TranslateRequestRunner(SqlLiteInstance.DB,
                new CachedResultReader(SqlLiteInstance.DB),
                new TranslateRequest(TypeTranslateServices.YandexDictionary),
                new TranslateRequest(TypeTranslateServices.YandexTranslate));
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
                Toast.MakeText(this, "Неизвестное выражение, проверьте текст на наличие ошибок.", Android.Widget.ToastLength.Long).Show();
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
                /*case Resource.Id.menu_testing:
                    //StartActivity(typeof(FavoritesActivity));
                    return true;
                case Resource.Id.menu_settings:
                    StartActivity(typeof(SettingsActivity));
                    return true;*/
                case global::Android.Resource.Id.Home:
                    StartActivity(typeof(SettingsActivity));
                    return true;
                default:
                    break;
            }
            return true;
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

