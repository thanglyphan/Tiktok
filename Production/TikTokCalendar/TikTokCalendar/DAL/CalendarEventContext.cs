using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using TikTokCalendar.Models;

// The database itself. C# uses the fields (a bunch of DbSet<> a.k.a. tables) to set up the database.

namespace TikTokCalendar.DAL
{
	public class CalendarEventContext : DbContext
	{
		public CalendarEventContext() : base("CalendarEventContext")
			// Name to be used in the Web.config (could put connection string here instead)
		{
		}

		public DbSet<EventUserStat> EventUserStats { get; set; }

		// This makes it so that table names won't become plurizized (Rooms, Subjects, etc)
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}