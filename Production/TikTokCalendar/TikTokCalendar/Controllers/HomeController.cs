using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TikTokCalendar.DAL;
using TikTokCalendar.Models;

namespace TikTokCalendar.Controllers
{
	public class HomeController : Controller
	{
		private readonly Cookies cookie = new Cookies();
		private readonly DataParser dataParser = new DataParser();

		[HttpPost]
		[ValidateInput(false)]
		public void LogIn(string username = "", string password = "")
		{
			// Parse all the JSON data
			dataParser.ParseAllData();

			// Make a new ModelDataWrapper with the events based on the user, tags, and filters
			var user = InitUser(username, password, "");
			var failedLogin = false;
			try
			{
				if (user == null || !DataWrapper.Instance.IsValidUser(user))
				{
					failedLogin = true;
					user = new StudentUser("NO NAME", SchoolCourses.VisAlt, "NaN");
				}
			}
			catch (HttpRequestValidationException e)
			{
				Console.WriteLine(e.ToString());
			}

			var modelWrapper = CreateModelDataWrapper(DataWrapper.Instance.GetEventsWithName(user), user);
			modelWrapper.FailedLogin = failedLogin;

			CultureManager.UpdateCulture(HttpContext.Request);
			// Send the model to the view
			Session["keywords"] = null;

			HttpContext.Response.Redirect(HttpContext.Request.UrlReferrer.AbsolutePath);
		}

		public void LogOut()
		{
			cookie.DeleteCookies();
			Session["keywords"] = null;
			dataParser.ParseAllData();

			var user = new StudentUser("Not logged in", "", "", -1, SchoolCourses.VisAlt);
			var modelWrapper = CreateModelDataWrapper(DataWrapper.Instance.GetEventsWithName(user), user);
			CultureManager.UpdateCulture(HttpContext.Request);
			HttpContext.Response.Redirect(HttpContext.Request.UrlReferrer.AbsolutePath);
		}

		[ValidateInput(false)]
		public ActionResult Index(string Email, string Password, string tags = "", string lecture = "", string assignment = "",
			string exam = "", bool filtered = false)
		{
			// Parse all the JSON data
			dataParser.ParseAllData();
			var rooms = new List<Room>();
			foreach (var room in DataWrapper.Instance.Rooms)
			{
				rooms.Add(room.Value);
			}

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
					emptyWrap.Rooms = rooms;
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

			var user = InitUser(Email, Password, tags);

			modelWrapper.Months = DataWrapper.Instance.GetEventsWithName(user, tags, lec, ass, exa);
			modelWrapper.User = user;
			modelWrapper.CultureText = CultureManager.GetSavedCultureOrDefault(HttpContext.Request);
			modelWrapper.Rooms = rooms;

			// Show event count
			if (!(lec && ass && exa) && (filtered || tags.Length > 0))
			{
				modelWrapper.isFiltered = true;
			}
			CultureManager.UpdateCulture(HttpContext.Request);
			// Send the model to the view
			return View(modelWrapper);
		}

		private ModelDataWrapper CreateModelDataWrapper(List<CustomEventMonth> months, StudentUser user)
		{
			var modelWrapper = new ModelDataWrapper();
			modelWrapper.Months = months;
			modelWrapper.User = user;
			modelWrapper.CultureText = CultureManager.GetSavedCultureOrDefault(HttpContext.Request);

			// Set availible rooms
			var rooms = new List<Room>();
			foreach (var room in DataWrapper.Instance.Rooms)
			{
				rooms.Add(room.Value);
			}
			modelWrapper.Rooms = rooms;

			return modelWrapper;
		}

		public StudentUser InitUser(string userName, string password, string tags)
		{
			// Get the user from cookies
			StudentUser user = null;

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
				user = GetUserFromNameCourse();
			}

			return user;
		}

		public string CalTest(string id = "")
		{
			var dataParser = new DataParser();
			dataParser.ParseAllData();
			List<CustomEventMonth> months = null;
			var rnd = new Random();
			var c = (SchoolCourses)rnd.Next(1, 10);
			//c = SchoolCourses.Programmering;
			var u = new StudentUser("tordtest", c, "second");
			months = DataWrapper.Instance.GetEventsWithName(u, id, true, true, true);

			var page = string.Format("{0} - {1}, {2}\n", u.UserName, u.Course, u.ClassYear);
			foreach (var month in months)
			{
				page += "Month: " + month.GetMonthName() + "(" + month.GetEventTypeCount(EventType.Forelesning) + ")" + "<br>";
				foreach (var week in month.Weeks)
				{
					page += " -- Week: " + week.WeekName + "(" + week.GetEventTypeCount(EventType.Forelesning) + ")" + "<br>";
					foreach (var evnt in week.events)
					{
						page += " ---- Evnt(" + evnt.ID + "): " + evnt.StartDateTime + " - " + evnt.Subject.Name + " (" +
								evnt.Subject.Code + ") - " + evnt.EventTypeLabel + " - [" + evnt.YearLabelTest + "] - " +
								evnt.CoursesLabel + "<br>";
					}
				}
			}

			return page;
		}

