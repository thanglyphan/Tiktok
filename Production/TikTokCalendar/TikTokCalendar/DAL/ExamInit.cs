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
		//CalendarEventContext context;
		public ExamInit()
		{
			//this.context = context;
			//ReadJsonFile(context);
		}
		
		public CalendarEvent ReadJsonFile()
		{
			//List<CalendarEvent> list = new List<CalendarEvent>();
			CalendarEvent data = null;
			var dataPath = "~/Content/timeedit/innlevering-eksamen-dato.json";
			dataPath = HttpContext.Current.Server.MapPath(dataPath);
			var json = File.ReadAllText(dataPath);

			var rootObj = JsonConvert.DeserializeObject<RootObject>(json);
			int count = 0;
			foreach (var item in rootObj.reservations) {
				count++;

				var start = CalendarEventInitializer.GetParsedDateTime(item.Dato,"f");
				var end = CalendarEventInitializer.GetParsedDateTime(item.Dato,"f");
				data = new CalendarEvent {
					StartTime = start,
					EndTime = end,
					TimeEditID = 1,
					SubjectID = 123 + count,
					RoomName = item.Emnekode,
					EventName = item.Vurderingstype,
					Attendees = item.Emnenavn,
					Teacher = item.Emnenavn,
					Comment = "Varighet: " + item.Varighet + "\n Vekting: " + item.Vekting + "\n Emnekode: " +
					item.Emnekode + "\n Emnenavn: " + item.Emnenavn + "\n Hjelpemidler: " + item.Hjelpemidler
				};
				return data;
			}
			return data;
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

