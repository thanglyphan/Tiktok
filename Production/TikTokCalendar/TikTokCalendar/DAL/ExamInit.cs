using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TikTokCalendar.DAL
{
	public class ExamInit
	{
		CalendarEventContext context;
		public ExamInit(CalendarEventContext context)
		{
			this.context = context;
			ReadJsonFile();
		}

		public void ReadJsonFile()
		{
			var dataPath = "~/Content/NAMEHERE.json";
			dataPath = HttpContext.Current.Server.MapPath(dataPath);
			var json = File.ReadAllText(dataPath);
		}
	}
}