using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using TikTokCalendar.Extras;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class DataParser
	{
		// Filepaths are relative to the Content folder
		private const string courseFile = "SchoolSystem/courses.json";
		private const string subjectFile = "SchoolSystem/subjects.json";
		private const string courseSubjectFile = "SchoolSystem/courseSubjects.json";
		private const string examsFile = "TimeEdit/innlevering-eksamen-dato.json";
		private const string usersFile = "users.json";
		private const string contentDataFolder = "Data/";

		private const long ExamEventStartID = 500000;
			// Must be much higher than the ID's on the events from the TimeEdit json files

		private const int ColumnEmne = 0;
		private const int ColumnStudieProgram = 1;
		private const int ColumnRom = 2;
		private const int ColumnLaerer = 3;
		private const int ColumnAktivitet = 4;
		private const int ColumnKommentar = 5;
		private readonly DateTimeParser dtParser = new DateTimeParser();

		private readonly List<EventDuplicate> possibleDuplicateEvents = new List<EventDuplicate>();

		private readonly string[] scheduleFiles =
		{
			"TimeEdit/1klasse.json",
			"TimeEdit/e-business.json",
			"TimeEdit/intelligente-systemer.json",
			"TimeEdit/interaktivt-design.json",
			"TimeEdit/programmering.json",
			"TimeEdit/spilldesign.json",
			"TimeEdit/spillprogrammering.json",
			"TimeEdit/mobilprogrammering.json",
			"TimeEdit/test.json"
		};

		private long examEventID; // A unique ID for the exam events

		private Dictionary<string, Room> rooms = new Dictionary<string, Room>();

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
			var users = GetUsers();
			DataWrapper.Instance.Initialize(subjects, courses, courseSubjects, users);

			// We have to do this in two different calls, as GetEvents() has functions that depend on DataWrapper to have the info about the base SchoolSystem(subjects, courses, etc)
			var events = GetEvents();
			var allRooms = GetRoomTimeSlots(events, DateTime.Now);
			DataWrapper.Instance.SetSchoolSystemDependantData(events, allRooms);
		}

		private Dictionary<string, Room> GetRoomTimeSlots(List<CustomEvent> events, DateTime date)
		{
			//rooms = new Dictionary<string, Room>();
			foreach (var evnt in events)
			{
				// If the event doesn't have a room name or a start/end time, skip it
				if (string.IsNullOrEmpty(evnt.RoomName) || !evnt.HasStartTime || !evnt.HasEndDateTime)
				{
					continue;
				}

				// Check if the event is today
				if (evnt.StartDateTime.Month == date.Month && evnt.StartDateTime.Day == date.Day)
				{
					// Get the possible roomnames in the roomtext
					var roomNames = GetRoomsFromRoomText(evnt.RoomName);
					foreach (var room in roomNames)
					{
						// Skip if empty
						if (string.IsNullOrEmpty(room)) continue;

						// Add the timeslot if the roomdictionary has this rooms registered
						if (rooms.ContainsKey(room))
						{
							rooms[room].TryAddTimeSlot(new TimeSlot(evnt));
						}
					}
				}
			}
			return rooms;
		}

		/// <summary>
		///     Splits the given roomtext into an array (splits with ',')
		/// </summary>
		/// <param name="roomText"></param>
		/// <returns></returns>
		public string[] GetRoomsFromRoomText(string roomText)
		{
			var splitResults = roomText.Split(',');
			for (var i = 0; i < splitResults.Length; i++)
			{
				splitResults[i] = splitResults[i].Trim();
			}
			return splitResults;
		}

		/// <summary>
		///     Returns the file contents of the gives filepath (relative to the Content/ folder).
		/// </summary>
		/// <param name="contentFolderRelativePath"></param>
		/// <returns></returns>
		private string GetFileContents(string contentFolderRelativePath)
		{
			string dataPath = null;
			string ret = null;
			try
			{
				dataPath = HttpContext.Current.Server.MapPath("~/Content/" + contentDataFolder + contentFolderRelativePath);
				ret = File.ReadAllText(dataPath, Encoding.GetEncoding("iso-8859-1"));
			}
			catch (Exception e)
			{
				Debug.WriteLine("Error while getting the filecontents: " + e.Message);
				throw;
			}
			return ret;
		}

		private List<StudentUser> GetUsers()
		{
			var users = new List<StudentUser>();
			var file = GetFileContents(usersFile);
			var container = JsonConvert.DeserializeObject<JUserRootObject>(file);
			foreach (var user in container.users)
			{
				var course = (SchoolCourses) user.Course;
				users.Add(new StudentUser(user.Uname, user.Password, user.Email, user.Year, course));
			}

			return users;
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
				c.SetAndParse(courseSubject.id, courseSubject.courseId, courseSubject.subjectId, courseSubject.semester, courses,
					subjects);
				courseSubjects.Add(c);
			}
			return courseSubjects;
		}

		private List<CustomEvent> GetEvents()
		{
			var addedIDs = new List<long>();
			rooms = new Dictionary<string, Room>();
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

						var roomNames = GetRoomsFromRoomText(evnts[0].RoomName);
						foreach (var room in roomNames)
						{
							if (!string.IsNullOrEmpty(room) && !rooms.ContainsKey(room))
							{
								rooms.Add(room, new Room(room));
							}
						}
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
		///     Parses an event and returns a List
		///     <CustomEvent>
		///         .
		///         The list contains either the date of the event, the date of the first day in the week of the event, or two
		///         dates (if the event has two dates)
		/// </summary>
		public List<CustomEvent> ParseEvent(string id, string startDate, string startTime, string endDate, string endTime,
			string subjectString, string courseData, string room, string teacher, string activity, string comment)
		{
			var retEvents = new List<CustomEvent>();

			// Parse ID
			long parsedId = -1;
			long.TryParse(id, NumberStyles.Integer, new NumberFormatInfo(), out parsedId);

			// Startdate
			var dtResults = DateParseResults.NoDate;
			// TODO Use the other parse if it isn't a fucked up dateformat
			DateTime[] startDates = {dtParser.SimpleParse(startDate, startTime, out dtResults)};

			// Enddate
			var endDateTime = dtParser.SimpleParse(endDate, endTime, out dtResults);
			var hasEndDateTime = dtResults == DateParseResults.Single;

			// Get subject & code
			var subjectCode = Subject.GetSubjectCode(subjectString);
			var subject = DataWrapper.Instance.GetSubjectByCode(subjectCode);
			if (subject == null) return retEvents;

			// Get courses
			var courses = new List<SchoolCourses>();
			if (courseData != null)
			{
				// Get the courses from the courseData field
				var courseDataLines = courseData.Split(',');
				foreach (var line in courseDataLines)
				{
					var c = DataWrapper.Instance.GetCourseFromName(line);
					if (c != null)
					{
						courses.Add(c.SchoolCourse);
					}
				}
			}

			// Make sure there are no duplicates
			// NOTE: This is only necesarry if we aren't sure all the events have correct IDs (we don't have to use this when we use data straight from timeedit)
			if (courses.Count > 1)
			{
				if (IsEventDuplicate(subject.Code, id, room, startDates[0]))
				{
					return retEvents;
				}
			}

			// Get years
			var years = GetClassYearsForEvent(courses, subject);

			// Get event type
			var eventType = ParseEventType(activity);

			// Go through the startdates that was parsed.
			// This makes it so that events where we couldn't parse a date from, will not be added
			foreach (var date in startDates)
			{
				var evnt = new CustomEvent(parsedId, date, true, endDateTime, hasEndDateTime,
					subject, years, courses, room, teacher, eventType, comment, 0);
				retEvents.Add(evnt);
			}
			return retEvents;
		}

		private bool IsEventDuplicate(string subjectCode, string eventId, string roomName, DateTime startDateTime)
		{
			// Make a EventDuplicate object with this events info
			var eventDup = new EventDuplicate(subjectCode, eventId, roomName, startDateTime);

			// Try to find eventDup in the duplicate list
			Predicate<EventDuplicate> eventFinder = e => e == eventDup;
			var possibleDuplicate = possibleDuplicateEvents.Find(eventFinder);

			// Check if we have a possible duplicate, and if the IDs doesn't match up
			if (possibleDuplicate != null && possibleDuplicate.ID != eventDup.ID)
			{
				// Event is a duplicate
				return true;
			}

			// Add the event to the possible duplicate list
			possibleDuplicateEvents.Add(eventDup);
			return false;
		}

		public List<CustomEvent> ParseExamEvent(string startDate, string subjectCode, string subjectName, string activity,
			int weighting, string duration, string helpers)
		{
			var retEvents = new List<CustomEvent>();

			// TODO Give examevents that are the same (same startdate, subject and eventtype) the same ID to prevent duplicates
			var id = examEventID;
			examEventID++;

			//////// Start date ////////
			// Startdate
			var dtResults = DateParseResults.NoDate;
			// TODO Use the other parse if it isn't a fucked up dateformat
			var startDates = dtParser.ParseDate(startDate, out dtResults);
			if (startDates.Length <= 0) return retEvents;

			//////// Subject ////////
			var subject = DataWrapper.Instance.GetSubjectByCode(subjectCode);
			if (subject == null) return retEvents;

			//////// Year and course ////////
			var courses = DataWrapper.Instance.GetCoursesWithSubject(subject);
			if (courses.Count <= 0)
			{
				return retEvents;
			}
			var years = GetClassYearsForEvent(courses, subject);

			//////// Making the events ////////
			var eventType = ParseEventType(activity);

			var comment = "Vekting: " + weighting + "\nVarighet: " + duration + "\nHjelpemidler: " + helpers;

			//////// Making the events ////////
			// Go through the startdates that was parsed.
			// This makes it so that events where we couldn't parse a date from, will not be added
			foreach (var date in startDates)
			{
				var evnt = new CustomEvent(id, date, false, DateTime.MinValue, false,
					subject, years, courses, Resources.LocalizedText.RoomInfoOnITL, null, eventType, comment, weighting);
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
					var y = CourseSubject.GetClassYearFromSemester(cs.Semester);
					years.Add(y);
				}
			}
			return years;
		}

		private EventType ParseEventType(string text)
		{
			var eventType = EventType.Annet;
			var evntName = text.ToLower();
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

		private class EventDuplicate
		{
			private readonly string roomName;
			private readonly DateTime startDateTime;
			private readonly string subjectCode;

			public EventDuplicate(string subjectCode, string id, string roomName, DateTime startDateTime)
			{
				this.subjectCode = subjectCode;
				ID = id;
				this.roomName = roomName;
				this.startDateTime = startDateTime;
			}

			public string ID { get; }

			public override bool Equals(object obj)
			{
				try
				{
					return this == (EventDuplicate) obj;
				}
				catch
				{
					return false;
				}
			}

			public static bool operator ==(EventDuplicate a, EventDuplicate b)
			{
				if (ReferenceEquals(a, b))
				{
					return true;
				}
				if (ReferenceEquals(a, null) ||
				    ReferenceEquals(b, null))
				{
					return false;
				}

				return a.subjectCode == b.subjectCode && a.roomName == b.roomName && a.startDateTime == b.startDateTime;
			}

			public static bool operator !=(EventDuplicate a, EventDuplicate b)
			{
				return !(a == b);
			}
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

		public class JUser
		{
			public string Uname { get; set; }
			public string Password { get; set; }
			public string Email { get; set; }
			public int Course { get; set; }
			public int Year { get; set; }
		}

		public class JUserRootObject
		{
			public List<JUser> users { get; set; }
		}
	}
}