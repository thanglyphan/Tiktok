using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TikTokCalendar
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
		}

		private void Session_Start(object sender, EventArgs e)
		{
			// Redirect mobile users to the mobile home page
			var httpRequest = HttpContext.Current.Request;
			if (httpRequest.Browser.IsMobileDevice)
			{
				var path = httpRequest.Url.PathAndQuery;
				var isOnMobilePage = path.StartsWith("/Home/Mobile/",
					StringComparison.OrdinalIgnoreCase);
				if (!isOnMobilePage)
				{
					var redirectTo = "~/Home/Mobile/";

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