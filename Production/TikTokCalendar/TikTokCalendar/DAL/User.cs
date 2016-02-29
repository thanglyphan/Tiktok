using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

// Class for a user
namespace TikTokCalendar.DAL
{
	public class User
	{
		public string UserName { get; set; }
		public SchoolCourses Course { get; set; }

		public User(string name, SchoolCourses course)
		{
			UserName = name;
			Course = course;
		}

		public bool ValidUsername(string name)
		{
			return ParseYear(name) != -1;
		}

		/// <summary>
		/// Returns the year based on the username.
		/// </summary>
		public int GetYearFromName()
		{
			// TODO Possibly return DateTime instead of int?
			return ParseYear(UserName);
		}

		private int ParseYear(string name)
		{
			string num = name.Substring(name.Length - 2);
			int year = 0;
			if (int.TryParse(num, NumberStyles.Integer, new NumberFormatInfo(), out year))
			{
				return year;
			}
			return -1;
		}
	}
}