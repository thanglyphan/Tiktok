using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TikTokCalendar.Controllers;
using TikTokCalendar.Models;

// A container for all events in a month. Used to send the events to the GUI layer (Home/Index.cshtml)

namespace TikTokCalendar.DAL
{
	//public class EventMonth
	//{
	//	private readonly bool _weekView = true;
	//	public StudentUser user;

	//	public EventMonth(int month, bool weekView)
	//	{
	//		Month = month;
	//		_weekView = weekView;
	//		Events = new List<CalendarEvent>();
	//	}

	//	public int Month { get; set; }
	//	public List<CalendarEvent> Events { get; set; }

	//	public string GetMonthName()
	//	{
	//		if (_weekView)
	//		{
	//			return "Uke " + GetWeekNumber();
	//		}
	//		return FirstCharToUpper(CultureManager.CurrentCulture.DateTimeFormat.GetMonthName(Month));
	//	}

	//	public int GetWeekNumber()
	//	{
	//		Calendar cal = new GregorianCalendar();
	//		var dt = DateTime.Now;
	//		dt = Events[0].StartTime;
	//		return cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
	//	}

	//	public string GetMonthId()
	//	{
	//		return GetMonthName().Substring(0, 3);
	//	}

	//	public static string FirstCharToUpper(string input)
	//	{
	//		if (string.IsNullOrEmpty(input))
	//		{ 
	//			return input;
	//		}
	//		return input.First().ToString().ToUpper() + input.Substring(1);
	//	}
	//}
}