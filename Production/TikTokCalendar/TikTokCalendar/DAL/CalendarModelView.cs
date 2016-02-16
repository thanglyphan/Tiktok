﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TikTokCalendar.Models;

namespace TikTokCalendar.DAL
{
	public class CalendarModelView
	{
		public IEnumerable<CalendarEvent> CalendarEvents { get; set; }
		public Account Account; 
	}
}