using PortableCore.BL.Managers;
using PortableCore.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL
{
    public class DetectInputLanguage
    {
        private string inputString;
        private ILanguageManager languageManager;
        /*public enum LanguageEnum
        {
            Unknown = 0, English = 1, Russian = 2 
        }*/

        public DetectInputLanguage(string inputString, ILanguageManager languageManager)
        {
            this.inputString = inputString.Trim();
            this.languageManager = languageManager;
        }
        /*public bool NeedInvertDirection(TranslateDirection currentDirection)
        {
            //DetectInputLanguage detect = new DetectInputLanguage(originalText);
            DetectInputLanguage.LanguageEnum result = Detect();
            return (result != DetectInputLanguage.LanguageEnum.Unknown) && !currentDirection.IsFrom(result);
            if ((result != DetectInputLanguage.Language.Unknown) && !currentDirection.IsFrom(result))
            {
                currentDirection.Invert();
            };
        }*/

        //Работает только с русским!
        public Language Detect()
        {
            var result = languageManager.DefaultLanguage();
            if(inputString.Length > 0)
            {
                var arrBytes = Encoding.UTF8.GetBytes(inputString);
                byte firstSymbol = arrBytes[0];
                if(byteIsEnglishCode(firstSymbol))
                {
                    result = languageManager.GetItemForNameEng("English");
                } else
                {
                    result = languageManager.GetItemForNameEng("Russian");
                }
                /*if(result == LanguageEnum.Unknown)
                {
                    result = byteIsRussianCode(firstSymbol);
                }*/
            }
            return result;
        }

        /*private bool byteIsRussianCode(byte firstSymbol)
        {
            //ToDo:Нужно разобраться как определить в UTF8 русский, пока в любом случае других языков нет, но при добавлении новых все-равно придется разобраться
            return LanguageEnum.Russian;
        }*/

        private bool byteIsEnglishCode(byte firstSymbol)
        {
            var check1 = Enumerable.Range(65, 25).Contains(firstSymbol);
            var check2 = Enumerable.Range(97, 25).Contains(firstSymbol);
            return check1 || check2;
        }
    }
}
