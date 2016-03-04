﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
		private readonly string[] scheduleFiles = new string[]
		{
			"timeedit/1klasse.json",
			"timeedit/e-business.json",
			"timeedit/intelligente-systemer.json",
			"timeedit/interaktivt-design.json",
			"timeedit/programmering.json",
			"timeedit/spilldesign.json",
			"timeedit/spillprogrammering.json"
		};

		public const int ColumnEmne = 0;
		public const int ColumnStudieProgram = 1;
		public const int ColumnRom = 2;
		public const int ColumnLaerer = 3;
		public const int ColumnAktivitet = 4;
		public const int ColumnKommentar = 5;
		private readonly DateTimeParser dtParser = new DateTimeParser();

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

			foreach (var item in events)
			{
				Printer.Print("Event: " + item.EventName);
			}
		}

		private string GetFileContents(string contentFolderRelativePath)
		{
			var dataPath = HttpContext.Current.Server.MapPath("~/Content/" + contentFolderRelativePath);
			return File.ReadAllText(dataPath);
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
			var events = new List<CustomEvent>();
			// TODO ParseEvent() with all the schedules jsons from TimeEdit
			// TODO ParseEvent() with the eksamen/innlevering json

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
					events.AddRange(evnts);
				}
			}

			file = GetFileContents(examsFile);
			var container = JsonConvert.DeserializeObject<JRootExamReservationRootObject>(file);
			foreach (var r in container.reservations)
			{
				//var evnts = ParseEvent("-1", r.Dato, null, null, null, string.Format("{0}({1})", r.Emnenavn, r.Emnekode), null,
				//	null, null, r.Vurderingstype, "Vekting: " + r.Vekting + "\n" + r.Hjelpemidler);
				var evnts = ParseExamEvent(r.Dato, r.Emnenavn, r.Emnekode, r.Vurderingstype, r.Vekting.ToString(), r.Varighet, r.Hjelpemidler);
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
			int parsedId = -1;
			int.TryParse(id, NumberStyles.Integer, new NumberFormatInfo(), out parsedId);

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

			//////// Year and course ////////
			List<SchoolCourses> courses = new List<SchoolCourses>();
			int classYear = 1;
			if (courseData != null)
			{
				// Figure out the classyear from the coursedata field
				if (courseData.Contains("2.klasse"))
				{
					classYear = 2;
				}
				else if (courseData.Contains("3.klasse"))
				{
					classYear = 3;
				}

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

			//////// Making the events ////////
			EventType eventType = EventType.None;
			int bestMatch = 1000;
			string[] events = Enum.GetNames(typeof(SchoolCourses));
			for (int i = 1; i < events.Length + 1; i++)
			{
				int match = Math.Abs(activity.CompareTo(events[i - 1]));
				if (match <= bestMatch)
				{
					eventType = (EventType)i;
					bestMatch = match;
				}
			}

			//////// Making the events ////////
			// Go through the startdates that was parsed.
			// This makes it so that events where we couldn't parse a date from, will not be added
			foreach (var date in startDates)
			{
				CustomEvent evnt = new CustomEvent(parsedId, DateTime.MinValue, endDateTime, hasEndDateTime,
					subject, classYear, courses, room, teacher, eventType, comment);
				retEvents.Add(evnt);
			}
			return retEvents;
		}

		public List<CustomEvent> ParseExamEvent(string startDate, string subjectCode, string subjectName, string activity, string weighting, string duration, string helpers)
		{
			List<CustomEvent> retEvents = new List<CustomEvent>();

			//////// Start date ////////
			// Startdate
			DateParseResults dtResults = DateParseResults.NoDate;
			// TODO Use the other parse if it isn't a fucked up dateformat
			DateTime[] startDates = dtParser.ParseDate(startDate, out dtResults);
			
			//////// Subject ////////
			// TODO Null check
			//string subjectCode = Subject.GetSubjectCode(subjectString);
			Subject subject = DataWrapper.Instance.GetSubjectByCode(subjectCode);

			// TODO Figure out a better way to do this
			// Subject basically cant be null
			if (subject == null) return retEvents; 

			//////// Year and course ////////
			List<SchoolCourses> courses = DataWrapper.Instance.GetCoursesWithSubject(subject);
			// TODO Get courses with a subject
			

			//////// Making the events ////////
			EventType eventType = EventType.None;
			int bestMatch = 1000;
			string[] events = Enum.GetNames(typeof(EventType));
			for (int i = 1; i < events.Length + 1; i++)
			{
				int match = Math.Abs(activity.CompareTo(courses[i - 1]));
				if (match <= bestMatch)
				{
					eventType = (EventType)i;
					bestMatch = match;
				}
			}

			string comment = "Vekting: " +  weighting + "\nVarighet: " + duration + "\nHjelpemidler: " + helpers;

			//////// Making the events ////////
			// Go through the startdates that was parsed.
			// This makes it so that events where we couldn't parse a date from, will not be added
			foreach (var date in startDates)
			{
				CustomEvent evnt = new CustomEvent(-1, date, DateTime.MinValue, false,
					subject, -1, courses, null, null, eventType, comment);
				retEvents.Add(evnt);
			}
			return retEvents;
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