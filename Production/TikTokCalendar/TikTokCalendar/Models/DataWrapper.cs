using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikTokCalendar.DAL;
using TikTokCalendar.Extras;

namespace TikTokCalendar.Models
{
	// Collection of the .json data
	public class DataWrapper
	{
		// TODO Possibly put these in dictionaries for fast lookup????
		public List<Subject> Subjects { get; private set; }
		public List<Course> Courses { get; private set; }
		public List<CourseSubject> CourseSubjects { get; private set; }
		public List<CustomEvent> AllEvents { get; private set; }

		public void Initialize(List<Subject> subjs, List<Course> courses, List<CourseSubject> courseSubjs)
		{
			Subjects = subjs;
			Courses = courses;
			CourseSubjects = courseSubjs;
		}

		public void SetSchoolSystemDependantData(List<CustomEvent> allEvents)
		{
			AllEvents = allEvents.OrderBy(x => x.StartDateTime).ToList();
			//AllEvents.OrderBy(x => x.StartDateTime);
			//OrderBy(x => x.StartTime).ToList();
		}

		public List<CustomEventMonth> GetEventsWithUser(StudentUser user)
		{
			List<CustomEventMonth> months = new List<CustomEventMonth>();
			CustomEventMonth month = null;
			CustomEventWeek week = null;
			HashSet<int> addedIDs = new HashSet<int>();

			foreach (var evnt in AllEvents)
			{
				if (!addedIDs.Contains(evnt.ID) && (user.Course == SchoolCourses.VisAlt 
					|| (evnt.Courses.Contains(user.Course) && evnt.ClassYear == user.ClassYear)))
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

		public List<CustomEventMonth> GetEventsWithName(StudentUser user, string tags)
		{
			List<CustomEventMonth> months = new List<CustomEventMonth>();
			CustomEventMonth month = null;
			CustomEventWeek week = null;

			foreach (var evnt in AllEvents)
			{
				if (user.Course == SchoolCourses.VisAlt
					|| (evnt.Courses.Contains(user.Course) && evnt.ClassYear == user.ClassYear))
				{
					string temp = "";
					if (!string.IsNullOrEmpty(tags))
					{
						temp = tags.ToLower();
						string eventname = evnt.Subject.Name.ToLower();
						string roomname = "";
						if (evnt.RoomName != null)
						{
							evnt.RoomName.ToLower();
						}
						string teacher = evnt.Teacher.ToLower();
						string comment = evnt.Comment.ToLower();
						string subjectcode = evnt.Subject.Code.ToLower();
						string eventtype = evnt.eventType.ToString().ToLower();
						if (!(eventname.Contains(temp) || roomname.Contains(temp) || teacher.Contains(temp) || comment.Contains(temp) || subjectcode.Contains(temp) || eventtype.Contains(temp)))
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
				months.Add(new CustomEventMonth(i+1));
				DateTime today = DateTime.Today;
				DateTime date = new DateTime(today.Year, i+1, 1);
				//DateTime date = DateTime.Today;
				// first generate all dates in the month of 'date'

				// TODO Here correctly get the start of each week
				//var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));
				//// then filter the only the start of weeks
				//var weekends = from d in dates where d.DayOfWeek == DayOfWeek.Monday select d;

				int numOfWeeks = MondaysInMonth(date);
				//foreach (var weeks in weekends)
				for(int j = 0; j < numOfWeeks+1; j ++)
				{
					months[i].Weeks.Add(new CustomEventWeek(weekNr, j+1));//weeks.GetWeekNumberOfYear()));
					weekNr ++;
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
					Printer.Print(string.Format("{0} == {1} = {2}", subj.Code, code, e));
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
				if (name.Contains(course.Name))
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

		// Singleton stuff
		private DataWrapper() { }
		static DataWrapper() { }
		private static readonly DataWrapper _instance = new DataWrapper();
		public static DataWrapper Instance { get { return _instance; } }

        /// <summary>
		/// Gets all keywords from AllEvents (user specific, mainly for the autocomplete)
		/// </summary>
        public List<string> GetUserKeywords(StudentUser user)
        {
            List<string> list = new List<string>();
            foreach (var evnt in AllEvents)
            {
                if (evnt.Courses.Contains(user.Course) && evnt.ClassYear == user.ClassYear)
                {
                    if (!list.Contains(evnt.Subject.Name))
                    {
                        list.Add(evnt.Subject.Name);
                    }
                    if (!list.Contains(evnt.Subject.Code))
                    {
                        list.Add(evnt.Subject.Code);
                    }
                    if (!list.Contains(evnt.RoomName))
                    {
                        list.Add(evnt.RoomName);
                    }
                    if (!list.Contains(evnt.Teacher))
                    {
                        list.Add(evnt.Teacher);
                    }
                }
            }
            return list;
        }
    }
}