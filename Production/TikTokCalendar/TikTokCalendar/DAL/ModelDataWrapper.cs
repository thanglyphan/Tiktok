using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikTokCalendar.DAL
{
	public class ModelDataWrapper
	{
		public List<EventMonth> calEvents;
		public StudentUser user;

		public ModelDataWrapper()
		{
			calEvents = new List<EventMonth>();
			user = new StudentUser("trotor14", SchoolCourses.SpillProgrammering);
		}
	}
}