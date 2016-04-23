using System.Collections.Generic;
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
			validUser = new StudentUser("spillprog2", "1234", "spillprog2@gmail.com", 2, SchoolCourses.Spillprogrammering);
			invalidUser = new StudentUser("spillprog2", "1", "spillprog2@gmail.com", 2, SchoolCourses.Spillprogrammering);

			userList = new List<StudentUser>();
			userList.Add(new StudentUser("prog2", "1234", "prog2@gmail.com", 2, SchoolCourses.Programmering));
			userList.Add(new StudentUser("prog3", "1234", "prog3@gmail.com", 3, SchoolCourses.Programmering));
			userList.Add(new StudentUser("spillprog2", "1234", "spillprog2@gmail.com", 2, SchoolCourses.Spillprogrammering));
			userList.Add(new StudentUser("spillprog3", "1234", "spillprog3@gmail.com", 3, SchoolCourses.Spillprogrammering));
		}

		private StudentUser validUser;
		private StudentUser invalidUser;
		private List<StudentUser> userList;

		[Test]
		public void NotValidUser()
		{
			var userIsValid = validUser.IsValid(userList[0]);
			Assert.AreEqual(userIsValid, false);
		}

		[Test]
		public void UserDoesntExistsInList()
		{
			var userIsValid = false;
			foreach (var u in userList)
			{
				if (u.IsValid(invalidUser))
				{
					userIsValid = true;
					break;
				}
			}
			Assert.AreEqual(userIsValid, false);
		}

		[Test]
		public void UserExistsInList()
		{
			var userIsValid = false;
			foreach (var u in userList)
			{
				if (u.IsValid(validUser))
				{
					userIsValid = true;
					break;
				}
			}
			Assert.AreEqual(userIsValid, true);
		}

		[Test]
		public void ValidUser()
		{
			var userIsValid = validUser.IsValid(userList[2]);
			Assert.AreEqual(userIsValid, true);
		}
	}
}