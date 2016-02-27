using System;
using System.Collections.Generic;
using PortableCore.DL;

namespace PortableCore.BL
{
    public interface ITestSelectWordsReader
    {
        List<string> GetIncorrectVariants(int excludeCorrectSourceId, int countOfIncorrectWords, TranslateDirection direction);
        List<FavoriteItem> GetRandomFavorites(int countOfWords, TranslateDirection direction);
        int GetCountDifferenceSources();
        Tuple<string, string> GetNextWord(int translatedExpressionID);
    }
}