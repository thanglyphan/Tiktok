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
			return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month);
		}

		public string GetMonthId()
		{
			return GetMonthName().Substring(0, 3);
		}
	}
}