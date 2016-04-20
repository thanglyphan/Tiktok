using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class ModelDataWrapper
	{
		public List<EventMonth> calEvents;
		public List<CustomEventMonth> Months { get; set; }
		public StudentUser user;
        public List<EventUserStat> eventUserStats { get; set; }
        public string searchWords { get; private set; }
        public bool showingLectures { get; private set; }
        public bool showingAssignments { get; private set; }
        public bool showingExams { get; private set; }
        public bool isFiltered { get; set; }

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
        private void init()
        {
            calEvents = new List<EventMonth>();
            Months = new List<CustomEventMonth>();
            user = new StudentUser("trotor14", SchoolCourses.Spillprogrammering, "second");
            var db = new CalendarEventContext();
            eventUserStats = new List<EventUserStat>(db.EventUserStats);
        }

        public int GetEventCount()
        {
            int events = 0;
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