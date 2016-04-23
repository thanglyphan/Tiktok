using System.Web.Mvc;
using System.Web.Security;
using TikTokCalendar.Models;

namespace TikTokCalender.Controllers
{
	public class UserController : Controller
	{
		//
		// GET: /User/
		public ActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Login(Account user)
		{
			//if (ModelState.IsValid) {
			//	if (user.IsValid(user.UserName,user.Password)) {
			//		FormsAuthentication.SetAuthCookie(user.UserName,user.RememberMe);
			//		return RedirectToAction("Index","Home");
			//	}
			//	else {
			//		ModelState.AddModelError("","Login data is incorrect!");
			//	}
			//}
			return View(user);
		}

		public ActionResult Logout()
		{
			Session.Clear();
			FormsAuthentication.SignOut();
			return RedirectToAction("Index", "Home");
		}
	}
}