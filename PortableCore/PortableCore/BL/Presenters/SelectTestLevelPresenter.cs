using PortableCore.BL.Managers;
using PortableCore.BL.Models;
using PortableCore.BL.Views;
using PortableCore.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Presenters
{
    public class SelectTestLevelPresenter
    {
        private ChatHistoryManager chatHistoryManager;
        private ISQLiteTesting db;
        private int currentChatId;
        private ISelectTestLevelView view;

        public SelectTestLevelPresenter(ISelectTestLevelView view, ISQLiteTesting db, int currentChatId)
        {
            this.view = view;
            this.db = db;
            this.currentChatId = currentChatId;
            this.chatHistoryManager = new ChatHistoryManager(db);
        }

        public int GetCountWords()
        {
            return this.chatHistoryManager.GetFavoriteMessages(currentChatId).Count;
        }
    }
}
