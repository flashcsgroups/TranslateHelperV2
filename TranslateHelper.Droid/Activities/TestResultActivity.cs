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
using PortableCore.BL.Presenters;
using PortableCore.BL.Views;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/caption_view_test_result", Theme = "@style/MyTheme")]
    public class TestResultActivity : Activity, ITestResultView
    {
        TestResultPresenter presenter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.TestResult);
        }
        protected override void OnStart()
        {
            base.OnStart();
            int currentChatId = Intent.GetIntExtra("currentChatId", -1);
            int countOfRightWords = Intent.GetIntExtra("countOfRightWords", -1);
            int countOfTotalWords = Intent.GetIntExtra("countOfTotalWords", -1);
            presenter = new TestResultPresenter(this, currentChatId, countOfRightWords, countOfTotalWords);
            TextView editSourceText = FindViewById<TextView>(Resource.Id.textResult);
            editSourceText.Text = presenter.GetTextResult();
            Button btnRepeatTest = FindViewById<Button>(Resource.Id.buttonRepeatTest);
            btnRepeatTest.Click += BtnRepeatTest_Click;
            Button btnCancelTest = FindViewById<Button>(Resource.Id.buttonCancelTest);
            btnCancelTest.Click += BtnCancelTest_Click;
        }

        private void BtnCancelTest_Click(object sender, EventArgs e)
        {
            var intentDirections = new Intent(this, typeof(DirectionsActivity));
            intentDirections.AddFlags(ActivityFlags.ClearTop);
            StartActivity(intentDirections);
        }

        private void BtnRepeatTest_Click(object sender, EventArgs e)
        {
            var intentTests = new Intent(this, typeof(SelectTestLevelActivity));
            intentTests.AddFlags(ActivityFlags.ClearTop);
            intentTests.PutExtra("currentChatId", presenter.CurrentChatId);
            StartActivity(intentTests);
        }
    }
}