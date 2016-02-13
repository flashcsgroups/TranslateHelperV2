using System.Threading.Tasks;
using PortableCore.WS;

namespace PortableCore.BL.Contracts
{
    public interface IRequestTranslateString
    {
        Task<TranslateRequestResult> Translate(string sourceString, string direction);
    }
}