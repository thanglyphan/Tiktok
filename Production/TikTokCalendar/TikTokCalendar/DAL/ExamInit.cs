using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using TikTokCalendar.Models;
using Newtonsoft.Json;

namespace TikTokCalendar.DAL
{
	public class ExamInit
	{
		CalendarEventInitializer a; //Need this to find right "emne kode"
		public ExamInit(CalendarEventInitializer a)
		{
			this.a = a;
		}
		
		//Returns a list used for adding data to database in CalendarEventInitializer.cs
		public List<CalendarEvent> ReadJsonFile()
		{
			List<CalendarEvent> list = new List<CalendarEvent>();
			CalendarEvent data = null;
			//Find the right JSON file, read it.
			var dataPath = "~/Content/timeedit/innlevering-eksamen-dato.json";
			dataPath = HttpContext.Current.Server.MapPath(dataPath);
			var json = File.ReadAllText(dataPath);

			var rootObj = JsonConvert.DeserializeObject<RootObject>(json);

			//Getting each item in list rootObj.reservations.
			foreach (var item in rootObj.reservations) {
				var start = CalendarEventInitializer.GetParsedDateTime(item.Dato + "16", "09:00");
				var end = CalendarEventInitializer.GetParsedDateTime(item.Dato,"f");
				int year = -1; // TODO Get year based on which subject it is (check against another subject and see what year that is?)

				//This variable become CalendarEvent obj.
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
					item.Emnekode + "\n Emnenavn: " + item.Emnenavn + "\n Hjelpemidler: " + item.Hjelpemidler,
					Year = year
				};
				list.Add(data); //For each item, add data(CalendarEvent) to list.
			}
			return list;
		}

	}

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

