using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using TikTokCalendar.DAL;

namespace TikTokCalendar.UnitTests
{
	[TestFixture]
	public class RoomNameSplitTest
	{
		private DataParser dp;
		private string roomText = "Rom 40, Rom 79-80, Rom 81, Vrimle";

		[SetUp]
		public void Setup()
		{
			dp = new DataParser();
		}

		[Test]
		public void FindRoom81()
		{
			string[] results = dp.GetRoomsFromRoomText(roomText);
			Assert.Contains("Rom 81", results);
		}

		[Test]
		public void FindRoom79_80()
		{
			string[] results = dp.GetRoomsFromRoomText(roomText);
			Assert.Contains("Rom 79-80", results);
		}

		[Test]
		public void FindUnavailibleRoom()
		{
			string[] results = dp.GetRoomsFromRoomText(roomText);
			Assert.AreEqual(results.Contains("Rom1"), false);
		}
	}
}