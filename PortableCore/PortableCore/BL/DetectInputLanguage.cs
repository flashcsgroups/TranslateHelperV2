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
        public enum Language
        {
            Unknown = 0, English = 1, Russian = 2 
        }

        public DetectInputLanguage(string inputString)
        {
            this.inputString = inputString.Trim();
        }
        public bool NeedInvertDirection(TranslateDirection currentDirection)
        {
            //DetectInputLanguage detect = new DetectInputLanguage(originalText);
            DetectInputLanguage.Language result = Detect();
            return (result != DetectInputLanguage.Language.Unknown) && !currentDirection.IsFrom(result);
            /*if ((result != DetectInputLanguage.Language.Unknown) && !currentDirection.IsFrom(result))
            {
                currentDirection.Invert();
            };*/
        }

        public Language Detect()
        {
            var result = Language.Unknown;
            if(inputString.Length > 0)
            {
                var arrBytes = Encoding.UTF8.GetBytes(inputString);
                byte firstSymbol = arrBytes[0];
                result = byteIsEnglishCode(firstSymbol);
                if(result == Language.Unknown)
                {
                    result = byteIsRussianCode(firstSymbol);
                }
            }
            return result;
        }

        private Language byteIsRussianCode(byte firstSymbol)
        {
            //ToDo:Нужно разобраться как определить в UTF8 русский, пока в любом случае других языков нет, но при добавлении новых все-равно придется разобраться
            return Language.Russian;
        }

        private Language byteIsEnglishCode(byte firstSymbol)
        {
            var check1 = Enumerable.Range(65, 25).Contains(firstSymbol);
            var check2 = Enumerable.Range(97, 25).Contains(firstSymbol);
            return check1 || check2 ? Language.English : Language.Unknown;
        }
    }
}
