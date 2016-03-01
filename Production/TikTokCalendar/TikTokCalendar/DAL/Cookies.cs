using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TikTokCalendar.DAL
{
	public class Cookies
	{

		public void SaveToCookie(String UserName)
		{
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
			myCookie = Request.Cookies[UserName];
			if (myCookie == null) {
				SaveToCookie(UserName);
			}
		}
	}
}