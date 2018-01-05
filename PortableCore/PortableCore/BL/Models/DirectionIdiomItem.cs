using PortableCore.DL;

namespace PortableCore.BL.Managers
{
    public class DirectionIdiomItem
    {
        public Language LanguageFrom { get; private set; }
        public Language LanguageTo { get; private set; }
        public string SourceFileName { get; private set; }

        public DirectionIdiomItem(Language languageFrom, Language languageTo, string sourceFileName)
        {
            this.LanguageFrom = languageFrom;
            this.LanguageTo = languageTo;
            this.SourceFileName = sourceFileName;
        }
    }
}