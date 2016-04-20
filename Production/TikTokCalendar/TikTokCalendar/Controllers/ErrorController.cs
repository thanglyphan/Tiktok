using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TikTokCalendar.Models;

namespace TikTokCalendar.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index()
        {
			return View();
			return View(new WebpageErrorContainer("An error occured!"));
		}

		public ActionResult NotFound()
		{
			Response.StatusCode = 200; // OK status code
			return View(new WebpageErrorContainer("404, page not found!"));
		}

		public ActionResult InternalServer()
		{
			Response.StatusCode = 200; // OK status code
			return View(new WebpageErrorContainer("An internal server error occured! If the problem persists, please contact the administrator at admin@tiktokcal.com"));
		}
	}
}