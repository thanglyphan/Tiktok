using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace TikTokCalendar.DAL
{
	public class Printer
	{
		public static void Print(string msg)
		{
			Debug.WriteLine(msg);
		}
	}
}