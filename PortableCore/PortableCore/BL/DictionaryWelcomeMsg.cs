using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL
{
    public class DictionaryWelcomeMsg
    {
        private string languageShort = string.Empty;
        List<DictionaryItem> listMessage;
        public DictionaryWelcomeMsg(string LanguageShort)
        {
            this.languageShort = LanguageShort;
        }

        public string GetWelcomeMessage()
        {
            InitWelcomeDictionary();
            Random rand = new Random();
            return listMessage[rand.Next(listMessage.Count - 1)].Message;
        }

        public string GetExampleMessage()
        {
            InitExampleDictionary();
            Random rand = new Random();
            return listMessage[rand.Next(listMessage.Count - 1)].Message;
        }

        private void InitWelcomeDictionary()
        {
            this.listMessage = new List<DictionaryItem>();
            switch (languageShort)
            {
                case "en":
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Hello!" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Good day!" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Welcome!" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "How are you?" });
                    }; break;
                case "ru":
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Привет! Здесь вы можете ввести предложение или отдельное слово, например:" });
                        //listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Добрый день!" });
                        //listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Как дела?" });
                    }; break;
                case "sp":
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Hola!" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Buenas dias!" });
                    }; break;
                default:
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = ":)" });
                    }; break;
            }
        }
        private void InitExampleDictionary()
        {
            this.listMessage = new List<DictionaryItem>();
            switch (languageShort)
            {
                case "en":
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Hello!" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Good day!" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Welcome!" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "How are you?" });
                    }; break;
                case "ru":
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "стол" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "полёт" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "иди" });
                    }; break;
                case "sp":
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Hola!" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Buenas dias!" });
                    }; break;
                default:
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = ":)" });
                    }; break;
            }
        }

        public struct DictionaryItem
        {
            public string Language;
            public string Message;
        }
    }
}
