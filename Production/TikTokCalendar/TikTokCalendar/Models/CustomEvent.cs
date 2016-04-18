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
		public long ID { get; private set; }
		public DateTime StartDateTime { get; private set; }
		public DateTime EndDateTime { get; private set; }
		public bool HasStartTime { get; set; }
		public bool HasEndDateTime { get; private set; }
		//public string EventName { get { return Subject.Name + Subject.Code; } } // TODO Not needed
		public Subject Subject { get; private set; }
		public int ClassYear { get; private set; }
		public int Weighting { get; private set; }
		public HashSet<int> ClassYears { get; private set; }

		// DEBUG
		public string YearLabelTest {
			get
			{
				string s = "";
				int i = 0;
				foreach (var y in ClassYears)
				{
					s += y;
					if (i < ClassYears.Count - 1)
					{
						s += ", ";
					}
					i++;
				}
				return s;
			}
		}
		public List<SchoolCourses> Courses { get; private set; }
		public string RoomName { get; private set; }
		public string Teacher { get; private set; }
		public EventType eventType { get; private set; }
		public MainEventType MainEventType { get; private set; }
		public string Comment { get; private set; }

		public bool IsFinal { get { return Weighting >= 100; } }
		public bool HasWeighting { get { return Weighting > 0; } }

		//////// Getters for the non-string field to be used in the view for displaying the data ////////
		/// <summary>
		/// Returns the eventtype, unless the event has no weighting, in which case it returns and empty string.
		/// </summary>
		public string EventTypeLabel
		{
			get
			{
				string text = "";
				if (eventType == EventType.Fremforing)
				{
					text = "Fremføring";
				}
				else if (eventType == EventType.Oving)
				{
					text = "Øving";
				}
				else if (eventType == EventType.SkriftligEksamen)
				{
					text = "Skriftlig eksamen";
				}
				else if (HasWeighting)
				{
					text = eventType.ToString();
				}
				return text;
			}
		}

		public string CoursesLabel {
			get {
				string text = "";
				for (int i = 0; i < Courses.Count; i ++)
				{
					text += Courses[i] + ((i < Courses.Count - 1) ? ", " : "");
				}
				return text;
			}
		}

		public string StartTimeLabel {
			get {
				string text = "";
				if (HasStartTime)
				{
					text = string.Format(", {0:HH:mm}", StartDateTime);
					if (HasEndDateTime)
					{
						text += string.Format(" - {0:HH:mm}", EndDateTime);
					}
				}
				return text;
			}
		}

		public CustomEvent(long id, DateTime startDateTime, bool hasStartTime, DateTime endDateTime, bool hasEndDateTime, Subject subject, 
			HashSet<int> classYears, List<SchoolCourses> courses, string room, string teacher, EventType eventType, string comment, int weighting)
		{
			ID = id;
			StartDateTime = startDateTime;
			HasStartTime = hasStartTime;
			HasEndDateTime = hasEndDateTime;
			EndDateTime = endDateTime;
			Subject = subject;
			ClassYears = classYears;
			Courses = courses;
			RoomName = room;
			Teacher = teacher;
			Comment = comment;
			Weighting = weighting;
			this.eventType = eventType;

			// If the event has a weighting of 100%, the event is Eksamen
			if (IsFinal)
			{
				MainEventType = MainEventType.Eksamen;
			}
			else
			{
				switch (eventType)
				{
					case EventType.None:
						MainEventType = MainEventType.Forelesning;
						break;
					case EventType.Forelesning:
						MainEventType = MainEventType.Forelesning;
						break;
					case EventType.Eksamen:
						MainEventType = MainEventType.Eksamen;
						break;
					case EventType.Innlevering:
						MainEventType = MainEventType.Innlevering;
						break;
					case EventType.Prosjekt:
						MainEventType = MainEventType.Forelesning;
						break;
					case EventType.Hjemmeeksamen:
						MainEventType = MainEventType.Eksamen;
						break;
					case EventType.SkriftligEksamen:
						MainEventType = MainEventType.Eksamen;
						break;
					case EventType.Muntlig:
						MainEventType = MainEventType.Eksamen;
						break;
					case EventType.Mappe:
						MainEventType = MainEventType.Innlevering;
						break;
					case EventType.Fremforing:
						MainEventType = MainEventType.Innlevering;
						break;
					case EventType.Oving:
						MainEventType = MainEventType.Forelesning;
						break;
					case EventType.Annet:
						MainEventType = MainEventType.Forelesning;
						break;
					default:
						MainEventType = MainEventType.Forelesning;
						break;
				}
			}
		}

		public bool IsYear(int year)
		{
			return ClassYears.Contains(year);
		}

		public string GetDayOfWeek()
		{
			string dayName = CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(StartDateTime.DayOfWeek);
			return dayName;//.Substring(0, 3).ToLower();
		}

		public string GetTimeSlot()
		{
			return StartTimeLabel;
		}
	}

	public enum MainEventType
	{
		Forelesning,
		Innlevering,
		Eksamen
	}

	public enum EventType
	{
		None,
		Forelesning,
		Eksamen,
		Innlevering,
		Prosjekt,
		Hjemmeeksamen,
		SkriftligEksamen,
		Muntlig,
		Mappe,
		Fremforing,
		Oving,
		Annet
	}
}