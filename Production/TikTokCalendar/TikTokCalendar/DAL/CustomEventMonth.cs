using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TikTokCalendar.Extras;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class CustomEventMonth
	{
		public int MonthNumber { get; set; }
		public int FirstWeekOfMonthNumber { get; private set; }
		public List<CustomEventWeek> Weeks { get; set; }
		private DateTime firstDate;
		public CustomEventMonth(int monthNumber)
		{
			MonthNumber = monthNumber;
			Weeks = new List<CustomEventWeek>();
			firstDate = new DateTime(DateTime.Now.Year, monthNumber, 1);
			FirstWeekOfMonthNumber = firstDate.GetWeekNumberOfYear();
		}

		public void AddEvent(CustomEvent evnt)
		{
			//int weekNr = evnt.StartDateTime.GetWeekNumberOfYear();
			//weekNr = weekNr / MonthNumber;
			int evntWeekNr = evnt.StartDateTime.GetWeekNumberOfYear();
			int weekNr = evntWeekNr - FirstWeekOfMonthNumber;
			Weeks[weekNr].events.Add(evnt);
		}

		public string GetMonthName()
		{
			return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(MonthNumber).FirstCharToUpper();
		}
	}
}