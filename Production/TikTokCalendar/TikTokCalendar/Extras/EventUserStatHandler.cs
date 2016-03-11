using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using TikTokCalendar.DAL;
using TikTokCalendar.Models;

namespace TikTokCalendar
{
    public static class EUSH_global
    {
        public static long ID_ATM = 0; // event ID at the moment
    }
    public class EventUserStatHandler
    {
        private CalendarEventContext db = new CalendarEventContext();
        private Cookies cookie = new Cookies();

        public bool UserGoing(long id)
        {
            string userName = cookie.LoadStringFromCookie("UserName");
            foreach (EventUserStat eus in db.EventUserStats)
            {
                if (eus.UserName == userName && eus.EventID == id)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UserAttending(long id, string username)
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

        public int HowManyUsersGoing(long id)
        {
            int counter = 0;
            foreach (EventUserStat eus in db.EventUserStats)
            {
                if (eus.EventID == id)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}