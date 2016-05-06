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
    public class ChatManager : IInitDataTable<Chat>
    {
        ISQLiteTesting db;

        public ChatManager(ISQLiteTesting dbHelper)
        {
            db = dbHelper;
        }

        public void InitDefaultData()
        {
            throw new NotImplementedException();
        }

        public Chat GetItemForId(int id)
        {
            Repository<Chat> repos = new Repository<Chat>();
            return repos.GetItem(id);
        }

        public int SaveItem(Chat item)
        {
            Repository<Chat> repo = new Repository<Chat>();
            return repo.Save(item);
        }

        internal Chat GetChatByLanguage(Language userLanguage, Language robotLanguage)
        {
            return db.Table<Chat>().SingleOrDefault(item=>item.LanguageFrom == userLanguage.ID && item.LanguageTo == robotLanguage.ID);
        }
    }
}
