using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TikTokCalendar.DAL;
using TikTokCalendar.Extras;

namespace TikTokCalendar.Models
{
	// Collection of the .json data
	public class DataWrapper
	{
		static DataWrapper()
		{
		}

		// Singleton stuff
		private DataWrapper()
		{
		}

		public static DataWrapper Instance { get; } = new DataWrapper();

		// TODO Possibly put these in dictionaries for fast lookup????
		public List<Subject> Subjects { get; private set; }
		public List<Course> Courses { get; private set; }
		public List<CourseSubject> CourseSubjects { get; private set; }
		public List<CustomEvent> AllEvents { get; private set; }
		public List<StudentUser> Users { get; private set; }
		public Dictionary<string, Room> Rooms { get; private set; }

		public void Initialize(List<Subject> subjs, List<Course> courses, List<CourseSubject> courseSubjs,
			List<StudentUser> users)
		{
			Subjects = subjs;
			Courses = courses;
			CourseSubjects = courseSubjs;
			Users = users;
		}

		public void SetSchoolSystemDependantData(List<CustomEvent> allEvents, Dictionary<string, Room> rooms)
		{
			AllEvents = allEvents.OrderBy(x => x.StartDateTime).ToList();
			Rooms = rooms;
		}

		public bool IsValidUser(StudentUser user)
		{
			if (user == null) return false;

			foreach (var u in Users)
			{
				if (user.IsValid(u))
				{
					return true;
				}
			}
			return false;
		}

		public StudentUser GetUser(string name, string password)
		{
			// TODO User Predicate<>
			foreach (var user in Users)
			{
				if (user.UserName == name && user.Password == password)
				{
					return user;
				}
			}
			return null;
		}

		public List<CustomEventMonth> GetEventsWithUser(StudentUser user)
		{
			var months = new List<CustomEventMonth>();
			CustomEventMonth month = null;
			CustomEventWeek week = null;
			var addedIDs = new HashSet<long>();

			foreach (var evnt in AllEvents)
			{
				if (!addedIDs.Contains(evnt.ID) && (user.Course == SchoolCourses.VisAlt
				                                    || (evnt.Courses.Contains(user.Course) && evnt.IsYear(user.ClassYear))))
				{
					var m = AddEvent(evnt, ref month, ref week);
					if (m != null)
					{
						months.Add(m);
						addedIDs.Add(evnt.ID);
					}
				}
			}
			return months;
		}

		public List<CustomEventMonth> GetEventsWithName(StudentUser user)
		{
			var months = new List<CustomEventMonth>();
			CustomEventMonth month = null;
			CustomEventWeek week = null;

			foreach (var evnt in AllEvents)
			{
				if (user.Course == SchoolCourses.VisAlt
				    || (evnt.Courses.Contains(user.Course) && evnt.IsYear(user.ClassYear)))
				{
					var m = AddEvent(evnt, ref month, ref week);
					if (m != null)
					{
						months.Add(m);
					}
				}
			}
			return months;
		}

		public List<CustomEventMonth> GetEventsWithName(StudentUser user, string tags, bool lecture, bool assignment,
			bool exam)
		{
			var months = new List<CustomEventMonth>();
			CustomEventMonth month = null;
			CustomEventWeek week = null;

			var temp = "";
			string[] array = null;
			if (!string.IsNullOrEmpty(tags))
			{
				temp = FilterCharacters(tags).ToLower();
				array = temp.Split(' ');
			}

			foreach (var evnt in AllEvents)
			{
				if (user.Course == SchoolCourses.VisAlt
				    || (evnt.Courses.Contains(user.Course) && evnt.IsYear(user.ClassYear)))
				{
					if ((lecture && evnt.MainEventType == MainEventType.Forelesning)
					    || (assignment && evnt.MainEventType == MainEventType.Innlevering)
					    || (exam && evnt.MainEventType == MainEventType.Eksamen))
					{
						if (!string.IsNullOrEmpty(tags))
						{
							string[] eventInfo =
							{
								evnt.Subject.Name, evnt.RoomName, evnt.Teacher, evnt.Comment, evnt.Subject.Code, evnt.eventType.ToString(),
								evnt.StartDateTime.Day.ToString(), evnt.StartDateTime.GetWeekNumberOfYear().ToString(), evnt.GetMonthName()
							};

							var flagged = false;
							for (var j = 0; j < array.Length; j++)
							{
								var empty = 0;
								for (var k = 0; k < eventInfo.Length; k++)
								{
									if (!eventInfo[k].Contains(array[j], StringComparison.OrdinalIgnoreCase))
									{
										empty++;
									}
								}
								if (empty == eventInfo.Length)
								{
									flagged = true;
									break;
								}
							}
							if (flagged)
							{
								continue;
							}
						}

						var m = AddEvent(evnt, ref month, ref week);
						if (m != null)
						{
							months.Add(m);
						}
					}
				}
			}
			return months;
		}

		private CustomEventMonth AddEvent(CustomEvent evnt, ref CustomEventMonth month, ref CustomEventWeek week)
		{
			CustomEventMonth retMonth = null;

			var curDate = evnt.StartDateTime;
			var newMonth = false;
			if (month == null || month.MonthNumber != curDate.Month)
			{
				// New month
				month = new CustomEventMonth(curDate.Month);
				//months.Add(month);
				retMonth = month;
				newMonth = true;
			}

			if (week == null || newMonth || curDate.GetWeekNumberOfYear() != week.WeekNumber)
			{
				// New week
				var weekNr = curDate.GetWeekNumberOfYear();
				week = new CustomEventWeek(weekNr, 1);

				month.Weeks.Add(week);
			}
			week.events.Add(evnt);
			return retMonth;
		}

