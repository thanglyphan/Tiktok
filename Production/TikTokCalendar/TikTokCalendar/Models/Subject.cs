using System.Collections.Generic;
using TikTokCalendar.Extras;

namespace TikTokCalendar.Models
{
	public class Subject
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public virtual ICollection<CalendarEvent> CalendarEvents { get; set; }

		public string Code { get; set; }

		public void SetAndParse(string id, string name, string code)
		{
			ID = Utils.ParsePositiveInt(id);

			Name = name; // name.Substring(0, name.Length - 11);
			Code = code; // GetSubjectCode(name);
		}

		public static string GetSubjectCode(string name)
		{
			if (name.Length < 11)
			{
				return null;
			}
			var code = name.Substring(name.Length - 11);
			return code.Substring(1, code.Length - 5);
		}

		public string GetSubjectCode()
		{
			return GetSubjectCode(Name);
		}

		//{

		//public static bool operator!=(Subject a, Subject b)
		//}
		//	return a.ID == b.ID;
		//	if (a == null || b == null) return false;
		//{

		//public static bool operator==(Subject a, Subject b)
		//	return !(a == b);
		//}
	}
}