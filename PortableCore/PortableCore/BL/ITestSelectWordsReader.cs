using System;
using System.Collections.Generic;
using PortableCore.DL;
using PortableCore.BL.Models;

namespace PortableCore.BL
{
    public interface ITestSelectWordsReader
    {
        List<string> GetIncorrectVariants(int excludeCorrectSourceId, int countOfIncorrectWords, TranslateDirection direction);
        List<FavoriteItem> GetRandomFavorites(int countOfWords, TranslateDirection direction);
        int GetCountDifferenceSources(TranslateDirection direction);
        Tuple<string, string> GetNextWord(int translatedExpressionID);
    }
}