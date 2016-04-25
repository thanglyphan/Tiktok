using System;
using System.Globalization;
using System.Linq;
using TikTokCalendar.Controllers;

namespace TikTokCalendar.Extras
{
	public static class Utils
	{
		/// <summary>
		///     Parses a string to an integer. If the parsing failed or the parsed int is negative, -1 is returned, otherwise the
		///     parsed int is returned.
		/// </summary>
		/// <param name="s">string to parse.</param>
		/// <returns>If the parsing failed or the parsed int is negative, -1 is returned, otherwise the parsed int is returned.</returns>
		public static int ParsePositiveInt(string s)
		{
			var parsed = -1;
			if (int.TryParse(s, NumberStyles.Integer, new NumberFormatInfo(), out parsed) && parsed > 0)
			{
				return parsed;
			}
			return -1;
		}

		// From: http://stackoverflow.com/a/444818
		/// <summary>
		///     Checks if the toCheck string is found in source.
		/// </summary>
		public static bool Contains(this string source, string toCheck, StringComparison comp)
		{
			if (source == null || toCheck == null)
			{
				return false;
			}
			return source.IndexOf(toCheck, comp) >= 0;
		}


		public static int GetWeekNumberOfYear(this DateTime dt)
		{
			return CultureManager.CurrentCulture.Calendar.GetWeekOfYear(dt, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
		}

		public static string FirstCharToUpper(this string input)
		{
			if (string.IsNullOrEmpty(input))
				return null;
			return input.First().ToString().ToUpper() + input.Substring(1);
		}

		public static string ToYearMonthDay(this DateTime dt)
		{
			return string.Format("{0}{1}{2}", dt.Year, dt.Month, dt.Day);
		}
	}
}