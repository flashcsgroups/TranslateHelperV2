using NUnit.Framework;
using PortableCore.BL.Contracts;
using System.Collections.Generic;
using PortableCore.DL;
using PortableCore.BL;

namespace PortableCore.Tests
{
    [TestFixture]
    public class UserTestSelectWordsTests
    {
        [Test]
        public void TestMust_GetFavoritesDataForTest()
        {
            //arrange
            int countOfWords = 10;
            SQLiteTest dbHelper = new SQLiteTest();
            TranslateDirection direction = new TranslateDirection(dbHelper);
            //direction.SetDefaultDirection();

            //act
            TestSelectWordsReader data = new TestSelectWordsReader(dbHelper);
            List<Favorites> favoritesList = data.GetRandomFavorites(countOfWords, direction);

            //assert
            Assert.IsTrue(favoritesList.Count == 10);
        }

        public class SQLiteTest : ISQLiteTesting
        {
            public IEnumerable<Favorites> Table<Favorites>() where Favorites : IBusinessEntity, new()
            {
                List<Favorites> listFav = new List<Favorites>();
                listFav.Add(new Favorites() { ID = 1});
                listFav.Add(new Favorites() { ID = 2 });
                listFav.Add(new Favorites() { ID = 3 });
                listFav.Add(new Favorites() { ID = 4 });
                listFav.Add(new Favorites() { ID = 5 });
                listFav.Add(new Favorites() { ID = 6 });
                listFav.Add(new Favorites() { ID = 7 });
                listFav.Add(new Favorites() { ID = 8 });
                listFav.Add(new Favorites() { ID = 9 });
                listFav.Add(new Favorites() { ID = 10 });
                return listFav;
            }
        }

    }
}