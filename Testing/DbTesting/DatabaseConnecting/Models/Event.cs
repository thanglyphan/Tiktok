using System;
using System.Data.Entity;

namespace DatabaseConnecting.Models
{
	public class Event
	{
		public int ID { get; set; }
		public DateTime Date { get; set; }
		public string Name { get; set; }
		public CourseID Course { get; set; }
		public EventRoom Room { get; set; }
		public EventType Type { get; set; }

		public class EventDBContext : DbContext
		{
			public DbSet<Event> Events { get; set; }
		}
	}

	public enum EventType
	{
		Lecture,
		Task,
		Exam
	}

	public enum EventRoom
	{
		Auditoriet,
		_41,
		_42
	}

	public enum CourseID
	{
		PJ3100,
		PG3400,
		PG3300,
		PG4200
	}
}