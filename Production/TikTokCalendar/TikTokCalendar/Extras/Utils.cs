using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace TikTokCalendar.Extras
{
	public static class Utils
	{
		/// <summary>
		/// Parses a string to an integer. If the parsing failed or the parsed int is negative, -1 is returned, otherwise the parsed int is returned.
		/// </summary>
		/// <param name="s">string to parse.</param>
		/// <returns>If the parsing failed or the parsed int is negative, -1 is returned, otherwise the parsed int is returned.</returns>
		public static int ParsePositiveInt(string s)
		{
			int parsed = -1;
			if (int.TryParse(s, NumberStyles.Integer, new NumberFormatInfo(), out parsed) && parsed > 0)
			{
				return parsed;
			}
			return -1;
		}
	}
}