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
			var course = SchoolCourses.Programmering;
			
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
			return course;
		}
	}
}