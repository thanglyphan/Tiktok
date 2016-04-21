using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using TikTokCalendar.DAL;
using TikTokCalendar.Extras;

namespace TikTokCalendar.Models
{
    // Collection of the .json data
    public class DataWrapper
    {
		// Singleton stuff
		private DataWrapper() { }
		static DataWrapper() { }
		private static readonly DataWrapper _instance = new DataWrapper();
		public static DataWrapper Instance { get { return _instance; } }

		// TODO Possibly put these in dictionaries for fast lookup????
		public List<Subject> Subjects { get; private set; }
        public List<Course> Courses { get; private set; }
        public List<CourseSubject> CourseSubjects { get; private set; }
        public List<CustomEvent> AllEvents { get; private set; }
        public List<StudentUser> Users { get; private set; }
		public Dictionary<string, Room> Rooms { get; private set; }

		public void Initialize(List<Subject> subjs, List<Course> courses, List<CourseSubject> courseSubjs, List<StudentUser> users)
        {
            Subjects = subjs;
            Courses = courses;
            CourseSubjects = courseSubjs;
			Users = users;
        }

        public void SetSchoolSystemDependantData(List<CustomEvent> allEvents, Dictionary<string, Room> rooms)
        {
            AllEvents = allEvents.OrderBy(x => x.StartDateTime).ToList();
            //AllEvents.OrderBy(x => x.StartDateTime);
            //OrderBy(x => x.StartTime).ToList();
			Rooms = rooms;
		}

		public bool IsValidUser(StudentUser user)
		{
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
			// TODO User predicate
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
            List<CustomEventMonth> months = new List<CustomEventMonth>();
            CustomEventMonth month = null;
            CustomEventWeek week = null;
            HashSet<long> addedIDs = new HashSet<long>();

            foreach (var evnt in AllEvents)
            {
                if (!addedIDs.Contains(evnt.ID) && (user.Course == SchoolCourses.VisAlt
                    || (evnt.Courses.Contains(user.Course) && evnt.IsYear(user.ClassYear))))
                {
                    CustomEventMonth m = AddEvent(evnt, ref month, ref week);
                    if (m != null)
                    {
                        months.Add(m);
                        addedIDs.Add(evnt.ID);
                    }
                }
            }
            return months;
        }

	    public List<Room> GetRooms()
	    {
			/*
			Rooms = new List<Room>();
	        Room r1 = new Room("Rom 82");
	        Room r2 = new Room("Rom 83");
	        Room r3 = new Room("Rom 41");

			DateTime today = DateTime.Today;
	        r1.TryAddTimeSlot(t1);
			*/
			return new List<Room>();
	    }

		public List<CustomEventMonth> GetEventsWithName(StudentUser user)
		{
			List<CustomEventMonth> months = new List<CustomEventMonth>();
			CustomEventMonth month = null;
			CustomEventWeek week = null;

			string temp = "";

			foreach (var evnt in AllEvents)
			{
				if (user.Course == SchoolCourses.VisAlt
					|| (evnt.Courses.Contains(user.Course) && evnt.IsYear(user.ClassYear)))
				{
					CustomEventMonth m = AddEvent(evnt, ref month, ref week);
					if (m != null)
					{
						months.Add(m);
					}
				}
			}
			return months;
		}

