using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PortableCore.DL;
using PortableCore.WS;

namespace PortableCore.Tests.Mocks
{
    internal class MockClientApi : IApiClient
    {
        public async Task<List<int>> GetChangedIDsFromServer(string tableName, DateTime localMaxTimeStampString)
        {
            List<int> result = new List<int>() { 100, 101, 102 };
            return await Task.FromResult<List<int>>(result);
        }

        public async Task<List<Idiom>> GetIdiomsFromServer(List<int> iDs)
        {
            var eng = 2;
            var rus = 1;
            List<Idiom> result = new List<Idiom>()
            {
                new Idiom (){ ID=100, UpdateDate = DateTime.Parse("2017-10-14"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 6, DescriptionTextFrom = "DescriptionTextFrom", DescriptionTextTo = "DescriptionTextTo", TextFrom = "TextFrom", TextTo = "TextTo", ExampleTextFrom="ExampleTextFrom", ExampleTextTo="ExampleTextTo"},
                new Idiom (){ ID=101, UpdateDate = DateTime.Parse("2017-10-14"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 6, DescriptionTextFrom = "DescriptionTextFrom", DescriptionTextTo = "DescriptionTextTo", TextFrom = "TextFrom", TextTo = "TextTo", ExampleTextFrom="ExampleTextFrom", ExampleTextTo="ExampleTextTo"},
                new Idiom (){ ID=102, UpdateDate = DateTime.Parse("2017-10-14"), LanguageFrom = eng, LanguageTo = rus, CategoryID = 6, DescriptionTextFrom = "DescriptionTextFrom", DescriptionTextTo = "DescriptionTextTo", TextFrom = "TextFrom", TextTo = "TextTo", ExampleTextFrom="ExampleTextFrom", ExampleTextTo="ExampleTextTo"}
            };
            return await Task.FromResult<List<Idiom>>(result);
        }
    }
}