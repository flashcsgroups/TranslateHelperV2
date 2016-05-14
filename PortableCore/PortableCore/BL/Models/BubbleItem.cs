using PortableCore.DL;

namespace PortableCore.BL.Models
{
    public class BubbleItem
    {
        public string TextFrom;
        public string TextTo;
        public string Transcription;
        public string Definition;
        public Language LanguageFrom;
        public Language LanguageTo;

        public bool IsRobotResponse { get; internal set; }
        public int HistoryRowId { get; internal set; }
    }
}