		public ActionResult Mobile(string id = "", string tags = "", bool lecture = true, bool assignment = true,
			bool exam = true)
		{
			var user = GetUserFromNameCourse();
			//var dataParser = new DataParser();
			dataParser.ParseAllData();

			var modelWrapper = new ModelDataWrapper();
			if (string.IsNullOrEmpty(tags))
			{
				modelWrapper.Months = DataWrapper.Instance.GetEventsWithUser(user);
			}
			else
			{
				modelWrapper.Months = DataWrapper.Instance.GetEventsWithName(user, tags, true, true, true);
			}
			var rooms = new List<Room>();
			foreach (var room in DataWrapper.Instance.Rooms)
			{
				rooms.Add(room.Value);
			}
			modelWrapper.Rooms = rooms;
			modelWrapper.User = user;
			return View(modelWrapper); //.calEvents);
		}

		[ValidateInput(false)]
		public JsonResult AutoComplete(string search)
		{
			var temp = DataWrapper.Instance.FilterCharacters(search);
			var list = new List<string>();

			if (Session["keywords"] == null)
			{
				var user = GetUserFromNameCourse();
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

		public ActionResult GetVisited() //If been here, return true, else false.
		{
			if (cookie.LoadStringFromCookie(Cookies.UserNameCookieKey) != null)
			{
				return Json(true, JsonRequestBehavior.AllowGet);
			}
			return Json(false, JsonRequestBehavior.AllowGet);
		}

		public StudentUser GetUserFromNameCourse()
		{
			var name = "";
			var course = "";
			var year = "";

			var cookieYear = cookie.LoadStringFromCookie(Cookies.YearCookieKey);
			if (!string.IsNullOrEmpty(cookieYear))
			{
				year = cookieYear;
			}
			else
			{
				year = "second";
			}

			var cookieUserName = cookie.LoadStringFromCookie(Cookies.UserNameCookieKey);
			if (!string.IsNullOrEmpty(cookieUserName))
			{
				name = cookieUserName;
			}
			else
			{
				name = "NO NAME";
			}
			var cookieCourse = cookie.LoadStringFromCookie(Cookies.CourseCookieKey);
			if (!string.IsNullOrEmpty(cookieCourse))
			{
				course = cookieCourse;
			}
			else
			{
				course = "VisAlt";
			}


			var schoolCourse = Course.GetCourseFromName(course);
			return new StudentUser(name, schoolCourse, year);
			//If cookie name && course == default, name = anonym14, course = "VisAlt"
		}

		public PartialViewResult UserStatUpdate(int eventid, bool attend)
		{
			var db = new CalendarEventContext();
			var userName = cookie.LoadStringFromCookie(Cookies.UserNameCookieKey);

			if (userName != "" || userName != null)
			{
				var alreadyGoing = false;
				EventUserStat current = null;
				foreach (var eus in db.EventUserStats)
				{
					if (eus.UserName == userName && eus.EventID == eventid)
					{
						current = eus;
						alreadyGoing = true;
						// here: project extension material - calculate score based on eus.GoingTime, add "score" to account.cs
					}
				}
				if (attend)
				{
					if (alreadyGoing)
					{
						current.Attend = true;
					}
					else
					{
						db.EventUserStats.Add(new EventUserStat
						{
							UserName = userName,
							EventID = eventid,
							GoingTime = DateTime.Now,
							Attend = true
						});
					}
				}
				else
				{
					if (alreadyGoing)
					{
						db.EventUserStats.Remove(current);
					}
					else
					{
						db.EventUserStats.Add(new EventUserStat
                        {
                            UserName = userName,
                            EventID = eventid,
                            GoingTime = DateTime.Now
                        });
					}
				}
				db.SaveChanges();
			}

			var modelWrapper = new ModelDataWrapper();
			modelWrapper.eventID = eventid;
			return PartialView("_UserStatUpdate", modelWrapper);
		}

		public ActionResult GetMessage()
		{
			var message = "Welcome";
			return new JsonResult { Data = message, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
		}
	}
}