using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PortableCore.BL.Contracts;
using PortableCore.DAL;
using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public class ChatHistoryManager : IInitDataTable<ChatHistory>, IChatHistoryManager
    {
        ISQLiteTesting db;

        public ChatHistoryManager(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

        public void InitDefaultData()
        {
            throw new NotImplementedException();
        }

        public ChatHistory GetItemForId(int id)
        {
            DAL.Repository<ChatHistory> repos = new DAL.Repository<ChatHistory>();
            ChatHistory result = repos.GetItem(id);
            return result;
        }

        //попробую пока хотя бы в менеджере запись изолировать, не давать эту возможность напрямую презентеру. Хотя все равно repo надо переделать так,
        //чтобы можно было и запись элементов тестировать, для этого надо будет db в репо тоже за интерфейс вынести.
        public int SaveItem(ChatHistory item)
        {
            Repository<ChatHistory> repo = new Repository<ChatHistory>();
            return repo.Save(item);
        }

        public int GetMaxItemId()
        {
            ChatHistory resultItem = new ChatHistory();
            int id = db.Table<ChatHistory>().DefaultIfEmpty().Max(x=>x==null?0:x.ID);
            return id;
        }

        public ChatHistory GetLastRobotMessage()
        {
            ChatHistory resultItem = new ChatHistory();
            var view = (from item in db.Table<ChatHistory>() orderby item.ID descending select item).Take(1);
            if(view.Count() > 0)
            {
                resultItem = view.Single();
            }
            return resultItem;
        }

        public List<Tuple<ChatHistory, ChatHistory>> GetFavoriteMessages(int selectedChatID)
        {
            var view = from item in db.Table<ChatHistory>() 
                       join parentItem in db.Table<ChatHistory>() on item.ParentRequestID equals parentItem.ID into favorites
                       from subFavorites in favorites
                       where item.ChatID == selectedChatID && item.InFavorites && item.DeleteMark == 0
                       orderby item.TextFrom select new Tuple<ChatHistory, ChatHistory> (item, subFavorites);                   
            return view.ToList();
        }
        public List<ChatHistory> ReadChatMessages(Chat chatItem)
        {
            var view = from item in db.Table<ChatHistory>() where item.ChatID == chatItem.ID && item.DeleteMark == 0 orderby item.ID ascending select item;
            return view.ToList();
        }

        public List<ChatHistory> ReadSuspendedChatMessages(Chat chatItem)
        {
            string searchMsg = GetSearchMessage(new Language());//Временно до того момента пока не разберусь с сообщением о поиске на разных языках
            var view = from item in db.Table<ChatHistory>() where item.ChatID == chatItem.ID && item.DeleteMark == 0 && item.TextTo == searchMsg orderby item.ID ascending select item;
            return view.ToList();
        }

        public int GetCountOfMessagesForChat(int chatId)
        {
            return db.Table<ChatHistory>().Count(t => t.ChatID == chatId);
        }

        public void DeleteItemById(int historyRowId)
        {
            Repository<ChatHistory> repo = new Repository<ChatHistory>();
            int result = repo.Delete(historyRowId);
        }

        //ToDo: Доделать сообщение о поиске под разные языки
        public string GetSearchMessage(Language languageFrom)
        {
            return languageFrom.NameEng + ". Роюсь в словаре...";
        }
    }
}
