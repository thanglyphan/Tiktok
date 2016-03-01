using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TikTokCalendar.Models
{
	public class CalendarEvent
	{
		public int ID { get; set; }
		public int SubjectID { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string RoomName { get; set; }
		public string EventName { get; set; }
		public string Attendees { get; set; }
		public string Teacher { get; set; }
		public string Comment { get; set; }

		public virtual Subject Subject { get; set; }

		/// <summary>
		/// Returns the start and to time formatted for display.
		/// </summary>
		public string GetTimeSlot()
		{
			return $"{StartTime:HH:mm} - {EndTime:HH:mm}";
		}

		/// <summary>
		/// Returns the three first letters of this events day.
		/// </summary>
		public string GetDayOfWeek()
		{
			//return StartTime.DayOfWeek.ToString().Substring(0, 3);
			return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(StartTime.DayOfWeek).Substring(0, 3).ToLower();
		}

		public int GetWeekNumber()
		{
			Calendar cal = new GregorianCalendar();
			DateTime dt = StartTime;
			return cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
		}

		public static bool operator ==(CalendarEvent a, CalendarEvent b)
		{
			// Assumes two events are equal if the start time, subjectID and room are the same
			if (a.StartTime == b.StartTime && a.SubjectID == b.SubjectID && a.RoomName == b.RoomName)
			{
				return true;
			}
			return false;
		}

		public static bool operator !=(CalendarEvent a, CalendarEvent b)
		{
			return !(a == b);
		}
	}
}