using System;
using System.Web;


namespace TikTokCalendar.DAL
{
	public class Cookies
	{
		private static HttpCookie UsernameCookie = null;
		private static HttpCookie CourseCookie = null;
		private static HttpCookie WeekOrMonthCookie;

		private static HttpCookie myCookie;


		//Add cookie method.	
		/*
		public void SaveToCookie(String UserNameKey,String CourseKey,int WeekOrMonthKey, StudentUser a)
		{

			HttpCookie UsernameCookie = new HttpCookie(UserNameKey);
			HttpCookie CourseCookie = new HttpCookie(CourseKey);
			HttpCookie WeekOrMonthCookie = new HttpCookie(WeekOrMonthKey.ToString());
			UsernameCookie.Value = a.UserName;
			CourseCookie.Value =((int) a.Course).ToString();
			WeekOrMonthCookie.Value = ((int)a.WeekOrMonthShow).ToString();

			UsernameCookie.Expires = DateTime.Now.AddHours(1);
			CourseCookie.Expires = DateTime.Now.AddHours(1);
			WeekOrMonthCookie.Expires = DateTime.Now.AddHours(1);


            HttpContext.Current.Response.Cookies.Add(UsernameCookie);
			HttpContext.Current.Response.Cookies.Add(CourseCookie);
			HttpContext.Current.Response.Cookies.Add(WeekOrMonthCookie);
		}
		*/
		public static void SaveNameToCookie(String a)
		{
			UsernameCookie = new HttpCookie("UserName");
			UsernameCookie.Value = a;
			UsernameCookie.Expires = DateTime.Now.AddHours(1);
			Console.WriteLine("FROM COOKES.CS" + a);
			HttpContext.Current.Response.Cookies.Add(UsernameCookie);
		}

		public static void SaveCourseToCookie(String a)
		{
			CourseCookie = new HttpCookie("UserCourse");
			CourseCookie.Value = a;
			CourseCookie.Expires = DateTime.Now.AddHours(1);
			HttpContext.Current.Response.Cookies.Add(CourseCookie);
		}
		public static String LoadStringFromCookie(String key)
		{
			myCookie = new HttpCookie(key);
			myCookie = HttpContext.Current.Request.Cookies[key];

			Console.WriteLine(myCookie.Value);

			if (myCookie == null) { //Returns the value og cookie if not null.
				return null;
			}
			else {
				Console.WriteLine(myCookie.Value);
				return myCookie.Value;
			}
		}

		/*
		public static int LoadIntFromCookie(String key) //Week or month view by user.
		{
			String s = LoadStringFromCookie(key);
			int number;
			if(int.TryParse(s,out number)) {
				return number;
			}
			return -1;
		}
		*/
	}
}