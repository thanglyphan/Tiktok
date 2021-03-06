﻿using System.Diagnostics;

// Class for a user

namespace TikTokCalendar.DAL
{
	public class StudentUser
	{
		private SchoolCourses _course = SchoolCourses.VisAlt;

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

		public string UserName { get; }

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
			private set { _course = value; }
		}

		public int ClassYear { set; get; }
		public string Password { get; set; } // TODO this is not really secure...
		public string Email { get; set; }

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

		public string GetYearAsText()
		{
			switch (ClassYear)
			{
				case 1:
					return "first";
				case 2:
					return "second";
				case 3:
					return "third";
				default:
					return "NaN";
			}
		}

		public bool IsValid(StudentUser other)
		{
			if (other.UserName == UserName && other.Password == Password)
			{
				return true;
			}
			return false;
		}
	}
}