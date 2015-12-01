using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.Queries;
using TranslateHelper.Core.WS;

namespace TranslateHelper.Droid.UITests
{
	[TestFixture]
	public class Tests
	{
		AndroidApp app;

		[SetUp]
		public void BeforeEachTest ()
		{
			app = ConfigureApp.Android.StartApp ();
		}

		[Test]
		public void ClickingButtonTwiceShouldChangeItsLabel ()
		{
			Func<AppQuery, AppQuery> MyButton = c => c.Button ("myButton");

			app.Tap (MyButton);
			app.Tap (MyButton);
			AppResult[] results = app.Query (MyButton);
			app.Screenshot ("Button clicked twice.");

			Assert.AreEqual ("2 clicks!", results [0].Text);
		}

        /*[Test]
        public void TestMust_ParseResponseWithIncorrectWord()
        {

            //arrange
            string responseText = @"""{""head"":{},""def"":[{""text"":""grow"",""pos"":""verb"",""ts"":""grəʊ"",""tr"":[{""text"":""расти"",""pos"":""глагол"",""asp"":""несов"",""syn"":[{""text"":""возрастать"",""pos"":""глагол"",""asp"":""несов""},{""text"":""вырастать"",""pos"":""глагол"",""asp"":""несов""},{""text"":""развиваться"",""pos"":""глагол"",""asp"":""несов""},{""text"":""произрастать"",""pos"":""глагол"",""asp"":""несов""},{""text"":""подрасти"",""pos"":""глагол"",""asp"":""сов""},{""text"":""нарастать"",""pos"":""глагол"",""asp"":""несов""},{""text"":""усиливаться"",""pos"":""глагол"",""asp"":""несов""}],""mean"":[{""text"":""increase""},{""text"":""outgrow""},{""text"":""develop""},{""text"":""grow up""},{""text"":""accrue""}],""ex"":[{""text"":""grow upwards"",""tr"":[{""text"":""расти вверх""}]},{""text"":""grow gradually"",""tr"":[{""text"":""возрастать постепенно""}]},{""text"":""grow further"",""tr"":[{""text"":""развиваться дальше""}]}]},{""text"":""выращивать"",""pos"":""глагол"",""asp"":""несов"",""syn"":[{""text"":""вырастить"",""pos"":""глагол"",""asp"":""сов""},{""text"":""растить"",""pos"":""глагол"",""asp"":""несов""},{""text"":""культивировать"",""pos"":""глагол"",""asp"":""несов""}],""mean"":[{""text"":""cultivate""},{""text"":""breed""}],""ex"":[{""text"":""grow food"",""tr"":[{""text"":""выращивать продукты""}]},{""text"":""grow a beard"",""tr"":[{""text"":""вырастить бороду""}]},{""text"":""grow grain"",""tr"":[{""text"":""растить хлеб""}]}]},{""text"":""становиться"",""pos"":""глагол"",""asp"":""несов"",""syn"":[{""text"":""перерасти"",""pos"":""глагол"",""asp"":""сов""},{""text"":""перерастать"",""pos"":""глагол"",""asp"":""несов""}],""mean"":[{""text"":""become""},{""text"":""degenerate""}],""ex"":[{""text"":""grow up"",""tr"":[{""text"":""становиться взрослым""}]},{""text"":""grow in love"",""tr"":[{""text"":""перерасти в любовь""}]}]},{""text"":""стать"",""pos"":""глагол"",""asp"":""сов"",""syn"":[{""text"":""происходить"",""pos"":""глагол"",""asp"":""несов""}],""mean"":[{""text"":""be""}],""ex"":[{""text"":""grow longer"",""tr"":[{""text"":""стать длиннее""}]}]},{""text"":""увеличиваться"",""pos"":""глагол"",""asp"":""несов"",""syn"":[{""text"":""разрастись"",""pos"":""глагол"",""asp"":""сов""}],""mean"":[{""text"":""enlarge""}]},{""text"":""разрастаться"",""pos"":""глагол"",""asp"":""несов"",""mean"":[{""text"":""expand""}]},{""text"":""дорасти"",""pos"":""глагол"",""asp"":""сов""},{""text"":""отращивать"",""pos"":""глагол"",""asp"":""несов"",""mean"":[{""text"":""sprout""}],""ex"":[{""text"":""grow beards"",""tr"":[{""text"":""отращивать бороды""}]}]}]},{""text"":""grow"",""pos"":""noun"",""ts"":""grəʊ"",""tr"":[{""text"":""рост"",""pos"":""существительное"",""gen"":""м"",""syn"":[{""text"":""выращивание"",""pos"":""существительное"",""gen"":""ср""}],""mean"":[{""text"":""gain""},{""text"":""cultivate""}],""ex"":[{""text"":""grow rapidly"",""tr"":[{""text"":""быстрый рост""}]},{""text"":""grow tobacco"",""tr"":[{""text"":""выращивание табака""}]}]}]}]}""";
            //act
            YandexDictionaryJSON dict = new YandexDictionaryJSON();
            var result = dict.ParseResponse(responseText);
            //assert

            Assert.AreEqual(result.Length, 1);
        }*/
    }
}

