using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class TimeSlot
	{
		public const int StartHour = 7;
		public const int EndHour = 19;

		public DateTime Start { get; private set; }
		public DateTime End { get; private set; }

		public DateTime Min { get; private set; }
		public DateTime Max { get; private set; }
		public long FullDaySpan { get; private set; }

		public CustomEvent Event { get; private set; }

		public TimeSlot(CustomEvent evnt)
		{
			Event = evnt;
			Start = evnt.StartDateTime;
			End = evnt.EndDateTime;
		}

		public TimeSlot(DateTime start, DateTime end)
		{
			Start = start;
			End = end;
		}

		public long GetPercentOfDay()
		{
			Min = MinTime(Start);
			Max = MaxTime(End);

			// Calulate the span between the min/max
			long span = End.Ticks - Start.Ticks;
			long fullDaySpan = Max.Ticks - Min.Ticks;

			float slotSpan = (float)span;
			float daySpan = (float)fullDaySpan;
			long percent = (long)((slotSpan / daySpan) * 100);

			return percent;
		}

		public static DateTime MinTime(DateTime date)
		{
			return new DateTime(date.Year, date.Month, date.Day, StartHour, 0, 0);
		}

		public static DateTime MaxTime(DateTime date)
		{
			return new DateTime(date.Year, date.Month, date.Day, EndHour, 0, 0);
		}
	}
}