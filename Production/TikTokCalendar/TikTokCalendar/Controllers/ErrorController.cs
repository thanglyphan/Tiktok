using System.Web.Mvc;
using TikTokCalendar.Models;

namespace TikTokCalendar.Controllers
{
	public class ErrorController : Controller
	{
		public PartialViewResult Index()
		{
			return PartialView();
		}
		public PartialViewResult NotFound()
		{
			Response.StatusCode = 200;
			return PartialView();
		}
	}
}