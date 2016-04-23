using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;

namespace TikTokCalendar.Controllers
{
	public class CultureManager
	{
		private const string DefaultLang = "nb";
		private const string LanguageCookieKey = "_language";

		public static void SaveCulture(HttpResponseBase response, string language, int dayExpireCount)
		{
			var cookie = new HttpCookie(LanguageCookieKey) { Expires = DateTime.Now.AddDays(dayExpireCount) };
			cookie.Value = language;
			response.Cookies.Add(cookie);
		}

		public static string GetSavedCultureOrDefault(HttpRequestBase httpRequestBase)
		{
			var culture = DefaultLang;
			var cookie = httpRequestBase.Cookies[LanguageCookieKey];
			if (cookie != null && !string.IsNullOrEmpty(cookie.Value))
				culture = cookie.Value;

			return culture;
		}

		public static void UpdateCulture(HttpRequestBase httpRequestBase)
		{
			var cultureString = GetSavedCultureOrDefault(httpRequestBase);
			if (!string.IsNullOrEmpty(cultureString))
			{
				var cultureInfo = CultureInfo.CreateSpecificCulture(cultureString);
				Thread.CurrentThread.CurrentCulture = cultureInfo;
				Thread.CurrentThread.CurrentUICulture = cultureInfo;
			}
		}
	}
}