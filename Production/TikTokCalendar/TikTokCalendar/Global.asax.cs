using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TikTokCalendar.Controllers;

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
				var isOnMobilePage = path.StartsWith("/Home/Mobile/", StringComparison.OrdinalIgnoreCase);
				if (!isOnMobilePage)
				{
					var redirectTo = "~/Home/Mobile/";
					HttpContext.Current.Response.Redirect(redirectTo);
				}
			}
		}

		protected void Application_AcquireRequestState(object sender, EventArgs e)
		{
			CultureManager.UpdateCulture(Context.Request.RequestContext.HttpContext.Request); // Wow
		}
	}
}