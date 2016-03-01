using System;
using System.Web;

namespace TikTokCalendar.DAL
{
	public class Cookies
	{

		public void SaveToCookie(String UserName)
		{
			HttpCookie myCookie = new HttpCookie(UserName);
			myCookie.Value = UserName;
			myCookie.Expires = DateTime.Now.AddHours(1);
			
			/*
			HttpCookie yourCookie = new HttpCookie(UserName);
			yourCookie.Value = "GetNameFromUserHERE" + DateTime.Now.ToString();
			yourCookie.Expires = DateTime.Now.AddDays(366);
			yourCookie.Path = "/TikTokCalendar";
			*/

		}
		public void LoadFromCookie(String UserName)
		{
			HttpCookie myCookie = new HttpCookie(UserName);
			
			if (myCookie == null) {
				SaveToCookie(UserName);
			}
		}
	}
}