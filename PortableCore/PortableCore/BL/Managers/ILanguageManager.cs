using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public interface ILanguageManager
    {
        Language GetItemForId(int Id);
        Language GetItemForNameEng(string name);
    }
}