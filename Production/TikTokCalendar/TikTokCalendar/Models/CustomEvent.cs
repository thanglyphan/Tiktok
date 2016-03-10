﻿using System;
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
		//public string EventName { get { return Subject.Name + Subject.Code; } } // TODO Not needed
		public Subject Subject { get; private set; }
		public int ClassYear { get; private set; }
		public List<SchoolCourses> Courses { get; private set; }
		public string RoomName { get; private set; }
		public string Teacher { get; private set; }
		//public string EventType { get; private set; }
		public EventType eventType { get; private set; }
		public string Comment { get; private set; }

		public string EventTypeLabel { get { return eventType.ToString(); } }

		public string CoursesLabel
		{
			get
			{
				string text = "";
				for (int i = 0; i < Courses.Count; i ++)
				{
					text += Courses[i] + ((i < Courses.Count - 1) ? ", " : "");
				}
				return text;
			}
		}

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

			//int rndEvnt = new Random().Next(1, 5);
			//EventType = (EventType)rndEvnt;
			this.eventType = eventType;
			Comment = comment;
		}

		private DateTime ParseDate(string date)
		{
			return DateTime.MinValue;
		}

		public string GetDayOfWeek()
		{
			return CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(StartDateTime.DayOfWeek).Substring(0, 3).ToLower();
		}

		public string GetTimeSlot()
		{
			return StartTimeLabel;
		}

		public int EventIsToday()
		{
			return 0;
		}
	}

	public enum EventType
	{
		None,
		Forelesning,
		Eksamen,
		Innlevering,
		Prosjekt,
		Annet
	}
}