		public List<CustomEventMonth> GetEventsWithName(StudentUser user, string tags, bool lecture, bool assignment, bool exam)
		{
			List<CustomEventMonth> months = new List<CustomEventMonth>();
			CustomEventMonth month = null;
			CustomEventWeek week = null;

			string temp = "";

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
							temp = FilterCharacters(tags).ToLower();
							string[] array = temp.Split(' ');

							string eventname = "";
							string roomname = "";
							string teacher = "";
							string comment = "";
							string subjectcode = "";
							string eventtype = "";
							string daynum = "";
							string dayname = "";
							string weeknum = "";
							string monthname = "";

							try { eventname = evnt.Subject.Name.ToLower(); } catch (NullReferenceException) { }
							try { roomname = evnt.RoomName.ToLower(); } catch (NullReferenceException) { }
							try { teacher = evnt.Teacher.ToLower(); } catch (NullReferenceException) { }
							try { comment = evnt.Comment.ToLower(); } catch (NullReferenceException) { }
							try { subjectcode = evnt.Subject.Code.ToLower(); } catch (NullReferenceException) { }
							try { eventtype = evnt.eventType.ToString().ToLower(); } catch (NullReferenceException) { }
							try { daynum = evnt.StartDateTime.Day.ToString(); } catch (NullReferenceException) { }
							try { dayname = evnt.GetDayOfWeek().ToLower(); } catch (NullReferenceException) { }
							try { weeknum = evnt.StartDateTime.GetWeekNumberOfYear().ToString(); } catch (NullReferenceException) { }
							try { monthname = evnt.GetMonthName().ToLower(); } catch (NullReferenceException) { }

							bool flagged = false;
							for (int i = 0; i < array.Length; i++)
							{
								if (!(eventname.Contains(array[i]) || roomname.Contains(array[i]) || teacher.Contains(array[i])
									|| comment.Contains(array[i]) || subjectcode.Contains(array[i]) || eventtype.Contains(array[i])
									|| daynum.Contains(array[i]) || dayname.Contains(array[i]) || weeknum.Contains(array[i]) || monthname.Contains(array[i])))
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

						CustomEventMonth m = AddEvent(evnt, ref month, ref week);
						if (m != null)
						{
							months.Add(m);
						}
					}
					else
					{
						continue;
					}
				}
			}
			return months;
		}

		private CustomEventMonth AddEvent(CustomEvent evnt, ref CustomEventMonth month, ref CustomEventWeek week)
        {
            CustomEventMonth retMonth = null;

            DateTime curDate = evnt.StartDateTime;
            bool newMonth = false;
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
                int weekNr = curDate.GetWeekNumberOfYear();
                week = new CustomEventWeek(weekNr, 1);

                month.Weeks.Add(week);
            }
            week.events.Add(evnt);
            return retMonth;
        }

        public List<CustomEventMonth> GetEventsWithSubject(Subject subject)
        {
            List<CustomEventMonth> months = GetInitializedEventMonthList();
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
            List<CustomEventMonth> months = new List<CustomEventMonth>();
            int weekNr = 1;
            for (int i = 0; i < 12; i++)
            {
                months.Add(new CustomEventMonth(i + 1));
                DateTime today = DateTime.Today;
                DateTime date = new DateTime(today.Year, i + 1, 1);
                //DateTime date = DateTime.Today;
                // first generate all dates in the month of 'date'

                // TODO Here correctly get the start of each week
                //var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));
                //// then filter the only the start of weeks
                //var weekends = from d in dates where d.DayOfWeek == DayOfWeek.Monday select d;

                int numOfWeeks = MondaysInMonth(date);
                //foreach (var weeks in weekends)
                for (int j = 0; j < numOfWeeks + 1; j++)
                {
                    months[i].Weeks.Add(new CustomEventWeek(weekNr, j + 1));//weeks.GetWeekNumberOfYear()));
                    weekNr++;
                }
            }
            return months;
        }

        private int MondaysInMonth(DateTime thisMonth)
        {
            int mondays = 0;
            int month = thisMonth.Month;
            int year = thisMonth.Year;
            int daysThisMonth = DateTime.DaysInMonth(year, month);
            DateTime beginingOfThisMonth = new DateTime(year, month, 1);
            for (int i = 0; i < daysThisMonth; i++)
                if (beginingOfThisMonth.AddDays(i).DayOfWeek == DayOfWeek.Monday)
                    mondays++;
            return mondays;
        }

        /// <summary>
        /// Tries to find a Subject with the code specified. Returns null if it couldn't find a mathcing Subject.
        /// </summary>
        public Subject GetSubjectByCode(string code)
        {
            Subject subject = null;
            if (code != null)
            {
                foreach (var subj in Subjects)
                {
                    bool e = subj.Code.Equals(code, StringComparison.OrdinalIgnoreCase);
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
        /// Tries to find a Course with a Name that is in the specified name. Returns null if it couldn't find a mathcing Course.
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
            List<CourseSubject> retList = new List<CourseSubject>();
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
        /// Gets all keywords from AllEvents (user specific, mainly for the autocomplete)
        /// </summary>
        public List<string> GetUserKeywords(StudentUser user)
        {
            List<string> list = new List<string>();
            foreach (var evnt in AllEvents)
            {
                if ((evnt.Courses.Contains(user.Course) && evnt.IsYear(user.ClassYear)) || user.Course == SchoolCourses.VisAlt)
                {
                    string eventname = null;
                    string roomname = null;
                    string teacher = null;
                    string subjectcode = null;
                    string eventtype = null;
                    string monthname = null;

                    try { eventname = evnt.Subject.Name.ToLower();
                        if (!list.Contains(eventname))
                        {
                            list.Add(eventname);
                        }
                    } catch (NullReferenceException) { }
                    try { roomname = evnt.RoomName.ToLower();
                        if (!list.Contains(roomname))
                        {
                            list.Add(roomname);
                        }
                    } catch (NullReferenceException) { }
                    try { teacher = evnt.Teacher.ToLower();
                        if (!list.Contains(teacher))
                        {
                            list.Add(teacher);
                        }
                    } catch (NullReferenceException) { }
                    try { subjectcode = evnt.Subject.Code.ToLower();
                        if (!list.Contains(subjectcode))
                        {
                            list.Add(subjectcode);
                        }
                    } catch (NullReferenceException) { }
                    try { eventtype = evnt.eventType.ToString().ToLower();
                        if (!list.Contains(eventtype))
                        {
                            list.Add(eventtype);
                        }
                    } catch (NullReferenceException) { }
                    try { monthname = evnt.GetMonthName().ToLower();
                        if (!list.Contains(monthname))
                        {
                            list.Add(monthname);
                        }
                    } catch (NullReferenceException) { }
                }
            }
            list.Add("Innlevering");
            list.Add("Eksamen");
            list.Add("Forelesning");
            return list;
        }

        /// <summary>
        /// Only include 0-9, A-Å, space, comma, and ._+#: from string
        /// </summary>
        public string FilterCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
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