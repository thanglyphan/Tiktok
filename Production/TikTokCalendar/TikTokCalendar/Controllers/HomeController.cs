using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TikTokCalendar.DAL;
using TikTokCalendar.Models;
using System.Diagnostics;

namespace TikTokCalendar.Controllers
{
	public class HomeController : Controller
	{
		private readonly Cookies cookie = new Cookies();

		public ActionResult Index(string id = "", string tags = "")
		{
			// Get the user from cookies
			StudentUser user = GetUserFromNameCourse();

			// Parse all the JSON data
			DataParser dataParser = new DataParser();
			dataParser.ParseAllData();

			// DEBUG Set the page title
			var username = cookie.LoadStringFromCookie("UserName");
			if (!string.IsNullOrEmpty(username))
			{
				ViewBag.Title = string.Format("{0}[{1}]: {2} [{3}]", cookie.LoadStringFromCookie("UserName"), cookie.LoadStringFromCookie("Year"), cookie.LoadStringFromCookie("Usercourse"), tags);
			}
			else
			{
				ViewBag.Title = "Not logged in";
			}

            // Make a new ModelDataWrapper with the events based on the user and the tags
            ModelDataWrapper modelWrapper;
            if (string.IsNullOrEmpty(tags))
            {
                modelWrapper = new ModelDataWrapper();
            }
            else
            {
                modelWrapper = new ModelDataWrapper(tags);
            }
			modelWrapper.Months = DataWrapper.Instance.GetEventsWithName(user, tags);

			// Send the model to the view
			return View(modelWrapper);
		}

        public string CalTest(string id = "")
		{
			DataParser dataParser = new DataParser();
			dataParser.ParseAllData();
			List<CustomEventMonth> months = null;
			Random rnd = new Random();
			SchoolCourses c = (SchoolCourses)rnd.Next(1, 10);
			//c = SchoolCourses.Programmering;
			StudentUser u = new StudentUser("tordtest", c, "second");
			months = DataWrapper.Instance.GetEventsWithName(u, id);

			string page = string.Format("{0} - {1}, {2}\n", u.UserName, u.Course, u.ClassYear);
			foreach (var month in months)
			{
				page += "Month: " + month.GetMonthName() + "(" + month.GetEventTypeCount(EventType.Forelesning) + ")" + "<br>";
				foreach (var week in month.Weeks)
				{
					page += " -- Week: " + week.WeekName + "(" + week.GetEventTypeCount(EventType.Forelesning) + ")" + "<br>";
					foreach (var evnt in week.events)
					{
						page += " ---- Evnt(" + evnt.ID + "): " + evnt.StartDateTime + " - " + evnt.Subject.Name + " (" + evnt.Subject.Code + ") - " + evnt.EventTypeLabel + " - [" + evnt.YearLabelTest + "] - " + evnt.CoursesLabel + "<br>";
					}
				}
			}

			return page;
		}


		public string GetRoom()
		{
			string s = "";
			foreach (var room in GetRooms()) {
				s += room + "<br>";
			}
			return s;
		}

		private string[] GetRooms()
		{
			return new[] {"Rom 40", "Rom 41", "Rom 82", "Rom 83", "Vrimle", "Auditoriet"};
		}

		private bool SameYear(CalendarEvent calEvent, StudentUser user)
		{
			if (calEvent.Year < 0) return true;
			return user.ClassYear == calEvent.Year;
		}
		private String returnName()
		{

			string userName = (string)Session["b"];
			if (userName == "")
			{
				return "phatha14";
			}
			else { return userName; }
		}
		private String returnCourse()
		{
			string userCourse = (string)Session["UserCourse"];
			if (userCourse == "")
			{
				return "phatha14";
			}
			else { return userCourse; }
		}

        public ActionResult Mobile(string id = "", string tags = "")
		{
			StudentUser user = GetUserFromNameCourse();
			DataParser dataParser = new DataParser();
			dataParser.ParseAllData();

			if (cookie.LoadStringFromCookie("UserName") != null)
			{
				ViewBag.Title = string.Format("Halla, {0}! Du går: {1}", cookie.LoadStringFromCookie("UserName"), cookie.LoadStringFromCookie("Usercourse"));
			}
			else
			{
				ViewBag.Title = "tiktok";
				//ViewBag.Title = string.Format("Year: {0}, sem: {1}, valid: {2}",user.ClassYear,user.GetCurrentSemester(),user.ValidUsername(user.UserName));
			}

			var modelWrapper = new ModelDataWrapper();
			if (string.IsNullOrEmpty(tags))
			{
				modelWrapper.Months = DataWrapper.Instance.GetEventsWithUser(user);
			}
			else
			{
				modelWrapper.Months = DataWrapper.Instance.GetEventsWithName(user, tags);
			}

			return View(modelWrapper);//.calEvents);
		}

        public ActionResult Rooms(string a)
        {
            return View("Home/Rooms", new ModelDataWrapper()); //.calEvents);
        }

