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

namespace TikTokCalendar.Controllers
{
	public class HomeController : Controller
	{
		private CalendarEventContext db = new CalendarEventContext();

		public ActionResult Index()
		{
			StudentUser user = new StudentUser("trotor14", SchoolCourses.SpillProgrammering);

			Account acc = db.Accounts.Find(3);
			//new Random().Next(2, 4)); // TODO Replace with the currently logged in account
			ViewBag.Title = "User: " + acc.ID;

			var events = db.CalendarEvents.ToList();

			// TODO Make the year go from august to june like a schoolyear
			var calEvents = new List<EventMonth>(new EventMonth[12]);
			int startMonth = 8; // Starting month number
			int monthNum = startMonth; 
			//for (var i = monthNum; i < 12 + monthNum; i++)
			for (int i = 0; i < 12; i++)
			{
				calEvents[i] = new EventMonth(i+1);
				//monthNum++;
				//if (monthNum > 12) monthNum = 1;
			}

			// Go through all course subjects
			foreach (var item in db.CourseSubject)
			{
				// Check if the coursesubject has the same ID and semester as the user
				// TODO Replace acc.semesterID with user.GetSemester()
				// TODO and replace acc.courseID with user.Course
				if (item.CourseID == acc.CourseID && item.Semester == acc.SemesterID)
				{
					// Go through all events
					foreach (var calEvent in events)
					{
						// Add the event if the events subjectID mathces the courseubjects subjectID
						if (calEvent.SubjectID == item.SubjectID)
						{
							//calendarEvents.Add(calEvent);
							//calEvents[calEvent.StartTime.Month].Add(calEvent);
							int monthIndex = calEvent.StartTime.Month - 1;
							for (int i = 0; i < calEvents.Count; i++)
							{
								if (calEvents[i].Month == calEvent.StartTime.Month)
								{
									monthIndex = i;
								}
							}
							/*for (int i = 0; i < list.Count; i++)
							{
								if (list[i].StartTime.Month == month)
								{
									return i;
								}
							}*/
							calEvents[monthIndex].Events.Add(calEvent);
							//list.Add(calEvent.Subject.Name);
						}
					}
				}
			}

			// Sort that shit
			for (int i = 0; i < calEvents.Count; i++)
			{
				calEvents[i].Events = calEvents[i].Events.OrderBy(x => x.StartTime).ToList();
			}

			return View(calEvents);
			//return View(calendarEvents.ToList());


			/*
			 * Make a class for the calendar view?
			 *		With a List of months that each have a list with events for that month (List<List<CalendarEvents>> events)
			 * foreach (month)
			 *		display month header
			 *			foreach(events in month)
			 *				display event
			 */
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