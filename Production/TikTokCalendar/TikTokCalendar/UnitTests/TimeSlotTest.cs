﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Web;
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
			DateTime now = DateTime.Now;
			DateTime start = new DateTime(now.Year, now.Month, now.Day, 7, 0, 0);
			DateTime end = new DateTime(now.Year, now.Month, now.Day, 19, 0, 0);

			TimeSlot slot = new TimeSlot(start, end);
			var percent = slot.GetPercentOfDay();
			Assert.AreEqual(100, percent);
		}

		[Test]
		public void Percent50()
		{
			DateTime now = DateTime.Now;
			DateTime start = new DateTime(now.Year, now.Month, now.Day, 7, 0, 0);
			DateTime end = new DateTime(now.Year, now.Month, now.Day, 13, 0, 0);

			TimeSlot slot = new TimeSlot(start, end);
			var percent = slot.GetPercentOfDay();
			Assert.AreEqual(50, percent);
		}
	}
}