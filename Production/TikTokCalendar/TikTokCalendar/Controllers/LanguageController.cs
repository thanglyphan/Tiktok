using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TikTokCalendar.Controllers
{
    public class LanguageController : Controller
    {
		public void Set(string language)
		{
			// Save the culture given through the action
			CultureManager.SaveCulture(HttpContext.Response, language, 1);

			// Redirect back
			HttpContext.Response.Redirect(HttpContext.Request.UrlReferrer.AbsolutePath);
		}
	}
}