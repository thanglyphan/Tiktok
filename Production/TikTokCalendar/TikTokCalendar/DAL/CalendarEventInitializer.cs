using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Web;
using Newtonsoft.Json;
using TikTokCalendar.Models;

// Class for initializing the database. 
//The Seed() function is called either always or on a change (depending on what class it inherits from)

namespace TikTokCalendar.DAL
{
	public class CalendarEventInitializer : DropCreateDatabaseIfModelChanges<CalendarEventContext>
	/*NOTE: 
	 * Modifying the insertion doesn't count as model change.
	 * Modifying the DB name in the connection string DOES count as model change.
	 */
	//public class CalendarEventInitializer : DropCreateDatabaseAlways<CalendarEventContext>
	{
		protected override void Seed(CalendarEventContext context)
		{
		}
	}
}