using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;
using TikTokCalendar.DAL;

namespace TikTokCalendar.UnitTests
{
	[TestFixture]
	public class StudentUserTest
	{
		[SetUp]
		protected void SetUp()
		{
			DataParser dp = new DataParser();
			dp.ParseAllData();

		}
	}
}