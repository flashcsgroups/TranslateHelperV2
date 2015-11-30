using System.Threading.Tasks;
using TranslateHelper.Core.WS;

namespace TranslateHelper.Core.BL.Contracts
{
    public interface IRequestTranslateString
    {
        Task<TranslateRequestResult> Translate(string sourceString, string direction);
    }
}