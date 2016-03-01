using System;
using System.Web;

namespace TikTokCalendar.DAL
{
	public class Cookies
	{

		public void SaveToCookie(String UserName, String Program)
		{
			HttpCookie myCookie = new HttpCookie(UserName);
			myCookie.Value = Program;
			myCookie.Expires = DateTime.Now.AddHours(1);

			HttpContext.Current.Response.Cookies.Add(myCookie);

		}
		public String LoadFromCookie(String UserName, String Program)
		{
			HttpCookie myCookie = HttpContext.Current.Request.Cookies[UserName];
			
			if (myCookie == null) {
				SaveToCookie(UserName, Program);
				return null;
			}
			else {
				return UserName + " " + Program;
			}
		}
	}
}