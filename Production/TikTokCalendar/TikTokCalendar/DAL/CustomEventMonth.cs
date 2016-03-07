using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikTokCalendar.Extras;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class CustomEventMonth
	{
		public int MonthNumber { get; set; }
		public List<CustomEventWeek> Weeks { get; set; }

		public CustomEventMonth(int monthNumber)
		{
			MonthNumber = monthNumber;
			Weeks = new List<CustomEventWeek>();
		}

		public void AddEvent(CustomEvent evnt)
		{
			Weeks[evnt.StartDateTime.GetWeekNumberOfYear()].events.Add(evnt);
		}

		public string GetMonthName()
		{
			
			return "Måned";
		}
	}
}