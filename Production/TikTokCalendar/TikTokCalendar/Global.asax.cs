using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TikTokCalendar
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		void Session_Start(object sender, EventArgs e)
		{
			// Redirect mobile users to the mobile home page
			HttpRequest httpRequest = HttpContext.Current.Request;
			if (httpRequest.Browser.IsMobileDevice)
			{
				string path = httpRequest.Url.PathAndQuery;
				bool isOnMobilePage = path.StartsWith("/Home/Mobile/",
									   StringComparison.OrdinalIgnoreCase);
				if (!isOnMobilePage)
				{
					string redirectTo = "~/Home/Mobile/";

					// Could also add special logic to redirect from certain 
					// recognized pages to the mobile equivalents of those 
					// pages (where they exist). For example,
					// if (HttpContext.Current.Handler is UserRegistration)
					//     redirectTo = "~/Mobile/Register.aspx";

					HttpContext.Current.Response.Redirect(redirectTo);
				}
			}
		}
	}
}
