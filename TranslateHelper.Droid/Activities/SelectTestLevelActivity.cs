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
using PortableCore.BL;
using PortableCore.BL.Managers;
using PortableCore.BL.Views;
using PortableCore.BL.Presenters;
using HockeyApp.Android.Metrics;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/act_selectcountwords_caption", Theme = "@style/MyTheme")]
    public class SelectTestLevelActivity : Activity, ISelectTestLevelView
    {
        SelectTestLevelPresenter presenter;
        private int currentChatId;
        int countOfAvailableWords = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            MetricsManager.Register(Application, "1fa12db7cc804215bdd1a7542b3d1c96");
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.SelectTestLevel);
        }
        protected override void OnStart()
        {
            base.OnStart();
            currentChatId = Intent.GetIntExtra("currentChatId", -1);
            if (currentChatId >= 0)
            {
                presenter = new SelectTestLevelPresenter(this, SqlLiteInstance.DB, currentChatId);
                countOfAvailableWords = presenter.GetCountWords();
                if (countOfAvailableWords >= 10)
                    initLevelButtons();
                else
                    ShowErrorDialog();
            }
            else
            {
                throw new Exception("Chat not found");
            }
        }

        private void initLevelButtons()
        {
            ImageButton button10w = FindViewById<ImageButton>(Resource.Id.buttonSelect10Words);
            ImageButton button20w = FindViewById<ImageButton>(Resource.Id.buttonSelect20Words);
            ImageButton button50w = FindViewById<ImageButton>(Resource.Id.buttonSelect50Words);
            ImageButton button100w = FindViewById<ImageButton>(Resource.Id.buttonSelect100Words);
            button10w.Click += (object sender, EventArgs e) => {
                {
                    HockeyApp.MetricsManager.TrackEvent("Select test level", new Dictionary<string, string> { { "property", "value" } }, new Dictionary<string, double> { { "SelectCountWordForTest", 10 } });
                    StartTest_SelectRightWords(10);
                }
            };
            button20w.Click += (object sender, EventArgs e) => {
                {
                    HockeyApp.MetricsManager.TrackEvent("Select test level", new Dictionary<string, string> { { "property", "value" } }, new Dictionary<string, double> { { "SelectCountWordForTest", 20 } });
                    StartTest_SelectRightWords(20);
                }
            };
            button50w.Click += (object sender, EventArgs e) => {
                {
                    HockeyApp.MetricsManager.TrackEvent("Select test level", new Dictionary<string, string> { { "property", "value" } }, new Dictionary<string, double> { { "SelectCountWordForTest", 50 } });
                    StartTest_SelectRightWords(50);
                }
            };
            button100w.Click += (object sender, EventArgs e) => {
                {
                    HockeyApp.MetricsManager.TrackEvent("Select test level", new Dictionary<string, string> { { "property", "value" } }, new Dictionary<string, double> { { "SelectCountWordForTest", 100 } });
                    StartTest_SelectRightWords(100);
                }
            };
            button10w.Activated = countOfAvailableWords >= 10;
            button20w.Activated = countOfAvailableWords >= 20;
            button50w.Activated = countOfAvailableWords >= 50;
            button100w.Activated = countOfAvailableWords >= 100;
        }

        private void StartTest_SelectRightWords(int countOfWords)
        {
            var intentTest = new Intent(this, typeof(TestSelectWordsActivity));
            intentTest.PutExtra("countOfWords", countOfWords);
            intentTest.PutExtra("currentChatId", currentChatId);
            StartActivity(intentTest);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_SelectTestLevelScreen, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    StartDictionaryActivity();
                    return true;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void ShowErrorDialog()
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle(Resource.String.msg_warning);
            alert.SetMessage(Resource.String.act_trytoaddfavorites);
            alert.SetPositiveButton("Ok", (senderAlert, args) => { StartDictionaryActivity(); });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        private void StartDictionaryActivity()
        {
            var intentDictActivity = new Intent(this, typeof(DictionaryChatActivity));
            intentDictActivity.AddFlags(ActivityFlags.ClearTop);
            intentDictActivity.PutExtra("currentChatId", currentChatId);
            StartActivity(intentDictActivity);
        }

    }
}