using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace TikTokCalender.Controllers
{
	public class UserController:Controller
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
		public ActionResult Login(TikTokCalendar.Models.Account user)
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
            return RedirectToAction("Index","Home");
		}
	}
}