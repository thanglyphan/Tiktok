using System.IdentityModel.Services;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using TikTokCalendar.App_Start;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(SessionAuthenticationConfig), "PreAppStart")]

namespace TikTokCalendar.App_Start
{
	public static class SessionAuthenticationConfig
	{
		public static void PreAppStart()
		{
			DynamicModuleUtility.RegisterModule(typeof(SessionAuthenticationModule));
		}
	}
}