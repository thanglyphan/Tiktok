using System;
using NUnit.Framework;
using TikTokCalendar.DAL;

namespace TikTokCalendar.UnitTests
{
	[TestFixture]
	public class TimeSlotTest
	{
		[Test]
		public void Percent100()
		{
			var now = DateTime.Now;
			var start = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
			var end = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);

			var slot = new TimeSlot(start, end);
			var percent = slot.GetPercentOfDay();
			Assert.AreEqual(100, percent);
		}

		[Test]
		public void Percent50()
		{
			var now = DateTime.Now;
			var start = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
			var end = new DateTime(now.Year, now.Month, now.Day, 13, 0, 0);

			var slot = new TimeSlot(start, end);
			var percent = slot.GetPercentOfDay();
			Assert.AreEqual(50, percent);
		}

		//public void PercentStart50()

		//[Test]
		//{
		//	DateTime now = DateTime.Now;
		//	DateTime start = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
		//	DateTime end = new DateTime(now.Year, now.Month, now.Day, 14, 0, 0);

		//	TimeSlot slot = new TimeSlot(start, end);
		//	var percent = slot.GetStartPercent();
		//	Assert.AreEqual(50, percent);
		//}

		//[Test]
		//public void PercentStart100()
		//{
		//	DateTime now = DateTime.Now;
		//	DateTime start = new DateTime(now.Year, now.Month, now.Day, 8, 0, 0);
		//	DateTime end = new DateTime(now.Year, now.Month, now.Day, 18, 0, 0);

		//	TimeSlot slot = new TimeSlot(start, end);
		//	var percent = slot.GetStartPercent();
		//	Assert.AreEqual(100, percent);
		//}
	}
}