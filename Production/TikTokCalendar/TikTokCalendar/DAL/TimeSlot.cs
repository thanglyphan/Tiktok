using System;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class TimeSlot
	{
		public const int StartHour = 8;
		public const int EndHour = 18;

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

		public DateTime Start { get; }
		public DateTime End { get; }

		public DateTime Min { get; private set; }
		public DateTime Max { get; private set; }
		public long FullDaySpan { get; private set; }

		public CustomEvent Event { get; private set; }

		public float GetPercentOfDay()
		{
			Min = MinTime(Start);
			Max = MaxTime(End);

			// Calulate the span between the min/max
			var span = End.Ticks - Start.Ticks;
			var fullDaySpan = Max.Ticks - Min.Ticks;

			float slotSpan = span;
			float daySpan = fullDaySpan;
			var percent = slotSpan/daySpan*100;

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