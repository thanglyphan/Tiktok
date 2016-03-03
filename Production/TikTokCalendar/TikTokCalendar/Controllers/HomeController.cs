﻿using System;
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
		private CalendarEventContext db = new CalendarEventContext();

		public ActionResult Index(string inputValue)
		{
			//Debug.WriteLine(inputValue); // HER PRINTES DET SOM ER SKREVET INN
			string id = inputValue; // HER PRINTES DET SOM ER SKREVET INN
			//string id = "None";

			Printer.Print("User: " + id);
			string name = "trotor14";
			SchoolCourses course = SchoolCourses.SpillProgrammering;
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
				course = SchoolCourses.MobilApputvikling;
				Printer.Print("User: " + id + " idesign");
			}
			//Cookies cookie;
			//string user = cookie.LoadFromCookie("Username");
			//int program = cookie.LoadFromCookie("Program");
			StudentUser user = new StudentUser(name, course); // TODO Get this from cookies
			int weekOrMonthView = 1; // TODO Get this from cookies

			if (id.StartsWith("0"))
			{
				weekOrMonthView = 0;
			}
			else if (id.StartsWith("1"))
			{
				weekOrMonthView = 1;
			}

			bool weekView = (weekOrMonthView == 0);

			ViewBag.Title = string.Format("Year: {0}, sem: {1}, valid: {2}", user.ClassYear, user.GetCurrentSemester(), user.ValidUsername(user.UserName));

			var events = db.CalendarEvents.ToList();
			// TODO Refactor instances of Month into something else
			// TODO Make the year go from august to june like a schoolyear
			int eventGroupCount = (weekView) ? 52 : 12;
			var modelWrapper = new ModelDataWrapper();
			modelWrapper.calEvents = new List<EventMonth>(new EventMonth[eventGroupCount]);
			int startMonth = 8; // Starting month number
			int monthNum = startMonth; 
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
			
			// Go through all course subjects
			foreach (var item in db.CourseSubject)
			{
				// Check if the coursesubject has the same ID and semester as the user
				//if (item.CourseID == acc.CourseID && item.Semester == acc.SemesterID)
				if (item.CourseID == (int)user.Course && item.Semester == user.GetCurrentSemester())
				{

					// Go through all events
					foreach (var calEvent in events)
					{
						if (calEvent.SubjectID == item.SubjectID && SameYear(calEvent, user))
						{
							int monthIndex = -1;
							if (weekView)
							{
								monthIndex = calEvent.GetWeekNumber() - 1;
							}
							else
							{
								//monthIndex = calEvent.StartTime.Month - 1;
								for (int i = 0; i < modelWrapper.calEvents.Count; i++)
								{
									if (modelWrapper.calEvents[i].Month == calEvent.StartTime.Month)
									{
										monthIndex = i;
									}
								}
							}

							// Only add event if we haven't already added this TimeEditID already
							if (!addedEvents.Contains(calEvent.TimeEditID))
							{
								modelWrapper.calEvents[monthIndex].Events.Add(calEvent);
								addedEvents.Add(calEvent.TimeEditID);
							}
						}
					}
				}
			}


			// Sort that shit
			for (int i = 0; i < modelWrapper.calEvents.Count; i++)
			{
				modelWrapper.calEvents[i].Events = modelWrapper.calEvents[i].Events.OrderBy(x => x.StartTime).ToList();
			}
			return View(modelWrapper);//.calEvents);
		}

		private bool SameYear(CalendarEvent calEvent, StudentUser user)
		{
			if (calEvent.Year < 0) return true;
			return user.ClassYear == calEvent.Year;
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
	}
}