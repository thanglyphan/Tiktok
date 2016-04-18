using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class CustomEventWeek
	{
		/// <summary>
		/// The weeknumber in relation to the year (1-52)
		/// </summary>
		public int WeekNumber { get; private set; }

		/// <summary>
		/// The weeknumber in this month (usually 1-4), used for iterating
		/// </summary>
		public int LocalWeekNumber { get; private set; }
		public string WeekName { get { return "Uke " + WeekNumber; } }
		public List<CustomEvent> events { get; set; }

		public CustomEventWeek(int weekNumber, int localWeekNumber)
		{
			WeekNumber = weekNumber;
			LocalWeekNumber = localWeekNumber;
			events = new List<CustomEvent>();
		}

		public int GetEventTypeCount(EventType evntType)
		{
			int count = 0;
			foreach (var e in events)
			{
				if (e.eventType == evntType)
				{
					count++;
				}
				else if ((e.eventType == EventType.Hjemmeeksamen || e.eventType == EventType.SkriftligEksamen || e.eventType == EventType.Muntlig) 
					&& evntType == EventType.Eksamen)
				{
					count++;
				}
				else if ((e.eventType == EventType.Mappe || e.eventType == EventType.Fremforing) && e.IsFinal)
				{
					count++;
				}
			}
			return count;
		}
	}
}