        private int FindCalEventIndex(List<CalendarEvent> list, int month)
		{
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].StartTime.Month == month)
				{
					return i;
				}
			}
			return -1;
		}

		public JsonResult AutoComplete(string search)
		{
			/*
            var data = new[] {"Programmering","Spillprogrammering","Intelligente systemer","Mobil apputvikling",
                "Prosjekt software engineering","Matematikk og fysikk","C++ programmering","",
                "Game AI","Embedded systems","Mobil utvikling","Ruby on rails",
                "Avansert javaprogrammering","Undersøkelsesmetoder","Enterprise programmering 2","Innlevering",
                "Forelesning","Eksamen" };
            */
			StudentUser user = GetUserFromNameCourse();
			List<string> list = DataWrapper.Instance.GetUserKeywords(user);
			list.Add("Innlevering");
			list.Add("Eksamen");
			list.Add("Forelesning");
			list.RemoveAll(item => item == null);
			//var result = data.Where(x => x.ToLower().StartsWith(search.ToLower())).ToList(); <-- starter-med-søk
			var result = list.Where(x => x.ToLower().Contains(search.ToLower())).ToList();

			return Json(result, JsonRequestBehavior.AllowGet);
		}
		public JsonResult UserName(string a)
		{
			string userName = cookie.LoadStringFromCookie("UserName");

			if (userName == null)
			{
				cookie.SaveNameToCookie(a);
			}

			return Json("fungerer", JsonRequestBehavior.AllowGet);
		}
		public JsonResult UserCourse(string a)
		{
			string userCourse = cookie.LoadStringFromCookie("UserCourse");

			if (userCourse == null)
			{
				cookie.SaveCourseToCookie(a);
			}

			return Json("fungerer", JsonRequestBehavior.AllowGet);
		}

		public JsonResult UserYear(string a)
		{
			string userYear = cookie.LoadStringFromCookie("Year");
			if (userYear == null)
			{
				cookie.SaveYearToCookies(a);
			}
			return Json("fungerer", JsonRequestBehavior.AllowGet);
		}

		public JsonResult ShowDefault(string a)
		{
			cookie.SaveNameToCookie(a); //Add cookie 5 seconds
			cookie.SaveCourseToCookie(a); //Add cookie 5 seconds

			return Json("fungerer", JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetVisited() //If been here, return true, else false.
		{
			if (cookie.LoadStringFromCookie("UserName") != null)
			{
				return Json(true, JsonRequestBehavior.AllowGet);
			}
			else {
				return Json(false, JsonRequestBehavior.AllowGet);
			}
		}
		public JsonResult DeleteCookies()
		{
			cookie.DeleteCookies();
			return Json("hei", JsonRequestBehavior.AllowGet);
		}
		public StudentUser GetUserFromNameCourse()
		{
			string name = "";
			string course = "";
			string year = "";

			string cookieYear = cookie.LoadStringFromCookie("Year");
			if (!string.IsNullOrEmpty(cookieYear))
			{
				year = cookieYear;
			}
			else
			{
				year = "second";
			}

			string cookieUserName = cookie.LoadStringFromCookie("UserName");
			if (!string.IsNullOrEmpty(cookieUserName))
			{
				name = cookieUserName;
			}
			else {
				name = "NO NAME";
			}
			string cookieCourse = cookie.LoadStringFromCookie("UserCourse");
			if (!string.IsNullOrEmpty(cookieCourse))
			{
				course = cookieCourse;
			}
			else {
				course = "VisAlt";
			}


			SchoolCourses schoolCourse = Course.GetCourseFromName(course);
			return new StudentUser(name, schoolCourse, year); //If cookie name && course == default, name = anonym14, course = "VisAlt"
		}

		public PartialViewResult UserStatUpdate(int eventid, bool attend)
		{
			var db = new CalendarEventContext();
			string userName = cookie.LoadStringFromCookie("UserName");
			//int eventID = Int32.Parse(Request.Form["eventid"]);
			if (userName != "" || userName != null)
			{
				if (attend)
				{
					bool alreadyGoing = false;
					foreach (EventUserStat eus in db.EventUserStats)
					{
						if (eus.UserName == userName && eus.EventID == eventid
							&& !eus.Attend) // just in case
						{
							eus.Attend = true;
							alreadyGoing = true;
							// here: calculate score based on eus.GoingTime, ADD "SCORE" TO ACCOUNT.CS?
						}
					}
					if (!alreadyGoing)
					{
						db.EventUserStats.Add(new EventUserStat { UserName = userName, EventID = eventid, GoingTime = DateTime.Now, Attend = true });
					}
				}
				else
				{
					db.EventUserStats.Add(new EventUserStat { UserName = userName, EventID = eventid, GoingTime = DateTime.Now });
				}
				db.SaveChanges();
			}
			else
			{
				// possibly return another view showing error message (maybe do more validation like this?)
			}

			ModelDataWrapper modelWrapper = new ModelDataWrapper();
			EUSH_global.ID_ATM = eventid;
			return PartialView("_UserStatUpdate", modelWrapper);
		}
	}

}