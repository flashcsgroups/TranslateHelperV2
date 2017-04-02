using System;
using System.Collections.Generic;
using PortableCore.DL;
using PortableCore.BL.Models;

namespace PortableCore.BL
{
    public interface ITestSelectWordsReader
    {
        List<TestWordItem> GetIncorrectVariants(int countOfIncorrectWords, int chatId, int languageFromId, string correctWord);
        RandomWordsList GetRandomFavorites(int countOfWords, int chatId);
        Tuple<string, string> GetNextWord(int translatedExpressionID);
    }
}