using System.Linq;
using NUnit.Framework;
using TikTokCalendar.DAL;

namespace TikTokCalendar.UnitTests
{
	[TestFixture]
	public class RoomNameSplitTest
	{
		[SetUp]
		public void Setup()
		{
			dp = new DataParser();
		}

		private DataParser dp;
		private readonly string roomText = "Rom 40, Rom 79-80, Rom 81, Vrimle";

		[Test]
		public void FindRoom79_80()
		{
			var results = dp.GetRoomsFromRoomText(roomText);
			Assert.Contains("Rom 79-80", results);
		}

		[Test]
		public void FindRoom81()
		{
			var results = dp.GetRoomsFromRoomText(roomText);
			Assert.Contains("Rom 81", results);
		}

		[Test]
		public void FindUnavailibleRoom()
		{
			var results = dp.GetRoomsFromRoomText(roomText);
			Assert.AreEqual(results.Contains("Rom1"), false);
		}
	}
}