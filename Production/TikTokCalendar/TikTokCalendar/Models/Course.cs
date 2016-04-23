using System;
using System.Diagnostics;
using TikTokCalendar.Extras;

namespace TikTokCalendar.Models
{
	public class Course
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public SchoolCourses SchoolCourse { get; set; }

		public void SetAndParse(string id, string name)
		{
			// Parse id
			ID = Utils.ParsePositiveInt(id);

			// Name
			Name = name;

			// Parse the name to a course enum
			// TODO Something is fucky here
			//int bestMatch = 1000;
			var course = SchoolCourses.Programmering;
			//string[] courses = Enum.GetNames(typeof(SchoolCourses));
			//for (int i = 1; i < courses.Length + 1; i ++)
			//{
			//	int match = Math.Abs(name.CompareTo(courses[i - 1]));
			//	if (match <= bestMatch)
			//	{
			//		course = (SchoolCourses)i;
			//		bestMatch = match;
			//	}
			//}
			// Replace spaces and dashes so it matches the enum
			// TODO User RegEx
			var safeName = name.Replace("-", "");
			safeName = safeName.Replace(" ", "");
			Enum.TryParse(safeName, out course);

			SchoolCourse = course;
		}

		public static SchoolCourses GetCourseFromName(string name)
		{
			var course = SchoolCourses.Programmering;
			if (!Enum.TryParse(name, out course))
			{
				Debug.WriteLine("WARNING: ERROR: Couldn't parse course enum '" + name + "'!");
			}
			//int bestMatch = 1000;
			//string[] courses = Enum.GetNames(typeof(SchoolCourses));
			//for (int i = 1; i < courses.Length + 1; i++)
			//{
			//	int match = Math.Abs(name.CompareTo(courses[i - 1]));
			//	if (match <= bestMatch)
			//	{
			//		course = (SchoolCourses)i;
			//		bestMatch = match;
			//	}
			//}
			return course;
		}

		//}
		//	return a.ID == b.ID;
		//	if ((object)a == null || (object)b == null) return false;
		//{

		//public static bool operator ==(Course a, Course b)

		//public static bool operator !=(Course a, Course b)
		//{
		//	return !(a == b);
		//}
	}
}