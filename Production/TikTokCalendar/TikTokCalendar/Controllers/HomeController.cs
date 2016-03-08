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
		// TODO Replace db with the DataWrapper
		private CalendarEventContext db = new CalendarEventContext();
		Cookies cookie = new Cookies();
		public ActionResult Index(string id = "", string tags = "")
		{

			// DEBUG For testing
			//Printer.Print("User: " + id);

			/*
			string name = "trotor14";
			SchoolCourses course = SchoolCourses.Spillprogrammering;
			if (id.Contains("prog"))
			{
				name = id;
				course = SchoolCourses.Programmering;
				Printer.Print("User: " + id + " prog");
			}
			else if(id.Contains("intsys"))
			{
				name = id;
				course = SchoolCourses.IntelligenteSystemer;
				Printer.Print("User: " + id + " intsys");
			}
			else if (id.Contains("idesign"))
			{
				name = id;
				course = SchoolCourses.Mobilprogrammering;
				Printer.Print("User: " + id + " idesign");
			}
			*/
			//Cookies cookie;
			//string user = cookie.LoadFromCookie("Username");
			//int program = cookie.LoadFromCookie("Program");
			//string name = "trotor14";
			//SchoolCourses course = SchoolCourses.Spillprogrammering;
			//StudentUser user = new StudentUser(name, course); // TODO Get this from cookies
			StudentUser user = GetUserFromNameCourse();
			DataParser dataParser = new DataParser();
			dataParser.ParseAllData();

			int weekOrMonthView = 0; // TODO Get this from cookies

			if (id.StartsWith("0"))
			{
				weekOrMonthView = 0;
			}
			else if (id.StartsWith("1"))
			{
				weekOrMonthView = 1;
			}

			bool weekView = (weekOrMonthView == 0);

			
			if(cookie.LoadStringFromCookie("UserName") != null){
				ViewBag.Title = string.Format("Halla, {0}! Du går: {1}",cookie.LoadStringFromCookie("UserName"),cookie.LoadStringFromCookie("Usercourse"));
			}
			else {
				ViewBag.Title = string.Format("Year: {0}, sem: {1}, valid: {2}",user.ClassYear,user.GetCurrentSemester(),user.ValidUsername(user.UserName));
			}

			var events = db.CalendarEvents.ToList();
			// TODO Refactor instances of Month into something else
			// TODO Make the year go from august to june like a schoolyear
			
			int startMonth = 8; // Starting month number
			int monthNum = startMonth;
			
			var modelWrapper = new ModelDataWrapper();
			int eventGroupCount = (weekView) ? 52 : 12;
			modelWrapper.calEvents = new List<EventMonth>(new EventMonth[eventGroupCount]);
			modelWrapper.Months = DataWrapper.Instance.GetEventsWithUser(user);
			
			//for (var i = monthNum; i < 12 + monthNum; i++)
			for (int i = 0; i < eventGroupCount; i++)
			{
				modelWrapper.calEvents[i] = new EventMonth(i+1, weekView);
				//monthNum++;
				//if (monthNum > 12) monthNum = 1;
			}

			// Used to keep track of which TimeEditIDs we have already added so we don't get duplicate events
			// TODO This could be fixed by making sure the data doesn't have any duplicates
			var addedEvents = new HashSet<int>();
			

			// TODO Make this a function call with parameters for easier accessing when adding functionality
			// Go through all course subjects
			//foreach (var item in db.CourseSubject)
			//{
			//	// Check if the coursesubject has the same ID and semester as the user
			//	//if (item.CourseID == acc.CourseID && item.Semester == acc.SemesterID)
			//	if (user.Course == SchoolCourses.VisAlt 
			//		|| (item.CourseID == (int)user.Course && item.Semester == user.GetCurrentSemester()))
			//	{

			//		// Go through all events
			//		foreach (var calEvent in events)
			//		{
			//			if (calEvent.SubjectID == item.SubjectID && SameYear(calEvent, user))
			//			{
			//				int monthIndex = -1;
			//				if (weekView)
			//				{
			//					monthIndex = calEvent.GetWeekNumber() - 1;
			//				}
			//				else
			//				{
			//					//monthIndex = calEvent.StartTime.Month - 1;
			//					for (int i = 0; i < modelWrapper.calEvents.Count; i++)
			//					{
			//						if (modelWrapper.calEvents[i].Month == calEvent.StartTime.Month)
			//						{
			//							monthIndex = i;
			//						}
			//					}
			//				}

			//				// Only add event if we haven't already added this TimeEditID already
			//				if (!addedEvents.Contains(calEvent.TimeEditID))
   //                         {
   //                             string temp = "" + tags.ToLower();
   //                             if (tags != "")
   //                             {
   //                                 if (calEvent.EventName.Contains(temp) || calEvent.Teacher.Contains(temp) || calEvent.RoomName.Contains(temp) || calEvent.Comment.Contains(temp))
   //                                 {
   //                                     Debug.WriteLine("####1!!!!!!!!!!!" + calEvent.EventName);
   //                                     modelWrapper.calEvents[monthIndex].Events.Add(calEvent);
   //                                     addedEvents.Add(calEvent.TimeEditID);
   //                                 }
   //                             }
   //                             else
   //                             {
   //                                 modelWrapper.calEvents[monthIndex].Events.Add(calEvent);
   //                                 addedEvents.Add(calEvent.TimeEditID);
   //                             }
   //                         }
			//			}
			//		}
			//	}
			//}


			// Sort that shit
			for (int i = 0; i < modelWrapper.calEvents.Count; i++)
			{
				modelWrapper.calEvents[i].Events = modelWrapper.calEvents[i].Events.OrderBy(x => x.StartTime).ToList();
			}
			//bool IsVisited = GetVisited(); //This prints debug line and return true if visited, else false.

			return View(modelWrapper);//.calEvents);
		}

		public string CalTest(string id = "")
		{
			DataParser dataParser = new DataParser();
			dataParser.ParseAllData();
			List<CustomEventMonth> months = null;
			if (string.IsNullOrEmpty(id))
			{
				months = DataWrapper.Instance.GetEventsWithUser(new StudentUser("trotor14", SchoolCourses.VisAlt));
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
						page += " ---- Evnt: " + evnt.StartDateTime + " - " + evnt.Subject.Name + " (" + evnt.Subject.Code + ") - " + evnt.ClassYear + " - " + evnt.CoursesLabel + "<br>";
					}
				}
			}

			return page;
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
            var data = new[] {"Programmering","Spillprogrammering","Intelligente systemer","Mobil apputvikling",
                "Prosjekt software engineering","Matematikk og fysikk","C++ programmering","",
                "Game AI","Embedded systems","Mobil utvikling","Ruby on rails",
                "Avansert javaprogrammering","Undersøkelsesmetoder","Enterprise programmering 2","Innlevering",
                "Forelesning","Eksamen" };
            /*ModelDataWrapper model = new ModelDataWrapper();
            foreach (var month in model.calEvents)
            {
                foreach (var item in month.Events)
                {
                }
            }*/
            //var result = data.Where(x => x.ToLower().StartsWith(search.ToLower())).ToList(); <-- starter-med-søk
            var result = data.Where(x => x.ToLower().Contains(search.ToLower())).ToList();

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
			string course = "Programmering";

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
	}

}