using System.Web.Mvc;
using TikTokCalendar.Models;

namespace TikTokCalendar.Controllers
{
	public class ErrorController : Controller
	{
		public ViewResult Index()
		{
			return View("Error");
		}
		public ViewResult NotFound()
		{
			Response.StatusCode = 200;
			return View("NotFound");
		}
	}
}