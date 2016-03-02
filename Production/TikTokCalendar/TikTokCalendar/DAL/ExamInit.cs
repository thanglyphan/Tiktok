using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

using TikTokCalendar.Models;
using Newtonsoft.Json;
using System.Globalization;

namespace TikTokCalendar.DAL
{
	public class ExamInit
	{
		CalendarEventInitializer a;
		//CalendarEventContext context;
		public ExamInit(CalendarEventInitializer a)
		{
			this.a = a;
			//this.context = context;
			//ReadJsonFile(context);
		}
		
		public List<CalendarEvent> ReadJsonFile()
		{
			List<CalendarEvent> list = new List<CalendarEvent>();
			CalendarEvent data = null;
			var dataPath = "~/Content/timeedit/innlevering-eksamen-dato.json";
			dataPath = HttpContext.Current.Server.MapPath(dataPath);
			var json = File.ReadAllText(dataPath);

			var rootObj = JsonConvert.DeserializeObject<RootObject>(json);
			int count = 0;
			foreach (var item in rootObj.reservations) {
				count++;

				var start = CalendarEventInitializer.GetParsedDateTime(item.Dato + "16", "09:00");
				var end = CalendarEventInitializer.GetParsedDateTime(item.Dato,"f");
				data = new CalendarEvent {
					StartTime = DateTime.Now,
					EndTime = DateTime.Now,
					TimeEditID = 1,
					SubjectID = a.GetSubjectIdFromCode(item.Emnekode),
					RoomName = "Info kommer",
					EventName = item.Vurderingstype, //its ok
					Attendees = item.Emnenavn + item.Vurderingstype,
					Teacher = "Thang Phan",
					Comment = "Varighet: " + item.Varighet + "\n Vekting: " + item.Vekting + "\n Emnekode: " +
					item.Emnekode + "\n Emnenavn: " + item.Emnenavn + "\n Hjelpemidler: " + item.Hjelpemidler
				};
				list.Add(data);
			}
			return list;
		}

	}

	/*
	StartTime = start,
					EndTime = end,
					TimeEditID = timeEditId,
					//SubjectName = item.columns[0], 
					SubjectID = subject,
					RoomName = item.columns[2],
					EventName = item.columns[4],
					Attendees = item.columns[1],
					Teacher = item.columns[3],
					Comment = item.columns[5]
					*/

	public class Reservation
	{
		public string Dato { get; set; }
		public string Emnekode { get; set; }
		public string Emnenavn { get; set; }
		public string Vurderingstype { get; set; }
		public int Vekting { get; set; }
		public string Varighet { get; set; }
		public string Hjelpemidler { get; set; }
	}

	public class RootObject
	{
		public List<Reservation> reservations { get; set; }
	}

}

