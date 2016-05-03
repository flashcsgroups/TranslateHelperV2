namespace PortableCore.BL.Models
{
    public class BubbleItem
    {
        public string TextFrom;
        public string TextTo;
        public string Transcription;
        public string Definition;

        public bool IsRobotResponse { get; internal set; }
    }
}
