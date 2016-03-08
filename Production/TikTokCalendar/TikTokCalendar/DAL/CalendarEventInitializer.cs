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
		private List<Subject> subjects = new List<Subject>();
		protected override void Seed(CalendarEventContext context)
		{
			//InsertDummyData(context);
			//ReadJsonFile(context);
			//new ExamInit(context);
			//InitializeDummy();
		}

		public void InitializeDummy()
		{
			// Parse all the data
			DataParser dataParser = new DataParser();
			dataParser.ParseAllData();
		}

		/// <summary>
		/// Inserts dummy data to the database.
		/// </summary>
		private void InsertDummyData(CalendarEventContext context)
		{
			ExamInit exam = new ExamInit(this);
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
			subjects = new List<Subject>
			{
				new Subject { Name="Prosjekt software engineering (PJ3100-15)" },
				new Subject { Name="Matematikk og Fysikk (RF3100-15)" },
				new Subject { Name="C++ Programmering (PG4400-15)" },
				new Subject { Name="Game AI (PG4500-15)" },
				new Subject { Name="Embedded systems (PG5500-15)" },
				new Subject { Name="Mobil utvikling (PG4600-15)" },
				new Subject { Name="Ruby on Rails (PG4300-15)" },
				new Subject { Name="Avansert Javaprogrammering (PG4300-15)" },
				new Subject { Name="Undersøkelsesmetoder (PJ6100-15)" },
				new Subject { Name="Enterprise programmering 2 (PG6100-15)" }
			};
			subjects.ForEach(s => context.Subjects.Add(s));
			context.SaveChanges();

			// CourseSubjects
			// TODO Semester might not be necessary, as TimeEdit only show stuff for this semester
			var courseSubjects = new List<CourseSubject>
			{
				// Spillprog
				new CourseSubject { CourseID=2, SubjectID=1, Semester=4 },
				new CourseSubject { CourseID=2, SubjectID=2, Semester=4 },
				new CourseSubject { CourseID=2, SubjectID=3, Semester=4 },
				new CourseSubject { CourseID=2, SubjectID=4, Semester=4 },
				
				// Intsys
				new CourseSubject { CourseID=3, SubjectID=1, Semester=4 },
				new CourseSubject { CourseID=3, SubjectID=3, Semester=4 },
				new CourseSubject { CourseID=3, SubjectID=5, Semester=4 },
				new CourseSubject { CourseID=3, SubjectID=6, Semester=4 },

				// Prog 2.klasse
				new CourseSubject { CourseID=1, SubjectID=1, Semester=4 },
				new CourseSubject { CourseID=1, SubjectID=6, Semester=4 },
				new CourseSubject { CourseID=1, SubjectID=7, Semester=4 },
				new CourseSubject { CourseID=1, SubjectID=8, Semester=4 },

				// Prog 3.klasse
				new CourseSubject { CourseID=1, SubjectID=9, Semester=6 },
				new CourseSubject { CourseID=1, SubjectID=10, Semester=6 }

			};
			courseSubjects.ForEach(c => context.CourseSubject.Add(c));
			context.SaveChanges();

			// Parse the .json and insert into the dummy DB
			List<CalendarEvent> liste = new List<CalendarEvent>();
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
					// Compare subject codes
					//if (item.columns[0].Substring(0, 7).Equals(subj.Name.Substring(0, 7)))
					if (item.columns[0].Substring(item.columns[0].Length - 11).Equals(subj.Name.Substring(subj.Name.Length - 11)))
					{
						subject = subj.ID;
						break;
					}
				}
				int timeEditId = -1;
				int.TryParse(item.id, out timeEditId);

				// What classyear this item is for
				int year = 1;
				if (item.columns[1].Contains("2.klasse"))
				{
					year = 2;
				}
				else if (item.columns[1].Contains("3.klasse"))
				{
					year = 3;
				}
				//int.TryParse(item.columns[0], out year);

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
					Comment = item.columns[5],
					Year = year
				};
				// Add the event to the DB events list
				liste.Add(ce);
				//liste.Add(exam.ReadJsonFile());
			}

			foreach(var item in exam.ReadJsonFile()) {
				context.CalendarEvents.Add(item);
			}
			context.CalendarEvents.AddRange(liste);
			context.SaveChanges();

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
		}

		/// <summary>
		/// Parse a given string date and time to DateTime format. Returns DateTime.MinValue if the parse failed.
		/// </summary>
		/// 
		public int GetSubjectIdFromCode(string code)
		{
			foreach (var subject in subjects)
			{
				if (code == subject.GetSubjectCode())
				{
					return subject.ID;
				}
			}
			return 1;
		}

		public static DateTime GetParsedDateTime(string date, string time)
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

		public class Reservation
		{
			public string Dato { get; set; }
			public string Emnekode { get; set; }
			public string Emnenavn { get; set; }
			public string Vurderingstype { get; set; }
			public int Vekting { get; set; }
			public string Varighet { get; set; }
			public string Hjelpemidler { get; set; }
		}

		public class RootObject2
		{
			public List<Reservation> reservations { get; set; }
		}
	}
}