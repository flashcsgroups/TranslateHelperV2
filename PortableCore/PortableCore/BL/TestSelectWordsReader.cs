using PortableCore.BL.Managers;
using PortableCore.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL
{
    public class TestSelectWordsReader
    {
        ISQLiteTesting db;
        public TestSelectWordsReader(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

        public List<Favorites> GetRandomFavorites(int countOfWords, TranslateDirection direction)
        {
            List<Favorites> resultList = new List<Favorites>();
            var directionId = direction.GetCurrentDirectionId();
            //var view = from item in db.Table<Favorites>() where item.DirectionID == directionId select item.ID;
            var view = from item in db.Table<Favorites>() select item.ID;
            var favElements = view.ToList();
            int countOfRecords = favElements.Count();
            FavoritesManager favManager = new FavoritesManager(db);
            Random rnd = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            int maxCountOfWords = countOfWords <= countOfRecords ? countOfWords : countOfRecords;
            for (int i=0;i< maxCountOfWords; i++)
            {
                int indexOfRecord = rnd.Next(1, countOfRecords);
                var favItemId = favElements[indexOfRecord];
                resultList.Add(favManager.GetItemForId(favItemId));
            }
            return resultList;
        }
    }
}
