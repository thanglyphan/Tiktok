using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikTokCalendar.Models
{
	public class Subject
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public virtual ICollection<CalendarEvent> CalendarEvents { get; set; }

		public string GetSubjectCode()
		{
			string code = Name.Substring(Name.Length - 11);
			return code.Substring(1, code.Length - 2);
		}
	}
}