		public List<CustomEventMonth> GetEventsWithSubject(Subject subject)
		{
			var months = GetInitializedEventMonthList();
			foreach (var evnt in AllEvents)
			{
				if (evnt.Subject == subject)
				{
					months[evnt.StartDateTime.Month - 1].AddEvent(evnt);
				}
			}
			return months;
		}

		private List<CustomEventMonth> GetInitializedEventMonthList()
		{
			var months = new List<CustomEventMonth>();
			var weekNr = 1;
			for (var i = 0; i < 12; i++)
			{
				months.Add(new CustomEventMonth(i + 1));
				var today = DateTime.Today;
				var date = new DateTime(today.Year, i + 1, 1);
				//DateTime date = DateTime.Today;
				// first generate all dates in the month of 'date'

				// TODO Here correctly get the start of each week
				//var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));
				//// then filter the only the start of weeks
				//var weekends = from d in dates where d.DayOfWeek == DayOfWeek.Monday select d;

				var numOfWeeks = MondaysInMonth(date);
				//foreach (var weeks in weekends)
				for (var j = 0; j < numOfWeeks + 1; j++)
				{
					months[i].Weeks.Add(new CustomEventWeek(weekNr, j + 1)); //weeks.GetWeekNumberOfYear()));
					weekNr++;
				}
			}
			return months;
		}

		private int MondaysInMonth(DateTime thisMonth)
		{
			var mondays = 0;
			var month = thisMonth.Month;
			var year = thisMonth.Year;
			var daysThisMonth = DateTime.DaysInMonth(year, month);
			var beginingOfThisMonth = new DateTime(year, month, 1);
			for (var i = 0; i < daysThisMonth; i++)
				if (beginingOfThisMonth.AddDays(i).DayOfWeek == DayOfWeek.Monday)
					mondays++;
			return mondays;
		}

		/// <summary>
		///     Tries to find a Subject with the code specified. Returns null if it couldn't find a mathcing Subject.
		/// </summary>
		public Subject GetSubjectByCode(string code)
		{
			Subject subject = null;
			if (code != null)
			{
				foreach (var subj in Subjects)
				{
					var e = subj.Code.Equals(code, StringComparison.OrdinalIgnoreCase);
					//Printer.Print(string.Format("{0} == {1} = {2}", subj.Code, code, e));
					if (e)
					{
						subject = subj;
						break;
					}
				}
			}
			return subject;
		}

		/// <summary>
		///     Tries to find a Course with a Name that is in the specified name. Returns null if it couldn't find a mathcing
		///     Course.
		/// </summary>
		public Course GetCourseFromName(string name)
		{
			Course c = null;
			foreach (var course in Courses)
			{
				if (name.Contains(course.Name, StringComparison.OrdinalIgnoreCase))
				{
					c = course;
					break;
				}
			}
			return c;
		}

		// TODO Possible problem with this function: Will return all coruses with the subject without accounting for semesters (programming has C in 5. semester, along with gamesprog in 3.semester)
		public List<SchoolCourses> GetCoursesWithSubject(Subject subject)
		{
			var retList = new List<SchoolCourses>();
			if (subject != null)
			{
				foreach (var cs in CourseSubjects)
				{
					if (cs.Subject.ID == subject.ID && !retList.Contains(cs.Course.SchoolCourse))
					{
						retList.Add(cs.Course.SchoolCourse);
					}
				}
			}
			return retList;
		}

		public List<CourseSubject> GetCourseSubjectWithSchoolCourseSubject(SchoolCourses schoolCourse, Subject subject)
		{
			var retList = new List<CourseSubject>();
			var courseSubjs = GetCourseSubjectWithSchollCourse(schoolCourse);
			foreach (var cs in courseSubjs)
			{
				if (cs.SubjectID == subject.ID)
				{
					retList.Add(cs);
				}
			}
			return retList;
		}

		public List<CourseSubject> GetCourseSubjectWithSchollCourse(SchoolCourses course)
		{
			var retList = new List<CourseSubject>();
			foreach (var cs in CourseSubjects)
			{
				if (cs.Course.SchoolCourse == course)
				{
					retList.Add(cs);
				}
			}
			return retList;
		}

		/// <summary>
		///     Gets all keywords from AllEvents (user specific, mainly for the autocomplete)
		/// </summary>
		public List<string> GetUserKeywords(StudentUser user)
		{
			var list = new List<string>();
			foreach (var evnt in AllEvents)
			{
				if ((evnt.Courses.Contains(user.Course) && evnt.IsYear(user.ClassYear)) || user.Course == SchoolCourses.VisAlt)
				{
					string[] eventWords =
					{
						evnt.Subject.Name, evnt.RoomName, evnt.Teacher,
						evnt.Subject.Code, evnt.eventType.ToString(), evnt.GetMonthName()
					};

					for (var i = 0; i < eventWords.Length; i++)
					{
						if (eventWords[i] != null)
						{
							eventWords[i].ToLower();
							if (!list.Contains(eventWords[i]))
							{
								list.Add(eventWords[i]);
							}
						}
					}
				}
			}
			list.Add("innlevering");
			list.Add("eksamen");
			list.Add("forelesning");
			return list;
		}

		/// <summary>
		///     Only include 0-9, A-Å, space, comma, and ._+#: from string
		/// </summary>
		public string FilterCharacters(string str)
		{
			var sb = new StringBuilder();
			foreach (var c in str)
			{
				if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')
				    || c == ',' || c == ' ' || c == '.' || c == '_' || c == '+' || c == '#' || c == ':'
				    || c == 'æ' || c == 'ø' || c == 'å' || c == 'Æ' || c == 'Ø' || c == 'Å')
				{
					sb.Append(c);
				}
			}
			return sb.ToString();
		}
	}
}