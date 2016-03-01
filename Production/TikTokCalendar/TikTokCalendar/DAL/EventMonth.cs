using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TikTokCalendar.Models;

// A container for all events in a month. Used to send the events to the GUI layer (Home/Index.cshtml)
namespace TikTokCalendar.DAL
{
	public class EventMonth
	{
		public int Month { get; set; }
		public List<CalendarEvent> Events { get; set; }
		private bool _weekView = true;

		public EventMonth(int month, bool weekView)
		{
			Month = month;
			_weekView = weekView;
			Events = new List<CalendarEvent>();
		}

		public string GetMonthName()
		{
			if (_weekView)
			{
				return "Uke " + GetWeekNumber();
			}
			return FirstCharToUpper(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Month)); 

			//Calendar cal = new GregorianCalendar();
			//DateTime dt = DateTime.Now;
			//dt = Events[0].StartTime;
			//int weekNum = cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
			//return FirstCharToUpper(CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek.w(Month));
		}

		public int GetWeekNumber()
		{
			Calendar cal = new GregorianCalendar();
			DateTime dt = DateTime.Now;
			dt = Events[0].StartTime;
			return cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
		}

		public string GetMonthId()
		{
			return GetMonthName().Substring(0, 3);
		}

		public static string FirstCharToUpper(string input)
		{
			if (string.IsNullOrEmpty(input))
				throw new ArgumentException("Could not capitalize first letter of that word");
			return input.First().ToString().ToUpper() + input.Substring(1);
		}
	}
}