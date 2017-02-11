using PortableCore.BL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PortableCore.BL.Views
{
    public interface IDictionaryChatView
    {
        void UpdateChat(List<BubbleItem> listBubbles);
        void UpdateBackground(string v);
        void HideButtonForSwapLanguage();
        void ShowToast(string messageText);
    }
}
