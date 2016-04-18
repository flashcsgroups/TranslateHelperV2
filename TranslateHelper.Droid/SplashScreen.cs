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
using System.Threading.Tasks;
using PortableCore.Helpers;
using PortableCore.DAL;
using PortableCore.WS;
using PortableCore.BL.Contracts;
using PortableCore.BL;
using PortableCore.BL.Managers;
using TranslateHelper.Droid.Activities;

namespace TranslateHelper.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        protected override void OnResume()
        {
            base.OnResume();
            callTestRequest();
            //Task.Delay(2000);
            StartActivity(new Intent(Application.Context, typeof(DirectionsActivity)));
            /*Task startupWork = new Task(() =>
            {
                callTestRequest();
            });

            startupWork.ContinueWith(t =>
            {
                StartActivity(new Intent(Application.Context, typeof(DictionaryActivity)));
            }, TaskScheduler.FromCurrentSynchronizationContext());

            startupWork.Start();*/
        }

        /// <summary>
        /// запрос для установки соединения еще до того, как оно понадобится пользователю, для ускорения
        /// </summary>
        /// <returns></returns>
        private void callTestRequest()
        {
            TranslateDirection direction = new TranslateDirection(SqlLiteInstance.DB, new DirectionManager(SqlLiteInstance.DB));
            TranslateRequestRunner reqRunner = new TranslateRequestRunner(
                SqlLiteInstance.DB,
                new CachedResultReader(direction, SqlLiteInstance.DB),
                new TranslateRequest(TypeTranslateServices.YandexDictionary, direction),
                new TranslateRequest(TypeTranslateServices.YandexTranslate, direction));
            var reqResult = reqRunner.GetDictionaryResult(string.Empty, direction);
        }
    }
}