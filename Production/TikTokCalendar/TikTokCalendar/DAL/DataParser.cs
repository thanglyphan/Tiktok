using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using TikTokCalendar.Extras;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class DataParser
	{
		// Filepaths are relative to the Content folder
		private const string eventFile = "dummy-data.json";
		private const string courseFile = "SchoolSystem/courses.json";
		private const string subjectFile = "SchoolSystem/subjects.json";
		private const string courseSubjectFile = "SchoolSystem/courseSubjects.json";
		private const string examsFile = "timeedit/innlevering-eksamen-dato.json";
		private readonly string[] scheduleFiles = new string[] {
			"timeedit/1klasse.json",
			"timeedit/e-business.json",
			"timeedit/intelligente-systemer.json",
			"timeedit/interaktivt-design.json",
			"timeedit/programmering.json",
			"timeedit/spilldesign.json",
			"timeedit/spillprogrammering.json",
			"timeedit/mobilprogrammering.json",
			"timeedit/test.json"
		};

		private const long ExamEventStartID = 500000; // Must be much higher than the ID's on the events from the TimeEdit json files
		private long examEventID; // A unique ID for the exam events
		private const int ColumnEmne = 0;
		private const int ColumnStudieProgram = 1;
		private const int ColumnRom = 2;
		private const int ColumnLaerer = 3;
		private const int ColumnAktivitet = 4;
		private const int ColumnKommentar = 5;
		private readonly DateTimeParser dtParser = new DateTimeParser();

		public DataParser()
		{
			examEventID = ExamEventStartID;
		}

		public void ParseAllData()
		{
			// Initialize the base school system data for the wrapper
			var subjects = GetSubjects();
			var courses = GetCourses();
			var courseSubjects = GetCourseSubjects(subjects, courses);
			DataWrapper.Instance.Initialize(subjects, courses, courseSubjects);

			// We have to do this in two different calls, as GetEvents() has functions that depend on DataWrapper to have the info about the base SchoolSystem(subjects, courses, etc)
			var events = GetEvents();
			DataWrapper.Instance.SetSchoolSystemDependantData(events);
		}

		private string GetFileContents(string contentFolderRelativePath)
		{

			string dataPath = null;
			string ret = null;
			try
			{
				dataPath = HttpContext.Current.Server.MapPath("~/Content/" + contentFolderRelativePath);
				ret = File.ReadAllText(dataPath, Encoding.GetEncoding("iso-8859-1"));
			}
			catch (Exception e)
			{
				Debug.WriteLine("Error while getting the filecontents: " + e.Message);
				throw;
			}
			return ret;
		}

		private List<Subject> GetSubjects()
		{
			var subjects = new List<Subject>();
			var file = GetFileContents(subjectFile);
			var container = JsonConvert.DeserializeObject<JRootSubjectObject>(file);
			foreach (var subject in container.subject)
			{
				var s = new Subject();
				s.SetAndParse(subject.id, subject.name, subject.code);
				subjects.Add(s);
			}

			return subjects;
		}

		private List<Course> GetCourses()
		{
			var courses = new List<Course>();
			var file = GetFileContents(courseFile);
			var container = JsonConvert.DeserializeObject<JRootCourseObject>(file);
			foreach (var course in container.courses)
			{
				var c = new Course();
				c.SetAndParse(course.id, course.name);
				courses.Add(c);
			}
			return courses;
		}

		private List<CourseSubject> GetCourseSubjects(List<Subject> subjects, List<Course> courses)
		{
			var courseSubjects = new List<CourseSubject>();
			var file = GetFileContents(courseSubjectFile);
			var container = JsonConvert.DeserializeObject<JRootCourseSubjectObject>(file);
			foreach (var courseSubject in container.courseSubjects)
			{
				var c = new CourseSubject();
				c.SetAndParse(courseSubject.id, courseSubject.courseId, courseSubject.subjectId, courseSubject.semester, courses, subjects);
				courseSubjects.Add(c);
			}
			return courseSubjects;
		}

		private List<CustomEvent> GetEvents()
		{
			List<long> addedIDs = new List<long>();
			var events = new List<CustomEvent>();

			var file = "";
			foreach (var fileName in scheduleFiles)
			{
				file = GetFileContents(fileName);
				var resContainer = JsonConvert.DeserializeObject<JRootReservationObject>(file);
				foreach (var r in resContainer.reservations)
				{
					var evnts = ParseEvent(r.id, r.startdate, r.starttime, r.enddate, r.endtime, r.columns[ColumnEmne],
						r.columns[ColumnStudieProgram], r.columns[ColumnRom], r.columns[ColumnLaerer], r.columns[ColumnAktivitet],
						r.columns[ColumnKommentar]);
					if (evnts.Count > 0 && !addedIDs.Contains(evnts[0].ID))
					{
						events.AddRange(evnts);
						addedIDs.Add(evnts[0].ID);
					}
				}
			}

			file = GetFileContents(examsFile);
			var container = JsonConvert.DeserializeObject<JRootExamReservationRootObject>(file);
			foreach (var r in container.reservations)
			{
				var evnts = ParseExamEvent(r.Dato, r.Emnekode, r.Emnenavn, r.Vurderingstype, r.Vekting, r.Varighet, r.Hjelpemidler);
				events.AddRange(evnts);
			}
			return events;
		}

		/// <summary>
		/// Parses an event and returns a List<CustomEvent>. 
		/// The list contains either the date of the event, the date of the first day in the week of the event, or two dates (if the event has two dates)
		/// </summary>
		public List<CustomEvent> ParseEvent(string id, string startDate, string startTime, string endDate, string endTime,
			string subjectString, string courseData, string room, string teacher, string activity, string comment)
		{
			List<CustomEvent> retEvents = new List<CustomEvent>();

			//////// Event ID ////////
			// Parse ID
			long parsedId = -1;
			long.TryParse(id, NumberStyles.Integer, new NumberFormatInfo(), out parsedId);

			//////// Start date ////////
			// Startdate
			DateParseResults dtResults = DateParseResults.NoDate;
			// TODO Use the other parse if it isn't a fucked up dateformat
			DateTime[] startDates = new DateTime[] { dtParser.SimpleParse(startDate, startTime, out dtResults) };

			//////// End date ////////
			// Enddate
			DateTime endDateTime = dtParser.SimpleParse(endDate, endTime, out dtResults);
			bool hasEndDateTime = (dtResults == DateParseResults.Single);

			//////// Subject ////////
			// TODO Null check
			string subjectCode = Subject.GetSubjectCode(subjectString);
			Subject subject = DataWrapper.Instance.GetSubjectByCode(subjectCode);
			if (subject == null) return retEvents;

			//////// Year and course ////////
			List<SchoolCourses> courses = new List<SchoolCourses>();
			if (courseData != null)
			{
				// Figure out the classyear from the coursedata field
				//string courseName = courseData.ToLower();
				//if (courseName.Contains("bachelor i it")) {
				//	years.Add(1);
				//}
				//if (courseName.Contains("2.klasse")) {
				//	years.Add(2);
				//}
				//if (courseName.Contains("3.klasse")) {
				//	years.Add(3);
				//}

				// Get the courses from the courseData field
				string[] courseDataLines = courseData.Split(',');
				foreach (var line in courseDataLines)
				{
					Course c = DataWrapper.Instance.GetCourseFromName(line);
					if (c != null)
					{
						courses.Add(c.SchoolCourse);
					}
				}
			}
			var years = GetClassYearsForEvent(courses, subject);

			//////// Making the events ////////
			EventType eventType = ParseEventType(activity);

			//int bestMatch = 1000;
			//string[] events = Enum.GetNames(typeof(SchoolCourses));
			//for (int i = 1; i < events.Length + 1; i++)
			//{
			//	int match = Math.Abs(activity.CompareTo(events[i - 1]));
			//	if (match <= bestMatch)
			//	{
			//		eventType = (EventType)i;
			//		bestMatch = match;
			//	}
			//}

			//////// Making the events ////////
			// Go through the startdates that was parsed.
			// This makes it so that events where we couldn't parse a date from, will not be added
			foreach (var date in startDates)
			{
				CustomEvent evnt = new CustomEvent(parsedId, date, true, endDateTime, hasEndDateTime,
					subject, years, courses, room, teacher, eventType, comment, 0);
				retEvents.Add(evnt);
			}
			return retEvents;
		}

		public List<CustomEvent> ParseExamEvent(string startDate, string subjectCode, string subjectName, string activity, int weighting, string duration, string helpers)
		{
			List<CustomEvent> retEvents = new List<CustomEvent>();

			// TODO Give examevents that are the same (same startdate, subject and eventtype) the same ID to prevent duplicates
			long id = examEventID;
			examEventID++;

			//////// Start date ////////
			// Startdate
			DateParseResults dtResults = DateParseResults.NoDate;
			// TODO Use the other parse if it isn't a fucked up dateformat
			DateTime[] startDates = dtParser.ParseDate(startDate, out dtResults);
			if (startDates.Length <= 0) return retEvents;

			//////// Subject ////////
			Subject subject = DataWrapper.Instance.GetSubjectByCode(subjectCode);
			if (subject == null) return retEvents;

			//////// Year and course ////////
			List<SchoolCourses> courses = DataWrapper.Instance.GetCoursesWithSubject(subject);
			if (courses.Count <= 0)
			{
				return retEvents;
			}
			var years = GetClassYearsForEvent(courses, subject);

			//////// Making the events ////////
			EventType eventType = ParseEventType(activity);

			string comment = "Vekting: " + weighting + "\nVarighet: " + duration + "\nHjelpemidler: " + helpers;

			//////// Making the events ////////
			// Go through the startdates that was parsed.
			// This makes it so that events where we couldn't parse a date from, will not be added
			foreach (var date in startDates)
			{
				CustomEvent evnt = new CustomEvent(id, date, false, DateTime.MinValue, false,
					subject, years, courses, null, null, eventType, comment, weighting);
				retEvents.Add(evnt);
			}
			return retEvents;
		}

		private HashSet<int> GetClassYearsForEvent(List<SchoolCourses> courses, Subject subject)
		{
			var years = new HashSet<int>();
			foreach (var sc in courses)
			{
				var courseSubjs = DataWrapper.Instance.GetCourseSubjectWithSchoolCourseSubject(sc, subject);
				foreach (var cs in courseSubjs)
				{
					int y = CourseSubject.GetClassYearFromSemester(cs.Semester);
					years.Add(y);
				}
			}
			return years;
		}

		private EventType ParseEventType(string text)
		{
			EventType eventType = EventType.Annet;
			string evntName = text.ToLower();
			if (evntName.Contains("hjemmeeksamen"))
			{
				eventType = EventType.Hjemmeeksamen;
			}
			else if (evntName.Contains("fremføring"))
			{
				eventType = EventType.Fremforing;
			}
			else if (evntName.Contains("øving"))
			{
				eventType = EventType.Oving;
			}
			else if (evntName.Contains("skriftlig eksamen") || evntName == "skriftlig")
			{
				eventType = EventType.SkriftligEksamen;
			}
			else
			{
				if (!Enum.TryParse(text, true, out eventType))
				{
					eventType = EventType.Annet;
				}
			}
			return eventType;
		}


		//////// JSON Classes ////////
		private class JCourseSubject
		{
			public string id { get; set; }
			public string courseId { get; set; }
			public string subjectId { get; set; }
			public string semester { get; set; }
		}

		private class JRootCourseSubjectObject
		{
			public List<JCourseSubject> courseSubjects { get; set; }
		}

		private class JSubject
		{
			public string id { get; set; }
			public string code { get; set; }
			public string name { get; set; }
		}

		private class JRootSubjectObject
		{
			public List<JSubject> subject { get; set; }
		}

		private class JCourse
		{
			public string id { get; set; }
			public string name { get; set; }
		}

		private class JRootCourseObject
		{
			public List<JCourse> courses { get; set; }
		}

		private class JInfo
		{
			public int reservationlimit { get; set; }
			public int reservationcount { get; set; }
		}

		private class JReservation
		{
			public string id { get; set; }
			public string startdate { get; set; }
			public string starttime { get; set; }
			public string enddate { get; set; }
			public string endtime { get; set; }
			public List<string> columns { get; set; }
		}

		private class JRootReservationObject
		{
			public List<string> columnheaders { get; set; }
			public JInfo info { get; set; }
			public List<JReservation> reservations { get; set; }
		}

		private class JExamReservation
		{
			public string Dato { get; set; }
			public string Emnekode { get; set; }
			public string Emnenavn { get; set; }
			public string Vurderingstype { get; set; }
			public int Vekting { get; set; }
			public string Varighet { get; set; }
			public string Hjelpemidler { get; set; }
		}

		private class JRootExamReservationRootObject
		{
			public List<JExamReservation> reservations { get; set; }
		}
	}
}