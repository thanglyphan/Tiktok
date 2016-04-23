using System;
using System.Web;
using System.Web.UI;

namespace TikTokCalendar.DAL
{
	public class Cookies : Page
	{
		public const string UserNameCookieKey = "UserName";
		public const string YearCookieKey = "Year";
		public const string CourseCookieKey = "UserCourse";
		public const string ShownCookieText = "Shown";


		private DateTime ExpiryDate
		{
			get { return DateTime.Now.AddYears(1); }
		}

		public void DeleteCookies()
		{
			try
			{
				var name = new HttpCookie(UserNameCookieKey);
				var course = new HttpCookie(CourseCookieKey);
				var year = new HttpCookie(YearCookieKey);
				var shown = new HttpCookie(ShownCookieText);


				shown.Value = "";
				name.Value = "";
				course.Value = "";
				year.Value = "";

				shown.Expires = DateTime.Now.AddDays(-1);
				year.Expires = DateTime.Now.AddDays(-1);
				name.Expires = DateTime.Now.AddDays(-1);
				course.Expires = DateTime.Now.AddDays(-1);

				HttpContext.Current.Response.Cookies.Add(shown);
				HttpContext.Current.Response.Cookies.Add(name);
				HttpContext.Current.Response.Cookies.Add(course);
				HttpContext.Current.Response.Cookies.Add(year);
			}
			catch (HttpException e)
			{
				Console.WriteLine(e.ToString());
			}
		}

		public void SaveYearToCookies(string a)
		{
			try
			{
				var yearCookie = new HttpCookie(YearCookieKey);
				yearCookie.Value = a;
				yearCookie.Expires = ExpiryDate;
				HttpContext.Current.Response.Cookies.Add(yearCookie);
			}
			catch (HttpException e)
			{
				e.ToString();
			}
		}

		public void SaveNameToCookie(string a)
		{
			try
			{
				var usernameCookie = new HttpCookie(UserNameCookieKey);

				usernameCookie.Value = a;
				usernameCookie.Expires = ExpiryDate;

				HttpContext.Current.Response.Cookies.Add(usernameCookie);
			}
			catch (HttpException e)
			{
				Console.Write("Cookies.cs - SaveNameToCookies " + e.ErrorCode);
			}
		}

		public void SaveCourseToCookie(string a)
		{
			try
			{
				var courseCookie = new HttpCookie(CourseCookieKey);

				courseCookie.Value = a;
				courseCookie.Expires = ExpiryDate;
				HttpContext.Current.Response.Cookies.Add(courseCookie);
			}
			catch (HttpException e)
			{
				Console.Write("Cookies.cs - SaveCourseToCookie " + e);
			}
		}

		public void SaveHasShownToCookie()
		{
			try
			{
				var hasShownCookie = new HttpCookie(ShownCookieText);
				hasShownCookie.Value = "true";
				hasShownCookie.Expires = ExpiryDate;
				HttpContext.Current.Response.Cookies.Add(hasShownCookie);
			}
			catch (HttpException e)
			{
				Console.Write("Cookies.cs - SaveCourseToCookie " + e);
			}
		}

		public bool LoadHasShownFromCookie()
		{
			try
			{
				var myCookie = new HttpCookie("Shown");
				myCookie = HttpContext.Current.Request.Cookies["Shown"];

				if (myCookie != null && myCookie.Value == "true")
				{
					//Returns the value og cookie if not null.
					return true;
				}
				return false;
			}
			catch (HttpException e)
			{
				Console.Write("Cookies.cs - LoadHasShownFromCookie " + e.ErrorCode);
			}
			return false;
		}

		public string LoadStringFromCookie(string key)
		{
			try
			{
				var myCookie = new HttpCookie(key);
				myCookie = HttpContext.Current.Request.Cookies[key];

				if (myCookie == null)
				{
					//Returns the value og cookie if not null.
					return null;
				}
				Console.WriteLine(myCookie.Value);
				return myCookie.Value;
			}
			catch (HttpException e)
			{
				Console.Write("Cookies.cs - LoadStringFromCookie " + e.ErrorCode);
			}

			return null;
		}
	}
}