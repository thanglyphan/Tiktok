using System;
using System.Web;
using System.Web.UI;

namespace TikTokCalendar.DAL
{
	public class Cookies : System.Web.UI.Page
	{
		public const string UserNameCookieKey = "UserName";
		public const string YearCookieKey = "Year";
		public const string CourseCookieKey = "UserCourse";

		private DateTime ExpiryDate { get { return DateTime.Now.AddYears(1); } }

		public void DeleteCookies()
		{
			try {
				HttpCookie name = new HttpCookie(UserNameCookieKey);
				HttpCookie course = new HttpCookie(CourseCookieKey);
				HttpCookie year = new HttpCookie(YearCookieKey);

				year.Expires = DateTime.Now.AddDays(-1);
				name.Expires = DateTime.Now.AddDays(-1);
				course.Expires = DateTime.Now.AddDays(-1);

				HttpContext.Current.Response.Cookies.Add(name);
				HttpContext.Current.Response.Cookies.Add(course);
				HttpContext.Current.Response.Cookies.Add(year);
			}
			catch (HttpException e) {
				Console.WriteLine(e.ToString());
			}
		}
		public void SaveYearToCookies(string a)
		{
			try {
				HttpCookie yearCookie = new HttpCookie(YearCookieKey);
				yearCookie.Value = a;
				yearCookie.Expires = ExpiryDate;
				HttpContext.Current.Response.Cookies.Add(yearCookie);


			}catch (HttpException e) {
				e.ToString();
			}
		}
		public void SaveNameToCookie(string a)
		{
			try {
				HttpCookie usernameCookie = new HttpCookie(UserNameCookieKey);
				
				usernameCookie.Value = a;
				usernameCookie.Expires = ExpiryDate;

				HttpContext.Current.Response.Cookies.Add(usernameCookie);
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - SaveNameToCookies " + e.ErrorCode);
			}
		}
		public void SaveCourseToCookie(string a)
		{
			try {
				HttpCookie courseCookie = new HttpCookie(CourseCookieKey);
				
				courseCookie.Value = a;
				courseCookie.Expires = ExpiryDate;
				HttpContext.Current.Response.Cookies.Add(courseCookie);
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - SaveCourseToCookie " + e);
			}
		}
		public void SaveHasShownToCookie()
		{
			try {
				HttpCookie hasShownCookie = new HttpCookie("Shown");
				hasShownCookie.Value = "true";
				hasShownCookie.Expires = ExpiryDate;
				HttpContext.Current.Response.Cookies.Add(hasShownCookie);
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - SaveCourseToCookie " + e);
			}
		}
		public bool LoadHasShownFromCookie()
		{
			try {
				HttpCookie myCookie = new HttpCookie("Shown");
				myCookie = HttpContext.Current.Request.Cookies["Shown"];

				if (myCookie != null && myCookie.Value == "true") { //Returns the value og cookie if not null.
					return true;
				}
				else {
					return false;
				}
			}
			catch (HttpException e) {
				Console.Write("Cookies.cs - LoadHasShownFromCookie " + e.ErrorCode);
			}
			return false;
		}

		public string LoadStringFromCookie(string key)
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
	}
}