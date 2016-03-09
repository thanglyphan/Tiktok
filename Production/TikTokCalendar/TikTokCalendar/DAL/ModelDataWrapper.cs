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

		public ModelDataWrapper()
		{
			calEvents = new List<EventMonth>();
			Months = new List<CustomEventMonth>();
			user = new StudentUser("trotor14", SchoolCourses.Spillprogrammering);
            var db = new CalendarEventContext();
            eventUserStats = new List<EventUserStat>(db.EventUserStats);
        }
	}
}