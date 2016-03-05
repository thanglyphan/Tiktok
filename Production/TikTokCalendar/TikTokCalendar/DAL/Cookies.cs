using System;
using System.Web;
using System.Web.UI;

namespace TikTokCalendar.DAL
{
	public class Cookies : System.Web.UI.Page
	{


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
		public void SaveNameToCookie(String a)
		{
			try {
				HttpCookie UsernameCookie = new HttpCookie("UserName");
				UsernameCookie.Value = a;
				UsernameCookie.Expires = DateTime.Now.AddHours(1);
				Console.WriteLine("FROM COOKES.CS" + a);
				HttpContext.Current.Response.Cookies.Add(UsernameCookie);
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - SaveNameToCookies");
			}
		}

		public void SaveCourseToCookie(String a)
		{
			try {
				HttpCookie CourseCookie = new HttpCookie("UserCourse");
				CourseCookie.Value = a;
				CourseCookie.Expires = DateTime.Now.AddHours(1);
				HttpContext.Current.Response.Cookies.Add(CourseCookie);
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - SaveCourseToCookie");
			}
		}
		public String LoadStringFromCookie(String key)
		{
			try {
				HttpCookie myCookie = new HttpCookie(key);
				myCookie = HttpContext.Current.Request.Cookies[key];

				if (myCookie == null) { //Returns the value og cookie if not null.
					return null;
				}
				else {
					Console.WriteLine(myCookie.Value);
					return myCookie.Value;
				}
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - LoadStringFromCookie");
			}

			return null;

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