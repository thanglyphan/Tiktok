using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class TimeSlot
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
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
	}
}