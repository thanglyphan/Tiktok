using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace TikTokCalendar.Extras
{
	public class DateTimeParser
	{
		private const string dM_Format = "d.M.";// yyyy HH:mm:ss"; // Format for parsing date from eksamen/innlevering data
		private const string ddMMyyyy_Format = "dd.MM.yyyy HH:mm:ss"; // Format for parsing date from eksamen/innlevering data

		/// <summary>
		/// Parses the date and time specified (to the format dd.MM.yyyy HH:mm:ss). Results gives Single on succes, NoDate on fail.
		/// Checks if the two strings are null.
		/// </summary>
		public DateTime SimpleParse(string date, string time, out DateParseResults results)
		{
			results = DateParseResults.NoDate;
			DateTime dt;
			if (date != null && time != null && DateTime.TryParseExact($"{date} {time}:00", ddMMyyyy_Format, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
			{
				results = DateParseResults.Single;
				return dt;
			}
			return DateTime.MinValue;
		}

		/// <summary>
		/// Parses dates from a string. Results is set to Single if only one date was found, Two if two dates, Week if a week was found ("Uke xx"), NoDate if no date was found.
		/// Returns a list of dates. The list is empty if no dates were found. If a week was found, the first day of that week is returned.
		/// </summary>
		/// <param name="date">The datestring to parse.</param>
		/// <param name="results">The results of the parsing.</param>
		/// <returns></returns>
		public DateTime[] ParseDate(string date, out DateParseResults results)
		{
			results = DateParseResults.NoDate;
			List<DateTime> dates = new List<DateTime>();

			string pattern = "(\\d)?\\d\\.(\\d)?\\d\\."; // Regex pattern of the dates
			Match match = Regex.Match(date, pattern); // Find first match

			if (match.Success)
			{
				// Since we found match, try to parse it to a date and add it to the dates list
				DateTime dt1;
				if (TryParse(match.Value, out dt1))
				{
					results = DateParseResults.Single;
					dates.Add(dt1);
				}

				// See if there is another match and if the datetext contains "og" (which means there are two dates)
				Match nextMatch = match.NextMatch();
				if (date.ToLower().Contains("og") && nextMatch.Success)
				{
					// Also parse this date and add it to the list
					DateTime dt2;
					if (TryParse(nextMatch.Value, out dt2))
					{
						results = DateParseResults.Two;
						dates.Add(dt2);
					}
				}
			}
			else
			{
				// Since we couldn't find a match, see if the datestring has "uke", which means it specifies a week
				if (date.ToLower().Contains("uke"))
				{
					// Try to parse the weeknumber to a date (the first day of that week) and add it to the dates list
					DateTime dt;
					if (TryParseWeekToFirstDate(date, out dt))
					{
						results = DateParseResults.Week;
						dates.Add(dt);
					}
					else
					{
						results = DateParseResults.NoDate;
					}
				}
				else
				{
					results = DateParseResults.NoDate;
				}
			}
			return dates.ToArray();
		}

		private bool TryParse(string s, out DateTime d)
		{
			d = DateTime.MinValue;
			DateTime dt;
			if (DateTime.TryParseExact(s, dM_Format, CultureInfo.GetCultureInfoByIetfLanguageTag("nb-NO"), DateTimeStyles.None, out dt))
			{
				Console.WriteLine(" Parsed [" + s + "] to: {" + dt + "}");
				d = dt;
				return true;
			}
			return false;
		}

		/// <summary>
		/// Tries to parse the first date of the week if the string contains a week.
		/// </summary>
		/// <param name="s">String to parse week from.</param>
		/// <param name="dt">DateTime of the first day of the week</param>
		/// <returns></returns>
		private bool TryParseWeekToFirstDate(string s, out DateTime dt)
		{
			dt = DateTime.MinValue;
			Match match = Regex.Match(s, "\\d\\d?");
			if (match.Success)
			{
				int week = -1;
				int.TryParse(match.Value, out week);

				if (week >= 1 && week <= 53)
				{
					DateTime firstDate = FirstDateOfWeekISO8601(DateTime.Now.Year, week);
					dt = firstDate;
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Borrowed from: http://stackoverflow.com/a/9064954/5853590
		/// </summary>
		private DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
		{
			DateTime jan1 = new DateTime(year, 1, 1);
			int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

			DateTime firstThursday = jan1.AddDays(daysOffset);
			var cal = CultureInfo.CurrentCulture.Calendar;
			int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

			var weekNum = weekOfYear - 1;
			if (firstWeek <= 1)
			{
				weekNum -= 1;
			}
			var result = firstThursday.AddDays(weekNum * 7);
			return result.AddDays(-3).AddHours(-5);
		}

		/// <summary>
		/// Returns a weeknumber from a given datetime.
		/// </summary>
		/// <param name="dt"></param>
		/// <returns></returns>
		public int GetWeekNumber(DateTime dt)
		{
			Calendar cal = new GregorianCalendar();
			return cal.GetWeekOfYear(dt, CalendarWeekRule.FirstDay, DayOfWeek.Monday);
		}
	}

	/// <summary>
	/// The results of a Date parse
	/// </summary>
	public enum DateParseResults
	{
		/// <summary>
		/// A single date.
		/// </summary>
		Single,
		/// <summary>
		/// Two dates.
		/// </summary>
		Two,
		/// <summary>
		/// A week.
		/// </summary>
		Week,
		/// <summary>
		/// No date-
		/// </summary>
		NoDate
	}
}