using System;
using System.Web;
using System.Web.UI;

namespace TikTokCalendar.DAL
{
	public class Cookies : System.Web.UI.Page
	{
		
		public void SaveNameToCookie(String a)
		{
			try {
				HttpCookie UsernameCookie = new HttpCookie("UserName");
				if (a == "Default") {
					UsernameCookie.Value = "anonym14";
					UsernameCookie.Expires = DateTime.Now.AddSeconds(5);
				}
				else {
					UsernameCookie.Value = a;
					UsernameCookie.Expires = DateTime.Now.AddYears(1);
				}
				Console.WriteLine("FROM COOKES.CS" + a);
				HttpContext.Current.Response.Cookies.Add(UsernameCookie);
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - SaveNameToCookies " + e.ErrorCode);
			}
		}
		/*
		public void SaveIntToCookie(int a)
		{
			try {
				HttpCookie WeekOrMonthCookie = new HttpCookie("WeekMonth");
				WeekOrMonthCookie.Value = a.ToString();
				WeekOrMonthCookie.Expires = DateTime.Now.AddYears(1);
				Console.WriteLine("FROM COOKES.CS" + a);
				HttpContext.Current.Response.Cookies.Add(WeekOrMonthCookie);
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - SaveIntToCookies " + e.ErrorCode);
			}
		}
		*/
		public void DeleteCookies()
		{
			HttpCookie delC = new HttpCookie("UserName");
			HttpCookie delD = new HttpCookie("UserCouese");

			delC.Expires = DateTime.Now.AddDays(-1);
			delD.Expires = DateTime.Now.AddDays(-1);

			delC.Value = null;
			delD.Value = null;

			HttpContext.Current.Response.Cookies.Add(delC);
			HttpContext.Current.Response.Cookies.Add(delC);

		}

		public void SaveCourseToCookie(String a)
		{
			try {
				HttpCookie CourseCookie = new HttpCookie("UserCourse");
				if (a == "Default") {
					CourseCookie.Value = "VisAlt"; //Vis alt. Enum?
					CourseCookie.Expires = DateTime.Now.AddSeconds(5);
				}
				else {
					CourseCookie.Value = a;
					CourseCookie.Expires = DateTime.Now.AddYears(1);
				}
				HttpContext.Current.Response.Cookies.Add(CourseCookie);
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - SaveCourseToCookie " + e);
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
				Console.Write("Cookies.cs - LoadStringFromCookie " + e.ErrorCode);
			}

			return null;

		}

		public int LoadIntFromCookie(String key) //Week or month view by user.
		{
			String s = LoadStringFromCookie(key);
			int number;
			if(int.TryParse(s,out number)) {
				return number;
			}
			return -1;
		}
	}
}