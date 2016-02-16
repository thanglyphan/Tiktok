using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using DB_EF_Test.Models;

namespace DB_EF_Test.DAL
{
	//public class CalendarEventInitializer : DropCreateDatabaseIfModelChanges<CalendarEventContext>
	/*NOTE: 
	 * Modifying the insertion doesn't count as model change.
	 * Modifying the DB name in the connection string DOES count as model change.
	 */
    public class CalendarEventInitializer : DropCreateDatabaseAlways<CalendarEventContext>
	{
		protected override void Seed(CalendarEventContext context)
		{
			var rooms = new List<Room>
			{
				new Room {Name="Auditoriet",Location="Galleriet"},
				new Room {Name="Vrimle",Location="Galleriet"},
				new Room {Name="Rom81",Location="Galleriet"},
				new Room {Name="Rom82",Location="Galleriet"},
				new Room {Name="Rom42",Location="Galleriet"},
				new Room {Name="Rom43",Location="Galleriet"}
			};
			rooms.ForEach(r => context.Rooms.Add(r));
			context.SaveChanges();

			var subjects = new List<Subject>
			{
				new Subject {Name="PG3300 - Programvarearkitektur"},
				new Subject {Name="PG3400 - Programmering i C for Linux"},
				new Subject {Name="PG4200 - Algoritmer og datastrukturer"},
				new Subject {Name="PJ3100 - Prosjekt software engineering"},
			};
			subjects.ForEach(s => context.Subjects.Add(s));
			context.SaveChanges();


			var calendarEvents = new List<CalendarEvent>
			{
				new CalendarEvent {StartTime=DateTime.Parse("11/23/2015 10:15:00 AM"),EndTime=DateTime.Parse("11/23/2015 14:00:00 PM"),SubjectID=1,RoomID=3,Attendees="SpillProg, prog",EventType=EventType.Forelesning,Comment=""},
				new CalendarEvent {StartTime=DateTime.Parse("11/25/2015 13:15:00 PM"),EndTime=DateTime.Parse("11/25/2015 17:00:00 PM"),SubjectID=2,RoomID=1,Attendees="SpillProg, Int.sys.",EventType=EventType.Forelesning,Comment=""},
				new CalendarEvent {StartTime=DateTime.Parse("11/27/2015 10:15:00 AM"),EndTime=DateTime.Parse("11/27/2015 14:00:00 PM"),SubjectID=3,RoomID=1,Attendees="SpillProg, Prog",EventType=EventType.Forelesning,Comment=""},
				new CalendarEvent {StartTime=DateTime.Parse("12/02/2015 10:15:00 AM"),EndTime=DateTime.Parse("12/02/2015 12:00:00 PM"),SubjectID=4,RoomID=2,Attendees="SpillProg, Prog",EventType=EventType.Innlevering,Comment=""},
				new CalendarEvent {StartTime=DateTime.Parse("12/15/2015 09:00:00 AM"),EndTime=DateTime.Parse("12/15/2015 14:00:00 PM"),SubjectID=2,RoomID=5,Attendees="SpillProg, Prog",EventType=EventType.Eksamen,Comment=""}
			};
			/*var calendarEvents = new List<CalendarEvent>
			{
				new CalendarEvent {StartTime=DateTime.Parse("11/22/2015 10:15:00 AM"),EndTime=DateTime.Now,SubjectID=1,RoomID=3,Attendees="SpillProg, prog",EventType=EventType.Forelesning,Comment="COMMENT"},
				new CalendarEvent {StartTime=DateTime.Parse("11/22/2015 10:15:00 AM"),EndTime=DateTime.Now,SubjectID=1,RoomID=3,Attendees="SpillProg, prog",EventType=EventType.Forelesning,Comment="c2"}
			};*/
			calendarEvents.ForEach(c => context.CalendarEvents.Add(c));
			context.SaveChanges();
		}
	}
}