using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikTokCalendar.Models
{
    public class EventUserStat
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public int EventID { get; set; }
        public DateTime GoingTime { get; set; }
        public bool Attend { get; set; }
    }
}