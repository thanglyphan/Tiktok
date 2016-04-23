using System;
using System.Globalization;

namespace TikTokCalendar.Models
{
	public class CalendarEvent
	{
		/// <summary>
		///     Creates an empty event.
		/// </summary>
		public CalendarEvent()
		{
			StartTime = DateTime.MinValue;
			EndTime = DateTime.MinValue;
			TimeEditID = -1;
			SubjectID = -1;
			RoomName = null;
			EventName = null;
			Attendees = null;
			Teacher = null;
			Comment = null;
			Year = 0;
		}

		public int ID { get; set; }
		public int TimeEditID { get; set; }
		public int SubjectID { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public string RoomName { get; set; }
		public string EventName { get; set; }
		public string Attendees { get; set; }
		public string Teacher { get; set; }
		public string Comment { get; set; }

		public virtual Subject Subject { get; set; }


		public int Year { get; set; }

		public string[] GetEventAsArray
		{
			get
			{
				return new[]
				{
					ID.ToString(), StartTime.ToString(), EndTime.ToString(), RoomName, EventName, Attendees, Teacher, Comment,
					Subject.Name
				};
			}
		}

		public string GetEventNameWithoutSubjectCode()
		{
			//return Subject.Name;
			if (Subject != null && TimeEditID > 0)
			{
				//Printer.Print("SubjName: " + Subject.Name);
				return Subject.Name.Substring(0, Subject.Name.Length - 11);
			}
			return null;
		}

		/// <summary>
		///     Returns the start and to time formatted for display.
		/// </summary>
		public string GetTimeSlot()
		{
			return $"{StartTime:HH:mm} - {EndTime:HH:mm}";
		}

		/// <summary>
		///     Returns the three first letters of this events day.
		/// </summary>
		public string GetDayOfWeek()
		{
			//return StartTime.DayOfWeek.ToString().Substring(0, 3);
			return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(StartTime.DayOfWeek).Substring(0, 3).ToLower();
		}

		public int GetWeekNumber()
		{
			Calendar cal = new GregorianCalendar();
			var dt = StartTime;
			return cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
		}

		public bool Equals(CalendarEvent e)
		{
			if (e == null) return false;
			return TimeEditID == e.TimeEditID;
		}

		public string ToText()
		{
			return TimeEditID.ToString();
		}
	}
}