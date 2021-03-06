﻿using System.Web.Optimization;

namespace TikTokCalendar
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/script").Include(
				"~/Scripts/script.js"));

			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
				"~/Scripts/jquery-{version}.js",
				"~/Scripts/jquery-ui.js",
				"~/Scripts/jquery.unobtrusive-ajax.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
				"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
				"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
				"~/Scripts/bootstrap.js",
				"~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
				"~/Content/bootstrap.css",
				"~/Content/site.css"));

			// tooltip hover
			bundles.Add(new ScriptBundle("~/bundles/tooltip").Include(
				"~/Scripts/jquery.easing.1.3.js",
				"~/Scripts/jquery.BA.ToolTip.js"));

			// countdown
			bundles.Add(new ScriptBundle("~/bundles/countdown").Include(
				"~/Scripts/jquery.plugin.js",
				"~/Scripts/jquery.countdown.js"));

			// random usortert
			bundles.Add(new ScriptBundle("~/bundles/random").Include(
				"~/Scripts/vex/vex.combined.min.js",
				"~/Scripts/modal.js",
				"~/Scripts/script.js",
				"~/Scripts/mobile.js"
				));

			// showinfo & tooltip
			bundles.Add(new ScriptBundle("~/bundles/showinfo").Include(
				"~/Scripts/showInfo.js"));
		}
	}
}