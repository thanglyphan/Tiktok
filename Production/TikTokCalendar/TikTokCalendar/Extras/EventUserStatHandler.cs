using TikTokCalendar.DAL;
using TikTokCalendar.Models;

namespace TikTokCalendar
{
    public class EventUserStatHandler
    {
        private CalendarEventContext db = new CalendarEventContext();

        public bool UserGoing(long id, string username)
        {
            foreach (EventUserStat eus in db.EventUserStats)
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
            foreach (EventUserStat eus in db.EventUserStats)
            {
                if (eus.UserName == username && eus.EventID == id && eus.Attend == true)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetUsersGoing(long id)
        {
            int count = 0;
            foreach (EventUserStat eus in db.EventUserStats)
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
            int count = 0;
            foreach (EventUserStat eus in db.EventUserStats)
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