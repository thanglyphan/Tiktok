using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
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
			SchoolCourses course = SchoolCourses.Programmering;
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
			string safeName = name.Replace("-", "");
			safeName = safeName.Replace(" ", "");
			Enum.TryParse(safeName, out course);

			SchoolCourse = course;
		}

		public static SchoolCourses GetCourseFromName(string name)
		{
			int bestMatch = 1000;
			SchoolCourses course = SchoolCourses.Programmering;
			string[] courses = Enum.GetNames(typeof(SchoolCourses));
			for (int i = 1; i < courses.Length + 1; i++)
			{
				int match = Math.Abs(name.CompareTo(courses[i - 1]));
				if (match <= bestMatch)
				{
					course = (SchoolCourses)i;
					bestMatch = match;
				}
			}
			return course;
		}

		//public static bool operator ==(Course a, Course b)
		//{
		//	if ((object)a == null || (object)b == null) return false;
		//	return a.ID == b.ID;
		//}

		//public static bool operator !=(Course a, Course b)
		//{
		//	return !(a == b);
		//}
	}
}