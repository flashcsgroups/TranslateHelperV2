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
    public class ChatHistoryManager : IInitDataTable<ChatHistory>
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

        internal ChatHistory GetLastRobotMessage()
        {
            ChatHistory resultItem = new ChatHistory();
            var view = (from item in db.Table<ChatHistory>() orderby item.ID descending select item).Take(1);
            if(view.Count() > 0)
            {
                resultItem = view.Single();
            }
            return resultItem;
        }

        internal List<ChatHistory> ReadChatMessages(Chat chatItem)
        {
            var view = from item in db.Table<ChatHistory>() where item.ChatID == chatItem.ID orderby item.ID ascending select item;
            return view.ToList();
        }

        internal int GetCountOfMessagesForChat(int chatId)
        {
            return db.Table<ChatHistory>().Count(t => t.ChatID == chatId);
        }

        internal void DeleteItemById(int historyRowId)
        {
            Repository<ChatHistory> repo = new Repository<ChatHistory>();
            int result = repo.Delete(historyRowId);
        }
    }
}
