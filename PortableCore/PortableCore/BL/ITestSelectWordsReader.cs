using System;
using System.Collections.Generic;
using PortableCore.DL;

namespace PortableCore.BL
{
    public interface ITestSelectWordsReader
    {
        List<string> GetIncorrectVariants(string correctWord, int countOfIncorrectWords, TranslateDirection direction);
        List<Favorites> GetRandomFavorites(int countOfWords, TranslateDirection direction);
        Tuple<string, string> GetNextWord(int translatedExpressionID);
    }
}