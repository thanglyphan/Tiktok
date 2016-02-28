using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class EventMonth
	{
		public int Month { get; set; }
		public List<CalendarEvent> Events { get; set; }

		public EventMonth(int month)
		{
			Month = month;
			Events = new List<CalendarEvent>();
		}

		public string GetMonthName()
		{
			return FirstCharToUpper(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month)); 
		}

		public string GetMonthId()
		{
			return GetMonthName().Substring(0, 3);
		}

		public static string FirstCharToUpper(string input)
		{
			if (String.IsNullOrEmpty(input))
				throw new ArgumentException("Could not capitalize first letter of that word");
			return input.First().ToString().ToUpper() + input.Substring(1);
		}
	}
}