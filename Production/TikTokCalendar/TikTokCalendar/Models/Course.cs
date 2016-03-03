﻿using System;
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
		public SchoolCourses SchoolCourse;

		public void SetAndParse(string id, string name)
		{
			// Parse id
			ID = Utils.ParsePositiveInt(id);

			// Name
			Name = name;

			// Parse the name to a course enum
			int bestMatch = 1000;
			SchoolCourses course = SchoolCourses.Programmering;
			string[] courses = Enum.GetNames(typeof(SchoolCourses));
			for (int i = 1; i < courses.Length + 1; i ++)
			{
				int match = Math.Abs(name.CompareTo(courses[i - 1]));
				if (match <= bestMatch)
				{
					course = (SchoolCourses)i;
					bestMatch = match;
				}
			}
			SchoolCourse = course;
			Debug.WriteLine(name + " is course: " + course);
		}
	}
}