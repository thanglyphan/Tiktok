using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikTokCalendar.Models
{
	public class WebpageErrorContainer
	{
		public string ErrorMessage { get; private set; }
		public string Link { get; private set; }

		public WebpageErrorContainer(string message, string linkUrl = "~/Home/Index")
		{
			ErrorMessage = message;
			Link = linkUrl;
		}
	}
}