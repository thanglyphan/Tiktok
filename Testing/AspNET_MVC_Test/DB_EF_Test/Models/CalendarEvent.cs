using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DB_EF_Test.Models
{
	public class CalendarEvent
	{
		public int ID { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		public int SubjectID { get; set; }
		public int RoomID { get; set; }
		public string Attendees { get; set; }
		public EventType EventType { get; set; }
		public string Comment { get; set; }

		public virtual Subject Subject { get; set; }
		public virtual Room Room { get; set; }
	}

	public enum EventType
	{
		Forelesning,
		Innlevering,
		Eksamen,
		Annet
	}
}