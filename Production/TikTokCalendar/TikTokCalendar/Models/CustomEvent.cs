using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using TikTokCalendar.DAL;

namespace TikTokCalendar.Models
{
	public class CustomEvent
	{
		public int ID { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string EventName { get; set; }
		public int ClassYear { get; set; }
		public List<SchoolCourses> Courses { get; set; }
		public string RoomName { get; set; }
		public string Teacher { get; set; }
		public string EventType { get; set; }
		public string Comment { get; set; }

		public CustomEvent()
		{
			// parse date, get year, get courses
		}

		public bool SetAndParse(string id, string startDate, string startTime, string endDate, string endTime,
			string subject, string courseData, string room, string teacher, string activity, string comment)
		{
			bool success = true;

			// Parse ID
			int parsedId = -1;
			if (int.TryParse(id, NumberStyles.Integer, new NumberFormatInfo(), out parsedId))
			{
				ID = parsedId;
			}
			else
			{
				success = false;
			}

			// Startdate

			// Enddate

			// Emne (get course and turn into a subject class (compare the id (the last 11 characters)?)

			// Studioprogram (get classYear and different courses from this; just compare the names :/ )

			// Just assign the rest of the string data
			RoomName = room;
			Teacher = teacher;
			EventType = activity;
			Comment = comment;

			//Printer.Print("Parsing of the event: {" + ((success) ? "Successfully parsed" : "Couldn't parse!") + "}");
			if (!success)
			{
				Printer.Print("ERROR parsing event, couldn't parse event!");
			}
			return success;
		}

		/*
		 "Emne",
        "Studieprogram",
        "Rom",
        "Lærer",
        "Aktivitet",
        "Kommentar"
		*/
	}
}