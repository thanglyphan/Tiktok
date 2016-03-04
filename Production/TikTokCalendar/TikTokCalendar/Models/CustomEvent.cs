using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TikTokCalendar.DAL;
using TikTokCalendar.Extras;

namespace TikTokCalendar.Models
{
	public class CustomEvent
	{
		public int ID { get; private set; }
		public DateTime StartDateTime { get; private set; }
		public DateTime EndDateTime { get; private set; }
		public bool HasEndDateTime { get; private set; }
		public string EventName { get; private set; } // TODO Not needed
		public Subject Subject { get; private set; }
		public int ClassYear { get; private set; }
		public List<SchoolCourses> Courses { get; private set; }
		public string RoomName { get; private set; }
		public string Teacher { get; private set; }
		//public string EventType { get; private set; }
		public EventType EventType { get; private set; }
		public string Comment { get; private set; }

		//////// Getters for the non-string field to be used in the view for displaying the data ////////
		public string StartTimeLabel {
			get
			{
				string text = "";
				text = StartDateTime.ToString("HH:mm");
				if (HasEndDateTime)
				{
					text += string.Format(" - {0:HH:mm}", EndDateTime);
				}
				return text;
			}
		}

		public CustomEvent(int id, DateTime startDateTime, DateTime endDateTime, bool hasEndDateTime, Subject subject, 
			int classYear, List<SchoolCourses> courses, string room, string teacher, EventType eventType, string comment)
		{
			ID = id;
			StartDateTime = startDateTime;
			EndDateTime = endDateTime;
			Subject = subject;
			ClassYear = classYear;
			Courses = courses;
			RoomName = room;
			Teacher = teacher;
			EventType = eventType;
			Comment = comment;
		}

		private DateTime ParseDate(string date)
		{
			return DateTime.MinValue;
		}
	}

	public enum EventType
	{
		None,
		Forelesning,
		Eksamen,
		Innlevering,
		Annet
	}
}