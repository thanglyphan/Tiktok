using System.Collections.Generic;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class CustomEventWeek
	{
		public CustomEventWeek(int weekNumber, int localWeekNumber)
		{
			WeekNumber = weekNumber;
			LocalWeekNumber = localWeekNumber;
			events = new List<CustomEvent>();
		}

		/// <summary>
		///     The weeknumber in relation to the year (1-52)
		/// </summary>
		public int WeekNumber { get; }

		/// <summary>
		///     The weeknumber in this month (usually 1-4), used for iterating
		/// </summary>
		public int LocalWeekNumber { get; private set; }

		public string WeekName
		{
			get { return "Uke " + WeekNumber; }
		}

		public List<CustomEvent> events { get; set; }

		public int GetEventTypeCount(EventType evntType)
		{
			var count = 0;
			foreach (var e in events)
			{
				// TODO Use MainEventType instead
				if (e.eventType == evntType)
				{
					count++;
				}
				else if ((e.eventType == EventType.Hjemmeeksamen || e.eventType == EventType.SkriftligEksamen ||
				          e.eventType == EventType.Muntlig)
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