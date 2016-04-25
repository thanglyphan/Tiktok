using System.Collections.Generic;
using TikTokCalendar.Controllers;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class ModelDataWrapper
	{
		public StudentUser User { get; set; }
		public List<CustomEventMonth> Months { get; set; }
		public List<Room> Rooms { get; set; }
		public List<EventUserStat> eventUserStats { get; set; }
		public string searchWords { get; private set; }
		public bool showingLectures { get; private set; }
		public bool showingAssignments { get; private set; }
		public bool showingExams { get; private set; }
		public bool isFiltered { get; set; }
		public bool FailedLogin { get; set; } = false;
		public int eventID { get; set; }
		public string CultureText { get; set; }

		public ModelDataWrapper()
		{
			Init();
			searchWords = "";
			showingLectures = true;
			showingAssignments = true;
			showingExams = true;
		}

		public ModelDataWrapper(bool lecture, bool assignment, bool exam)
		{
			Init();
			showingLectures = lecture;
			showingAssignments = assignment;
			showingExams = exam;
			searchWords = "";
		}

		public ModelDataWrapper(string tags, bool lecture, bool assignment, bool exam)
		{
			Init();
			searchWords = tags;
			showingLectures = lecture;
			showingAssignments = assignment;
			showingExams = exam;
		}

		

		private void Init()
		{
			Months = new List<CustomEventMonth>();
			User = new StudentUser("NO NAME", SchoolCourses.VisAlt, "second");
			Rooms = new List<Room>();
			var db = new CalendarEventContext();
			eventUserStats = new List<EventUserStat>(db.EventUserStats);
		}

		public int GetEventCount()
		{
			var events = 0;
			foreach (var month in Months)
			{
				foreach (var week in month.Weeks)
				{
					foreach (var item in week.events)
					{
						events++;
					}
				}
			}
			return events;
		}
	}
}