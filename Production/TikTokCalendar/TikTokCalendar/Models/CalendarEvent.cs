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

		public string GetTimeSlot()
		{
			return $"{StartTime:HH:mm} - {EndTime:HH:mm}";
		}

		public string GetDayOfWeek()
		{
			//return StartTime.DayOfWeek.ToString().Substring(0, 3);
			return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(StartTime.DayOfWeek).Substring(0, 3).ToUpper();
		}
	}
}