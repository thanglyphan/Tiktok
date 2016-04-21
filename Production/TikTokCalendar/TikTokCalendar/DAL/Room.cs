using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikTokCalendar.DAL
{
	public class Room
	{
		public string RoomName { get; private set; }
		public List<TimeSlot> Availability { get; private set; } = new List<TimeSlot>();

		public Room(string roomName)
		{
			RoomName = roomName;
		}

		public bool TryAddTimeSlot(TimeSlot timeSlot)
		{
			Availability.Add(timeSlot);
			return false;
		}
	}
}