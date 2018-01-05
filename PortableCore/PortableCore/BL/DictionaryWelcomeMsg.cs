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
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Hello! You can write word or sentences for translate, for example:" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Good day! You can write word or sentences for translate, for example:" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Welcome! You can write word or sentences for translate, for example:" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "How are you? Now you can write word or sentences for translate, for example:" });
                    }; break;
                case "ru":
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Привет! Здесь вы можете ввести предложение или отдельное слово, например:" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Добрый день! Просто напишите то, что хотите перевести, например:" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "Здорово! Хорошо, что вы здесь. В чате вы можете написать то, что хотите превести, например: " });
                    }; break;
                case "es":
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
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "test" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "morning" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "table" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "sunshine" });
                    }; break;
                case "ru":
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "проверка" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "человек" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "песня" });
                    }; break;
                case "es":
                    {
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "personas" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "gato" });
                        listMessage.Add(new DictionaryItem() { Language = languageShort, Message = "madera" });
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
