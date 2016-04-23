using System.Collections.Generic;
using System.Globalization;
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
			//firstDate = new DateTime(DateTime.Now.Year, monthNumber, 1);
			//FirstWeekOfMonthNumber = firstDate.GetWeekNumberOfYear();
		}

		public int MonthNumber { get; set; }
		public int FirstWeekOfMonthNumber { get; private set; }
		public List<CustomEventWeek> Weeks { get; set; }

		public void AddEvent(CustomEvent evnt)
		{
			//int weekNr = evnt.StartDateTime.GetWeekNumberOfYear();
			//weekNr = weekNr / MonthNumber;
			var evntWeekNr = evnt.StartDateTime.GetWeekNumberOfYear();
			var weekNr = evntWeekNr - FirstWeekOfMonthNumber;
			Weeks[weekNr].events.Add(evnt);
		}

		public string GetMonthName()
		{
			return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(MonthNumber).FirstCharToUpper();
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