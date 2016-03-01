using System;
using System.Web;

namespace TikTokCalendar.DAL
{
	public class Cookies
	{
		//Add cookie method.	
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
		public String LoadStringFromCookie(String key)
		{
			HttpCookie myCookie = HttpContext.Current.Request.Cookies[key];

			if (myCookie == null) { //Returns the value og cookie if not null.
				return null;
			}
			else {
				return myCookie.Value;
			}
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