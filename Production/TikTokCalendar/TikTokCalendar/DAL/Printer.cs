using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TikTokCalendar.DAL
{
	public class Printer
	{
		public static string Test()
		{
			var c = HttpContext.Current;
			var v = c.Request["Test"]; //<-- i can see get data in this
			return v;
		}

		public static void Print(string msg)
		{
			Debug.WriteLine(msg);
		}
	}
}