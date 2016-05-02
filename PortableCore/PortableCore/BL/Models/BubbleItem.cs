namespace PortableCore.BL.Models
{
    public class BubbleItem
    {
        public string TextFrom;
        public string TextTo;

        public bool IsRobotResponse { get; internal set; }
        public string Text { get; internal set; }
        public string UserNameText { get; internal set; }
    }
}
