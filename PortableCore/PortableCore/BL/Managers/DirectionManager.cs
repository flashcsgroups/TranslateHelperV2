using PortableCore.DL;
using PortableCore.BL.Contracts;
using PortableCore.DAL;
using System.Linq;

namespace PortableCore.BL.Managers
{
    public class DirectionManager : IInitDataTable<Direction>
    {
        ISQLiteTesting db;

        public DirectionManager(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

		public void InitDefaultData ()
		{
            Repository<Direction> repos = new Repository<Direction>();
            Direction[] data = getDefaultData ();
            if(repos.Count() != data.Length)
            {
                repos.DeleteAllDataInTable();
                repos.AddItemsInTransaction(data);
            }			
		}

        public Direction GetItemForId(int Id)
        {
            Repository<Direction> repos = new Repository<Direction>();
            Direction result = repos.GetItem(Id);
            return result;
        }

        public Direction GetItemForName(string name)
        {
            Direction result = new Direction();
            //ToDo:ProviderId получать!
            int YandexProviderId = 11;
            Repository<Direction> repos = new DAL.Repository<Direction>();
            //var view = from item in SqlLiteInstance.DB.Table<Direction>() where item.Name == name && item.ProviderID == YandexProviderId select item;
            var view = from item in db.Table<Direction>() where item.Name == name && item.ProviderID == YandexProviderId select item;
            if (view.Count() == 1) result = view.First(); 
            return result;
        }

        private Direction[] getDefaultData()
		{
            //ToDo:ProviderId переделать на метод получения ID для дефолтного "Yandex"
            Direction[] directionList = new Direction[] {
                new Direction (){ Name = "en-ru", FullName = "English > Русский", ID=0, ProviderID = 11}, 
				new Direction (){ Name = "ru-en", FullName = "Русский > English", ID=1, ProviderID = 11},
            };

			return directionList;
		}
	}
}

