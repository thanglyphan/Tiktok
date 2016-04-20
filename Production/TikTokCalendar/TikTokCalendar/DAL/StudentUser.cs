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
		public SchoolCourses Course {
			get {
				if (ClassYear == 1) {
					return SchoolCourses.BacheloriIT;
				}
				return _course;
			}
			private set {
				_course = value;
			}
		}
		private SchoolCourses _course = SchoolCourses.VisAlt;
		//public int Year { get; set; }
		//private int WeekOrMonthShow { get; set; }
		//public int ClassYear { get { return DateTime.Now.Year - Year; } }
		public int ClassYear { set; get; }

		public StudentUser(string name, SchoolCourses course, string yearString)
		{
			UserName = name;
			//Year = GetYearFromName();
			switch (yearString) {
				case "first":
					ClassYear = 1;
					break;
				case "second":
					ClassYear = 2;
					break;
				case "third":
					ClassYear = 3;
					break;
				default:
					ClassYear = -1;
					Debug.WriteLine("Invalid year");
					break;
			}
			Course = course;
			/*
			int y = 0;
			if (int.TryParse(yearString, NumberStyles.Integer, new NumberFormatInfo(), out y))
			{
				ClassYear = y;
			}
			else
			{
				Debug.WriteLine("Error parsing year, couldn't parse '" + yearString + "' to an int!");
			}
			// TODO Check if username is valid, if it isn't make the user a special user that only have one event called EROOR or something
	*/
		}

		/// <summary>
		/// Gets the current semester based on the year of this user.
		/// </summary>
		//public int GetCurrentSemester()
		//{
		//	if (Year > 0)
		//	{
		//		DateTime now = DateTime.Today;
		//		int curYear = now.Year - Year;
		//		int partOfYear = (now.Month < 8) ? 0 : 1; // 0 if before august, 1 after august
		//		int semester = (curYear*2) - partOfYear; // Should give the semester (eg. 1*2-1 = 1 [year1 * 2 - 1 = semester1])
		//		return semester;
		//	}

		//	// HACK: Make it overflow so we get an error if we somehow cant get the current semester calculated
		//	// Futureme: handle this better
		//	int overflow = 10;
		//	return int.MaxValue + overflow;
		//}

		//public bool ValidUsername(string name)
		//{
		//	return ParseYear(name) != -1;
		//}

		/// <summary>
		/// Returns the year based on the username.
		/// </summary>
		//public int GetYearFromName()
		//{
		//	// TODO Possibly return DateTime instead of int?
		//	return ParseYear(UserName);
		//}

		//private int ParseYear(string name)
		//{
		//	if (name.Length > 2)
		//	{
		//		string num = name.Substring(name.Length - 2);
		//		// Splice the year at the end of the username to the first to letters of the current year (eg. "20" in "2016" and "name14" becomes "2014")
		//		// TODO Find a better way to do this
		//		num = DateTime.Today.Year.ToString().Substring(0, 2) + num;

		//		int year = 0;
		//		if (int.TryParse(num, NumberStyles.Integer, new NumberFormatInfo(), out year))
		//		{
		//			return year;

		//		}
		//	}
		//	return -1;
		//}

		//public int update()
		//{
		//	Printer.Print("hey");
		//	return 0;
		//}

		//public void SaveToCookies()
		//{

		//}

		//public void LoadFromCookies()
		//{

		//}
	}
}