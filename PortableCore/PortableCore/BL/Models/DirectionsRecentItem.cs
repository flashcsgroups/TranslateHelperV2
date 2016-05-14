namespace PortableCore.BL.Models
{
	public class DirectionsRecentItem
    {
        public int ChatId { get; internal set; }
		public string LangToFlagImageResourcePath { get; internal set; }
        public string LangTo { get; internal set; }
        public string LangFrom { get; internal set; }
        public int CountOfAllMessages { get; internal set; }
        public int CountOfNewMessages { get; internal set; }
    }
}
