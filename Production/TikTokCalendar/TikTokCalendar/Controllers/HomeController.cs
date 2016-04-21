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

        [ValidateInput(false)]
        public ActionResult Index(string Email, string Password, string tags = "", string lecture = "", string assignment = "", string exam = "", bool filtered = false)
		{
			bool lec = false, ass = false, exa = false;
            if (filtered)
            {
                if (lecture.Length > 0) lec = true;
                if (assignment.Length > 0) ass = true;
                if (exam.Length > 0) exa = true;

                //Check if actually empty page before continuing
                if (!lec && !ass && !exa && tags == "")
                {
                    ModelDataWrapper emptyWrap;
                    emptyWrap = new ModelDataWrapper("", false, false, false);
                    emptyWrap.isFiltered = true;
                    return View(emptyWrap);
                }
            }
            else
            {
                lec = true;
                ass = true;
                exa = true;
            }

            // Make a new ModelDataWrapper with the events based on the user, tags, and filters
            ModelDataWrapper modelWrapper;
            if (string.IsNullOrEmpty(tags))
            {
                modelWrapper = new ModelDataWrapper(lec, ass, exa);
            }
            else
            {
                modelWrapper = new ModelDataWrapper(tags, lec, ass, exa);
            }
			modelWrapper.Months = DataWrapper.Instance.GetEventsWithName(InitUser(Email, Password, tags), tags, lec, ass, exa);

            // Show event count
            if (!(lec && ass && exa) && (filtered || tags.Length > 0))
            {
                modelWrapper.isFiltered = true;
            }
			
			// Send the model to the view
			return View(modelWrapper);
		}

        public StudentUser InitUser(string userName, string password, string tags)
        {
            // Get the user from cookies
            StudentUser user = null;

            // Parse all the JSON data
            DataParser dataParser = new DataParser();
            dataParser.ParseAllData();

	        if (!string.IsNullOrEmpty(userName))
	        {
		        user = DataWrapper.Instance.GetUser(userName, password);
		        if (user != null)
		        {
			        cookie.SaveNameToCookie(user.UserName);
			        cookie.SaveCourseToCookie(user.Course.ToString());
			        cookie.SaveYearToCookies(user.GetYearAsText());
		        }
	        }
	        else
	        {
		        //user = GetUserFromNameCourse();
	        }
			
			// DEBUG Set the page title
            if (user != null)
            {
                ViewBag.Title = string.Format("{0}[{1}-{2}]: {3} [{4}]", 
					user.UserName, 
					user.ClassYear, 
					user.GetYearAsText(), 
					user.Course, 
					tags);
            }
            else
            {
                ViewBag.Title = "Not logged in";
            }
			// set default user here
            return user;
        }

		public void AccountLogin(string Email, string Password)
		{
			StudentUser a = DataWrapper.Instance.GetUser(Email,Password);

			if (a != null) {
				cookie.SaveNameToCookie(a.Email);
				cookie.SaveCourseToCookie(a.Course.ToString());
				cookie.SaveYearToCookies(a.GetYearAsText());
			}
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
			months = DataWrapper.Instance.GetEventsWithName(u, id, true, true, true);

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
		private string returnName()
		{

			string userName = (string)Session["b"];
			if (userName == "")
			{
				return "phatha14";
			}
			else { return userName; }
		}
		private string returnCourse()
		{
			string userCourse = (string)Session[Cookies.UserNameCookieKey];
			if (userCourse == "")
			{
				return "phatha14";
			}
			else { return userCourse; }
		}

        public ActionResult Mobile(string id = "", string tags = "", bool lecture = true, bool assignment = true, bool exam = true)
		{
			StudentUser user = GetUserFromNameCourse();
			DataParser dataParser = new DataParser();
			dataParser.ParseAllData();

			if (cookie.LoadStringFromCookie(Cookies.UserNameCookieKey) != null)
			{
				ViewBag.Title = string.Format("Halla, {0}! Du går: {1}", cookie.LoadStringFromCookie(Cookies.UserNameCookieKey), cookie.LoadStringFromCookie(Cookies.CourseCookieKey));
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
				modelWrapper.Months = DataWrapper.Instance.GetEventsWithName(user, tags, true, true, true);
			}

			return View(modelWrapper);//.calEvents);
		}

		[HttpGet]
		public ActionResult Rooms()
		{
			Debug.WriteLine("Rooms()");
			var modelWrapper = new ModelDataWrapper();
			return View("Rooms", modelWrapper);

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

        [ValidateInput(false)]
        public JsonResult AutoComplete(string search)
		{
            /* // static test data (delete eventually)
            var data = new[] {"Programmering","Spillprogrammering","Intelligente systemer","Mobil apputvikling",
                "Prosjekt software engineering","Matematikk og fysikk","C++ programmering","",
                "Game AI","Embedded systems","Mobil utvikling","Ruby on rails",
                "Avansert javaprogrammering","Undersøkelsesmetoder","Enterprise programmering 2","Innlevering",
                "Forelesning","Eksamen" };
            */
            string temp = DataWrapper.Instance.FilterCharacters(search);
            var list = new List<string>();

            if (Session["keywords"] == null)
            {
                StudentUser user = GetUserFromNameCourse();
                list = DataWrapper.Instance.GetUserKeywords(user);
                list.RemoveAll(item => item == null);
                Session["keywords"] = list;   
            }
            else
            {
                list = (List<string>)Session["keywords"]; 
            }
            
            var result = list.Where(x => x.Contains(temp.ToLower())).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
		}
		public JsonResult UserName(string a)
		{
			string userName = cookie.LoadStringFromCookie(Cookies.UserNameCookieKey);

			if (userName == null)
			{
				cookie.SaveNameToCookie(a);
			}

			return Json("fungerer", JsonRequestBehavior.AllowGet);
		}
		public JsonResult UserCourse(string a)
		{
			string userCourse = cookie.LoadStringFromCookie(Cookies.CourseCookieKey);

			if (userCourse == null)
			{
				cookie.SaveCourseToCookie(a);
			}
            // new user means new custom keywords
            Session["keywords"] = null;
            return Json("fungerer", JsonRequestBehavior.AllowGet);
		}

		public JsonResult UserYear(string a)
		{
			string userYear = cookie.LoadStringFromCookie(Cookies.YearCookieKey);
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
			if (cookie.LoadStringFromCookie(Cookies.UserNameCookieKey) != null)
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
            // to show all keywords when no user
            Session["keywords"] = null;
            return Json("hei", JsonRequestBehavior.AllowGet);
		}
		public StudentUser GetUserFromNameCourse()
		{
			string name = "";
			string course = "";
			string year = "";

			string cookieYear = cookie.LoadStringFromCookie(Cookies.YearCookieKey);
			if (!string.IsNullOrEmpty(cookieYear))
			{
				year = cookieYear;
			}
			else
			{
				year = "second";
			}

			string cookieUserName = cookie.LoadStringFromCookie(Cookies.UserNameCookieKey);
			if (!string.IsNullOrEmpty(cookieUserName))
			{
				name = cookieUserName;
			}
			else {
				name = "NO NAME";
			}
			string cookieCourse = cookie.LoadStringFromCookie(Cookies.CourseCookieKey);
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
			string userName = cookie.LoadStringFromCookie(Cookies.UserNameCookieKey);
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

        public ActionResult GetMessage()
        {
            string message = "Welcome";
            return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }

}