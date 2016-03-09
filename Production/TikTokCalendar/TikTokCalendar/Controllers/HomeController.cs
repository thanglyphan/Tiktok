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
	public class HomeController:Controller
	{
		Cookies cookie = new Cookies();

		public ActionResult Index(string id = "", string tags = "")
		{

            // temporary: delete EventUserStats content
            /*var db = new CalendarEventContext();
            foreach (var entity in db.EventUserStats)
            {
                db.EventUserStats.Remove(entity);
                
            }
            db.SaveChanges();*/

			StudentUser user = GetUserFromNameCourse();

			DataParser dataParser = new DataParser();
			dataParser.ParseAllData();

			if(cookie.LoadStringFromCookie("UserName") != null){
				ViewBag.Title = string.Format("Halla, {0}! Du går: {1}",cookie.LoadStringFromCookie("UserName"),cookie.LoadStringFromCookie("Usercourse"));
			}
			else
			{
				ViewBag.Title = "not logged in";
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

		

		public string CalTest(string id = "")
		{
			DataParser dataParser = new DataParser();
			dataParser.ParseAllData();
			List<CustomEventMonth> months = null;
			if (string.IsNullOrEmpty(id))
			{
				months = DataWrapper.Instance.GetEventsWithUser(new StudentUser("trotor14", SchoolCourses.Spillprogrammering));
			}
			else
			{
				months = DataWrapper.Instance.GetEventsWithName(new StudentUser("trotor14", SchoolCourses.Spillprogrammering), id);
			}


			string page = "";
			foreach (var month in months)
			{
				page += "Month: " + month.GetMonthName() + "<br>";
				foreach (var week in month.Weeks)
				{
					page += " -- Week: " + week.WeekName + "<br>";
					foreach (var evnt in week.events)
					{
						page += " ---- Evnt(" + evnt.ID + "): " + evnt.StartDateTime + " - " + evnt.Subject.Name + " (" + evnt.Subject.Code + ") - " + evnt.ClassYear + " - " + evnt.CoursesLabel + "<br>";
					}
				}
			}

			return page;
		}
		public ActionResult Test()
		{
			return View();
		}

		private bool SameYear(CalendarEvent calEvent, StudentUser user)
		{
			if (calEvent.Year < 0) return true;
			return user.ClassYear == calEvent.Year;
		}
		private String returnName()
		{
			
			string userName = (string)Session["b"];
			if (userName == "") {
				return "phatha14";
			}
			else { return userName; }
		}
		private String returnCourse()
		{
			string userCourse = (string)Session["UserCourse"];
			if (userCourse == "") {
				return "phatha14";
			}
			else { return userCourse; }
		}
		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
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

		private int FindCalEventIndex(List<CalendarEvent> list, int month)
		{
			for (int i = 0; i < list.Count; i ++)
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

            if (userName == null) {
				cookie.SaveNameToCookie(a);
			}

			return Json("fungerer",JsonRequestBehavior.AllowGet);
		}
		public JsonResult UserCourse (string a)
		{
			string userCourse = cookie.LoadStringFromCookie("UserCourse");

            if (userCourse == null) {
				cookie.SaveCourseToCookie(a);
			}
			
			return Json("fungerer",JsonRequestBehavior.AllowGet);
		}
		public JsonResult ShowDefault(string a)
		{
			cookie.SaveNameToCookie(a); //Add cookie 5 seconds
			cookie.SaveCourseToCookie(a); //Add cookie 5 seconds

			return Json("fungerer",JsonRequestBehavior.AllowGet);
		}
		public ActionResult GetVisited() //If been here, return true, else false.
		{
			if (cookie.LoadStringFromCookie("UserName") != null) {
				return Json(true,JsonRequestBehavior.AllowGet);
			}
			else {
				return Json(false,JsonRequestBehavior.AllowGet);
			}
		}
		public StudentUser GetUserFromNameCourse()
		{
			string name = "phatha14"; 
			string course = "SpillProgrammering";

			string un = cookie.LoadStringFromCookie("UserName");
			if (!string.IsNullOrEmpty(un))
			{
				name = cookie.LoadStringFromCookie("UserName");
			}
			string uc = cookie.LoadStringFromCookie("UserCourse");
			if (!string.IsNullOrEmpty(uc))
			{
				course = cookie.LoadStringFromCookie("UserCourse");
			}

			SchoolCourses schoolCourse = Course.GetCourseFromName(course);
			return new StudentUser(name, schoolCourse); //If cookie name && course == default, name = anonym14, course = "VisAlt"
		}

        public PartialViewResult UserStatUpdate(int eventid)
        {
            //System.Threading.Thread.Sleep(3000);
            var db = new CalendarEventContext();
            string userName = cookie.LoadStringFromCookie("UserName");
            //int eventID = Int32.Parse(Request.Form["eventid"]);
            if (userName.Length >= 8)
            {
                db.EventUserStats.Add(new EventUserStat { UserName = userName, EventID = eventid, GoingTime = DateTime.Now });
                db.SaveChanges();
            }
            ModelDataWrapper modelWrapper = new ModelDataWrapper();
            EUSH_global.ID_ATM = eventid;
            return PartialView("_UserStatUpdate", modelWrapper);
        }
	}

}