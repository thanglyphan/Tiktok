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
			AllEvents = allEvents;
		}

		public List<CustomEventMonth> GetEventsWithUser(StudentUser user)
		{
			List<CustomEventMonth> months = GetInitializedEventMonthList();
			foreach (var evnt in AllEvents)
			{
				if (evnt.Courses.Contains(user.Course))
				{
					months[evnt.StartDateTime.Month - 1].AddEvent(evnt);
					Printer.Print("E: " + evnt.Subject.Name);
				}
			}
			return months;
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
			for (int i = 0; i < 12; i++)
			{
				months.Add(new CustomEventMonth(i+1));
				DateTime today = DateTime.Today;
				DateTime date = new DateTime(today.Year, i+1, 1);
				//DateTime date = DateTime.Today;
				// first generate all dates in the month of 'date'
				var dates = Enumerable.Range(1, DateTime.DaysInMonth(date.Year, date.Month)).Select(n => new DateTime(date.Year, date.Month, n));
				// then filter the only the start of weeks
				var weekends = from d in dates where d.DayOfWeek == DayOfWeek.Monday select d;
				foreach (var weeks in weekends)
				{
					months[i].Weeks.Add(new CustomEventWeek(weeks.GetWeekNumberOfYear()));
				}
			}
			return months;
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
					if (subj.Code == code)
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
					if (cs.Subject.ID == subject.ID)
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
	}
}