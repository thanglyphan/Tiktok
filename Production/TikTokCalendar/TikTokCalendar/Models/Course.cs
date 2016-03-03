﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikTokCalendar.Models
{
	public class Course
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public SchoolCourses SchoolCourse;

		public Course(int id, string name)
		{
			SchoolCourse = (SchoolCourses)id;
			Name = name;
		}
	}
}