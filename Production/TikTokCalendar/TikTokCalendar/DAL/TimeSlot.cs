using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikTokCalendar.DAL
{
	public class TimeSlot
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }

		public TimeSlot(DateTime start, DateTime end)
		{
			Start = start;
			End = end;
		}
	}
}