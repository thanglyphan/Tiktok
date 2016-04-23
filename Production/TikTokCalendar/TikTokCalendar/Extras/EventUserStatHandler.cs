using TikTokCalendar.DAL;

namespace TikTokCalendar
{
	public class EventUserStatHandler
	{
		private readonly CalendarEventContext db = new CalendarEventContext();

		public bool UserGoing(long id, string username)
		{
			foreach (var eus in db.EventUserStats)
			{
				if (eus.UserName == username && eus.EventID == id)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsUserAttending(long id, string username)
		{
			foreach (var eus in db.EventUserStats)
			{
				if (eus.UserName == username && eus.EventID == id && eus.Attend)
				{
					return true;
				}
			}
			return false;
		}

		public int GetUsersGoing(long id)
		{
			var count = 0;
			foreach (var eus in db.EventUserStats)
			{
				if (eus.EventID == id)
				{
					count++;
				}
			}
			return count;
		}

		public int GetUsersAttending(long id)
		{
			var count = 0;
			foreach (var eus in db.EventUserStats)
			{
				if (eus.EventID == id && eus.Attend)
				{
					count++;
				}
			}
			return count;
		}
	}
}