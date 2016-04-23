using System.Collections.Generic;

namespace TikTokCalendar.DAL
{
	public class Room
	{
		public Room(string roomName)
		{
			RoomName = roomName;
		}

		public string RoomName { get; private set; }
		public List<TimeSlot> Availability { get; } = new List<TimeSlot>();

		public bool TryAddTimeSlot(TimeSlot timeSlot)
		{
			Availability.Add(timeSlot);
			return false;
		}
	}
}