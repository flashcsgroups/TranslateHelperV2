using PortableCore.BL.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortableCore.BL.Presenters
{
    public class TestResultPresenter
    {
        public int CurrentChatId { get; private set; }
        private int countOfRightWords;
        private int countOfTotalWords;
        private ITestResultView view;

        public TestResultPresenter(ITestResultView view, int currentChatId, int countOfRightWords, int countOfTotalWords)
        {
            this.view = view;
            this.CurrentChatId = currentChatId;
            this.countOfRightWords = countOfRightWords;
            this.countOfTotalWords = countOfTotalWords;
        }


        public string GetTextResult()
        {
            return countOfRightWords.ToString() + " from " + countOfTotalWords.ToString();
        }
    }
}
