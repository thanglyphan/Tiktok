using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class CustomEventWeek
	{
		public int WeekNumber { get; private set; }
		public string WeekName { get { return "Uke " + WeekNumber; } }
		public List<CustomEvent> events { get; set; }

		public CustomEventWeek(int weekNumber)
		{
			WeekNumber = weekNumber;
			events = new List<CustomEvent>();
		}
	}
}