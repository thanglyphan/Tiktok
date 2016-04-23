namespace TikTokCalendar.Models
{
	public class WebpageErrorContainer
	{
		public WebpageErrorContainer(string message, string linkUrl = "~/Home/Index")
		{
			ErrorMessage = message;
			Link = linkUrl;
		}

		public string ErrorMessage { get; private set; }
		public string Link { get; private set; }
	}
}