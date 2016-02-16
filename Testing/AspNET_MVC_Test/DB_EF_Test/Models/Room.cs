using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DB_EF_Test.Models
{
	public class Room
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }

		public virtual ICollection<CalendarEvent> CalendarEvents { get; set; }
	}
}