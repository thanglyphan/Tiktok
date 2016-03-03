using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TikTokCalendar.Extras;

namespace TikTokCalendar.Models
{
	public class Subject
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public virtual ICollection<CalendarEvent> CalendarEvents { get; set; }

		public string Code { get; set; }

		public void SetAndParse(string id, string name)
		{
			ID = Utils.ParsePositiveInt(id);

			Name = name.Substring(0, name.Length - 11);
			Code = GetSubjectCode(name);
		}

		public string GetSubjectCode(string name)
		{
			string code = name.Substring(name.Length - 11);
			return code.Substring(1, code.Length - 2);
		}

		public string GetSubjectCode()
		{
			return GetSubjectCode(Name);
		}
	}
}