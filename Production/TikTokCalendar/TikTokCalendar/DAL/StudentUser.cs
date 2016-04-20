using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;

// Class for a user
namespace TikTokCalendar.DAL
{
	public class StudentUser
	{
		public string UserName { get; private set; }
		public SchoolCourses Course
		{
			get
			{
				if (ClassYear == 1)
				{
					return SchoolCourses.BacheloriIT;
				}
				return _course;
			}
			private set
			{
				_course = value;
			}
		}
		private SchoolCourses _course = SchoolCourses.VisAlt;
		public int ClassYear { set; get; }
		public string Password { get; set; } // TODO this is not really secure...
		public string Email { get; set; }

		public StudentUser(string name, SchoolCourses course, string yearString)
		{
			UserName = name;
			ClassYear = ParseYear(yearString);
			Course = course;
		}

		public StudentUser(string name, string password, string email, int classYear, SchoolCourses course)
		{
			UserName = name;
			Password = password;
			Email = email;
			ClassYear = classYear;
			Course = course;
		}

		private int ParseYear(string yearString)
		{
			switch (yearString)
			{
				case "first":
					return 1;
				case "second":
					return 2;
				case "third":
					return 3;
				default:
					Debug.WriteLine("WARNING: User [" + UserName + "] has invalid year (" + yearString + ")");
					return -1;
			}
		}
	}
}