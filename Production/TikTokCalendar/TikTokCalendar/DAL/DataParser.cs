using Newtonsoft.Json;
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
		private readonly DateTimeParser dtParser = new DateTimeParser();

		public void ParseAllData()
		{
			var subjects = GetSubjects();
			var courses = GetCourses();
			DataWrapper.Instance.SetData(subjects, courses, GetCourseSubjects(subjects, courses));
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
				s.SetAndParse(subject.id, subject.name);
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
			DateTime[] startDates = dtParser.ParseDate(startDate, out dtResults);

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
			// Go through the startdates that was parsed.
			// This makes it so that events where we couldn't parse a date from, will not be added
			foreach (var date in startDates)
			{
				CustomEvent evnt = new CustomEvent(parsedId, DateTime.MinValue, endDateTime, hasEndDateTime, subject, classYear, courses, room, teacher, activity, comment);
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
	}
}