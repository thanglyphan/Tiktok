using System.Collections.Generic;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class ModelDataWrapper
	{
		public List<EventMonth> calEvents;
		public StudentUser User;

		public ModelDataWrapper()
		{
			init();
			searchWords = "";
			showingLectures = true;
			showingAssignments = true;
			showingExams = true;
		}

		public ModelDataWrapper(bool lecture, bool assignment, bool exam)
		{
			init();
			showingLectures = lecture;
			showingAssignments = assignment;
			showingExams = exam;
			searchWords = "";
		}

		public ModelDataWrapper(string tags, bool lecture, bool assignment, bool exam)
		{
			init();
			searchWords = tags;
			showingLectures = lecture;
			showingAssignments = assignment;
			showingExams = exam;
		}

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

		private void init()
		{
			calEvents = new List<EventMonth>();
			Months = new List<CustomEventMonth>();
			User = new StudentUser("NO NAME", SchoolCourses.VisAlt, "second");
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