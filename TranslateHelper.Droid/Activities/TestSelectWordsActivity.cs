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
using PortableCore.BL;
using PortableCore.DL;
using PortableCore.DAL;
using PortableCore.BL.Managers;
using PortableCore.BL.Presenters;
using PortableCore.BL.Views;
using PortableCore.BL.Models;
using System.Globalization;
using HockeyApp.Android.Metrics;

namespace TranslateHelper.Droid.Activities
{
    [Activity(Label = "@string/act_testselectwords_caption", Theme = "@style/MyTheme")]
    public class TestSelectWordsActivity : Activity, ITestSelectWordsView
    {
        readonly int countOfSubmitButtons = 8;//количество кнопок с ответами на форме
        TestSelectWordsPresenter presenter;
        private int currentChatId;

        private int lastSubmittedButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            MetricsManager.Register(Application, "1fa12db7cc804215bdd1a7542b3d1c96");
            MetricsManager.TrackEvent("Open test");
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);
            SetContentView(Resource.Layout.TestSelectWords);
            initSubmitButtons();
        }

        protected override void OnStart()
        {
            base.OnStart();
            currentChatId = Intent.GetIntExtra("currentChatId", -1);
            if (currentChatId >= 0)
            {
                int countOfWords = Intent.GetIntExtra("countOfWords", -1);
                presenter = new TestSelectWordsPresenter(this, SqlLiteInstance.DB, new TestSelectWordsReader(SqlLiteInstance.DB), currentChatId, countOfWords);
                presenter.Init();
            }
            else
            {
                throw new Exception("Chat not found");
            }
        }

        protected override void OnRestoreInstanceState(Bundle savedState)
        {
            base.OnRestoreInstanceState(savedState);
        }

        private void initSubmitButtons()
        {
            for (int index = 1; index <= countOfSubmitButtons; index++)
            {
                Button submit = getSubmitButtonByName("buttonSubmitTest" + index.ToString());
                submit.Click += Submit_Click;
                submit.SetBackgroundResource(Resource.Drawable.TestScreenButtonSelector);
            }
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            if(((Button)sender).Id!=lastSubmittedButton)
            {
                lastSubmittedButton = ((Button)sender).Id;
                presenter.OnSelectVariant(((Button)sender).Text);
            }
        }

        private Button getSubmitButtonByName(string buttonResourceName)
        {
            int res = (int)typeof(Resource.Id).GetField(buttonResourceName).GetValue(null);
            return FindViewById<Button>(res);
        }

        public void DrawNewVariant(TestWordItem originalWord, List<TestWordItem> variants)
        {
            var textOriginalWord = FindViewById<TextView>(Resource.Id.textOriginalWord);
            textOriginalWord.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(originalWord.TextFrom);
            var textTranscripton = FindViewById<TextView>(Resource.Id.textTranscripton);
            textTranscripton.Text = originalWord.Transcription;
            var textPartOfSpeech = FindViewById<TextView>(Resource.Id.textPartOfSpeech);
            textPartOfSpeech.Text = !string.IsNullOrEmpty(originalWord.PartOfSpeech)?"(" + originalWord.PartOfSpeech + ")":"";
            for (int buttonIndex = 1; buttonIndex <= countOfSubmitButtons; buttonIndex++)
            {
                Button submit = getSubmitButtonByName("buttonSubmitTest" + (buttonIndex).ToString());
                submit.SetBackgroundResource(Resource.Drawable.TestScreenButtonSelector);
                if(variants.Count <= countOfSubmitButtons)
                {
                    submit.Text = variants[buttonIndex - 1].TextTo;
                }
                else
                {
                    submit.Text = "*error*";
                }
            }
            lastSubmittedButton = 0;
        }

        public void SetButtonErrorState()
        {
            Button button = FindViewById<Button>(lastSubmittedButton);
            button.SetBackgroundResource(Resource.Drawable.TestScreenButtonSelectorError);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_TestSelectWordsScreen, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case global::Android.Resource.Id.Home:
                    backToDictionaryChat();
                    return true;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void backToDictionaryChat()
        {
            var intent = new Intent(this, typeof(DictionaryChatActivity));
            intent.PutExtra("currentChatId", currentChatId);
            StartActivity(intent);
        }

        public void SetFinalTest(int countOfTotalWords, int countOfRightWords)
        {
            var intentResultTest = new Intent(this, typeof(TestResultActivity));
            intentResultTest.PutExtra("currentChatId", currentChatId);
            intentResultTest.PutExtra("countOfRightWords", countOfRightWords);
            intentResultTest.PutExtra("countOfTotalWords", countOfTotalWords);
            StartActivity(intentResultTest);
        }
    }
}