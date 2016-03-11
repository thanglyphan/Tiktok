using System;
using System.Web;
using System.Web.UI;

namespace TikTokCalendar.DAL
{
	public class Cookies : System.Web.UI.Page
	{
		
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
			try {

				HttpCookie name = new HttpCookie("UserName");
				HttpCookie course = new HttpCookie("UserCourse");
				HttpCookie year = new HttpCookie("Year");

				year.Expires = DateTime.Now.AddDays(-1);
				name.Expires = DateTime.Now.AddDays(-1);
				course.Expires = DateTime.Now.AddDays(-1);

				HttpContext.Current.Response.Cookies.Add(name);
				HttpContext.Current.Response.Cookies.Add(course);
				HttpContext.Current.Response.Cookies.Add(year);


				/*
				HttpCookie name = new HttpCookie("UserName");
				HttpCookie course = new HttpCookie("UserCouese");
				name = HttpContext.Current.Request.Cookies["UserName"];
				course = HttpContext.Current.Request.Cookies[key];
				if ()
					*/
			}
			catch (HttpException e) {
				Console.WriteLine(e.ToString());
			}
		}
		public void SaveYearToCookies(string a)
		{
			try {
				HttpCookie YearCookie = new HttpCookie("Year");
				YearCookie.Value = a;
				YearCookie.Expires = DateTime.Now.AddYears(1);
				HttpContext.Current.Response.Cookies.Add(YearCookie);


			}catch (HttpException e) {
				e.ToString();
			}
		}
		public void SaveNameToCookie(String a)
		{

            string [] b = a.Split(';');
			if (b.Length >= 2)
			{
				SaveYearToCookies(b[1]);
			}

			try {
				HttpCookie UsernameCookie = new HttpCookie("UserName");
				if (a == "Default") {
					UsernameCookie.Value = "John Doe";
					SaveYearToCookies(DateTime.Now.Year.ToString());
					UsernameCookie.Expires = DateTime.Now.AddSeconds(5);
				}
				else {
					UsernameCookie.Value = b[0];
					UsernameCookie.Expires = DateTime.Now.AddYears(1);
				}
				//Console.WriteLine("FROM COOKES.CS" + a);
				HttpContext.Current.Response.Cookies.Add(UsernameCookie);
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - SaveNameToCookies " + e.ErrorCode);
			}
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