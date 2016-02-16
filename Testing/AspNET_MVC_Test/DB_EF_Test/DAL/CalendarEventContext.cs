using System;
using System.Data.Entity;
using DB_EF_Test.Models;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DB_EF_Test.DAL
{
	public class CalendarEventContext : DbContext
	{
		public CalendarEventContext() : base("CalendarEventContext") // Name to be used in the Web.config (could put connection string here instead)
		{
		}

		public DbSet<Room> Rooms { get; set; }
		public DbSet<CalendarEvent> CalendarEvents { get; set; } // Could omit, because Rooms reference Calendarevent, which reference Subject
		public DbSet<Subject> Subjects { get; set; } // Could omit, because Rooms reference Calendarevent, which reference Subject

		// This makes it so that table names won't become plurizized (Rooms, Subjects, etc)
		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
		}
	}
}