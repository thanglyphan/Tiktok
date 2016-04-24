using System.Collections.Generic;
using System.Globalization;
using TikTokCalendar.Controllers;
using TikTokCalendar.Extras;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class CustomEventMonth
	{
		public CustomEventMonth(int monthNumber)
		{
			MonthNumber = monthNumber;
			Weeks = new List<CustomEventWeek>();
		}

		public int MonthNumber { get; set; }
		public int FirstWeekOfMonthNumber { get; private set; }
		public List<CustomEventWeek> Weeks { get; set; }

		public void AddEvent(CustomEvent evnt)
		{
			var evntWeekNr = evnt.StartDateTime.GetWeekNumberOfYear();
			var weekNr = evntWeekNr - FirstWeekOfMonthNumber;
			Weeks[weekNr].events.Add(evnt);
		}

		public string GetMonthName()
		{
			string monthName = CultureManager.CurrentCulture.DateTimeFormat.GetMonthName(MonthNumber);
			return monthName.FirstCharToUpper();
		}

		public int GetEventTypeCount(EventType evntType)
		{
			var count = 0;
			foreach (var week in Weeks)
			{
				count += week.GetEventTypeCount(evntType);
			}
			return count;
		}
	}
}