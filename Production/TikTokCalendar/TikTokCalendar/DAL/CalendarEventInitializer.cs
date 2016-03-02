using System;
using System.Collections.Generic;
using TikTokCalendar.Models;
using System.Data.Entity;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;

// Class for initializing the database. 
//The Seed() function is called either always or on a change (depending on what class it inherits from)
namespace TikTokCalendar.DAL
{
	//public class CalendarEventInitializer : DropCreateDatabaseIfModelChanges<CalendarEventContext>
	/*NOTE: 
	 * Modifying the insertion doesn't count as model change.
	 * Modifying the DB name in the connection string DOES count as model change.
	 */
	public class CalendarEventInitializer : DropCreateDatabaseAlways<CalendarEventContext>
	{
		private const string Format = "dd.MM.yyyy HH:mm:ss";

		protected override void Seed(CalendarEventContext context)
		{
			InsertDummyData(context);
		}

		/// <summary>
		/// Inserts dummy data to the database.
		/// </summary>
		private void InsertDummyData(CalendarEventContext context)
		{
			// Courses
			var courses = new List<Course>
			{
				new Course { Name="Programmering" },
				new Course { Name="Spillprogrammering" },
				new Course { Name="Intelligente Systemer" },
				new Course { Name="Mobil Apputvikling" }
			};
			courses.ForEach(c => context.Courses.Add(c));
			context.SaveChanges();

			// Subjects
			// TODO Can populate this from going through the json file? or a file from the school with all subjects
			var subjects = new List<Subject>
			{
				new Subject { Name="Prosjekt software engineering" },
				new Subject { Name="Matematikk og Fysikk" },
				new Subject { Name="C++ Programmering" },
				new Subject { Name="Game AI" },
				new Subject { Name="Embedded systems" },
				new Subject { Name="Mobil utvikling" },
				new Subject { Name="Avansert Javaprogrammering" },
				new Subject { Name="Ruby on Rails" }
			};
			subjects.ForEach(s => context.Subjects.Add(s));
			context.SaveChanges();

			// CourseSubjects
			// TODO Semester might not be necessary, as TimeEdit only show stuff for this semester
			var courseSubjects = new List<CourseSubject>
			{
				new CourseSubject { CourseID=2, SubjectID=1, Semester=4 },
				new CourseSubject { CourseID=2, SubjectID=2, Semester=4 },
				new CourseSubject { CourseID=2, SubjectID=3, Semester=4 },
				new CourseSubject { CourseID=2, SubjectID=4, Semester=4 },
				
				new CourseSubject { CourseID=3, SubjectID=1, Semester=4 },
				new CourseSubject { CourseID=3, SubjectID=3, Semester=4 },
				new CourseSubject { CourseID=3, SubjectID=5, Semester=4 },
				new CourseSubject { CourseID=3, SubjectID=6, Semester=4 },

				new CourseSubject { CourseID=1, SubjectID=1, Semester=4 },
				new CourseSubject { CourseID=1, SubjectID=6, Semester=4 },
				new CourseSubject { CourseID=1, SubjectID=7, Semester=4 },
				new CourseSubject { CourseID=1, SubjectID=8, Semester=4 }

			};
			courseSubjects.ForEach(c => context.CourseSubject.Add(c));
			context.SaveChanges();

			// Calendar events
			/*var events = new List<CalendarEvent>
			{
				new CalendarEvent { SubjectID=1, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				new CalendarEvent { SubjectID=2, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				new CalendarEvent { SubjectID=3, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				new CalendarEvent { SubjectID=5, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				new CalendarEvent { SubjectID=3, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				new CalendarEvent { SubjectID=1, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				new CalendarEvent { SubjectID=5, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" }
			};*/

			// Parse the .json and insert into the dummy DB
			var dataPath = "~/Content/dummy-data.json";
			dataPath = HttpContext.Current.Server.MapPath(dataPath);
			var json = File.ReadAllText(dataPath);

			// TODO Check if the event exists? 


			var rootObj = JsonConvert.DeserializeObject<RootObject>(json);
			foreach (var item in rootObj.reservations)
			{
				// Time
				var start = GetParsedDateTime(item.startdate, item.starttime);
				var end = GetParsedDateTime(item.enddate, item.endtime);

				// Go through subjects and compare the first 10 letters to the subject name from the parsed file
				// TODO Find a safer way to do that (this all depends on us harcoding the correctly spelled subject names...)
				var subject = 1; // TODO Default to an empty event (to make it easier to see error)? If it can't find a similar one it will just take the first one
				foreach (var subj in context.Subjects)
				{
					if (item.columns[0].Substring(0, 7).Equals(subj.Name.Substring(0, 7)))
					{
						subject = subj.ID;
						break;
					}
				}
				int timeEditId = -1;
				int.TryParse(item.id, out timeEditId);


				// Make an event out of the data
				var ce = new CalendarEvent
				{
					StartTime = start,
					EndTime = end,
					TimeEditID = timeEditId,
					//SubjectName = item.columns[0], 
					SubjectID = subject,
					RoomName = item.columns[2],
					EventName = item.columns[4],
					Attendees = item.columns[1],
					Teacher = item.columns[3],
					Comment = item.columns[5]

				};
				// Add the event to the DB events list
				context.CalendarEvents.Add(ce);
			}
			context.SaveChanges();
			/*var events = new List<CalendarEvent>
			 {
				 new CalendarEvent { SubjectID=1, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Rom 41", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				 new CalendarEvent { SubjectID=2, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				 new CalendarEvent { SubjectID=3, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Rom 38", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				 new CalendarEvent { SubjectID=5, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				 new CalendarEvent { SubjectID=3, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				 new CalendarEvent { SubjectID=1, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" },
				 new CalendarEvent { SubjectID=5, StartTime=DateTime.Now, EndTime=DateTime.Now, RoomName="Auditoriet", EventName="Forelesning", Attendees="Spillprog", Teacher="Per", Comment="Comment" }
			 };*/
			//events.ForEach(c => context.CalendarEvents.Add(c));


			// Accounts
			var accounts = new List<Account>
			{
				new Account { UserName="test", Password=Encoder.SHA1.Encode("test"), CourseID=2, SemesterID=3, Email="test@test.com", Role = "Admin", RememberMe=false },
				new Account { UserName="prog", Password=Encoder.SHA1.Encode("test"), CourseID=1, SemesterID=3, Email="test@test.com", Role = "User", RememberMe=false },
				new Account { UserName="spill", Password=Encoder.SHA1.Encode("test"), CourseID=2, SemesterID=3, Email="test@test.com", Role = "User", RememberMe=false },
				new Account { UserName="sys", Password=Encoder.SHA1.Encode("test"), CourseID=3, SemesterID=3, Email="test@test.com", Role = "User", RememberMe=false },
				new Account { UserName="broken", Password=Encoder.SHA1.Encode("test"), CourseID=1, SemesterID=0, Email="test@test.com", Role = "User", RememberMe=false }
			};
			accounts.ForEach(a => context.Accounts.Add(a));
			context.SaveChanges();

			// TODO Add to database
			/*
			put an index for course and semester
			*/
			/* var subjects = new List<Subject>
			 {
				 new Subject {Name = "PG3300 - Programvarearkitektur"},
				 new Subject {Name = "PG3400 - Programmering i C for Linux"},
				 new Subject {Name = "PG4200 - Algoritmer og Datastrukturer"},
				 new Subject {Name = "PJ3100 - Prosjekt software engineering"}
			 };
			 subjects.ForEach(s => context.Subjects.Add(s));
			 context.SaveChanges();*/

			/*var courses = new List<Course>
			{
				new Course { Name="Name", Semesters = new List<List<Subject>>()}
			};
			// something wrong with the list list probably. Might have to do this the SQL way
			courses.ForEach(c => context.Courses.Add(c));
			context.SaveChanges();*/

			/*var accounts = new List<Account>
			{
				new Account {UserName = "test", Password = TikTokCalendar.Encoder.SHA1.Encode("test"), CourseID = 1, SemesterID = 2, Email = "lol@lol.lol", RememberMe = false }
			};
			accounts.ForEach(a => context.Accounts.Add(a));
			context.SaveChanges();*/

			/*var calendarEvents = new List<CalendarEvent>
			{
				new CalendarEvent {StartTime=DateTime.Parse("2015/11/27 10:15:00"),EndTime=DateTime.Parse("2015/11/27 14:00:00"),SubjectID=3,RoomID=1,Attendees="SpillProg, Prog",EventType=EventType.Forelesning,Comment=""},
				new CalendarEvent {StartTime=DateTime.Parse("2015/11/23 10:15:00"),EndTime=DateTime.Parse("2015/11/23 14:00:00"),SubjectID=1,RoomID=3,Attendees="SpillProg, prog",EventType=EventType.Forelesning,Comment=""},
				new CalendarEvent {StartTime=DateTime.Parse("2015/12/02 10:15:00"),EndTime=DateTime.Parse("2015/12/02 12:00:00"),SubjectID=4,RoomID=2,Attendees="SpillProg, Prog",EventType=EventType.Innlevering,Comment=""},
				new CalendarEvent {StartTime=DateTime.Parse("2015/11/25 13:15:00"),EndTime=DateTime.Parse("2015/11/25 17:00:00"),SubjectID=2,RoomID=1,Attendees="SpillProg, Int.sys.",EventType=EventType.Forelesning,Comment=""},
				new CalendarEvent {StartTime=DateTime.Parse("2015/12/15 09:00:00"),EndTime=DateTime.Parse("2015/12/15 14:00:00"),SubjectID=2,RoomID=5,Attendees="SpillProg, Prog",EventType=EventType.Eksamen,Comment=""}
			};*/


			/*var dataPath = "~/Content/dummy-data.json";
			dataPath = HttpContext.Current.Server.MapPath(dataPath);
			var json = File.ReadAllText(dataPath);

			var rootObj = JsonConvert.DeserializeObject<RootObject>(json);
            foreach (var item in rootObj.reservations)
			{
				var start = GetParsedDateTime(item.startdate, item.starttime);
				var end = GetParsedDateTime(item.enddate, item.endtime);
				var ce = new CalendarEvent
				{
					StartTime = start,
					EndTime = end,
					//SubjectName = item.columns[0], 
					SubjectID = 1,
					RoomName = item.columns[2],
					EventName = item.columns[4],
					Attendees = item.columns[1],
					Teacher = item.columns[3],
					Comment = item.columns[5]
				};
				context.CalendarEvents.Add(ce);
			}*/

			/*var calendarEvents = new List<CalendarEvent_OLD>
			{
				new CalendarEvent_OLD {Teacher="",StartTime=DateTime.Now,EndTime=DateTime.Now,SubjectID=1,Attendees="",Comment="COMMENT", RoomName="Room",EventName="Event"}
			};

			// Sort the list by the datetime 
			//		(might not be necessary if we get the data in order)
			//calendarEvents = calendarEvents.OrderBy(c => c.StartTime).ToList();

			calendarEvents.ForEach(c => context.CalendarEvents.Add(c));
			context.SaveChanges();*/
		}

		/// <summary>
		/// Parse a given string date and time to DateTime format. Returns DateTime.MinValue if the parse failed.
		/// </summary>
		private static DateTime GetParsedDateTime(string date, string time)
		{
			DateTime dt;
			if (DateTime.TryParseExact($"{date} {time}:00", Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
			{
				return dt;
			}
			return DateTime.MinValue;
		}

		/////// Below are classes for parsing the JSON ///////
		public class JsonEventInfo
		{
			public int reservationlimit { get; set; }
			public int reservationcount { get; set; }
		}

		public class JsonEvent
		{
			public string id { get; set; }
			public string startdate { get; set; }
			public string starttime { get; set; }
			public string enddate { get; set; }
			public string endtime { get; set; }
			public List<string> columns { get; set; }
		}

		public class RootObject
		{
			public List<string> columnheaders { get; set; }
			public JsonEventInfo info { get; set; }
			public List<JsonEvent> reservations { get; set; }
		}
	}
}