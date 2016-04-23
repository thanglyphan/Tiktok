using System.Collections.Generic;
using System.Web.Mvc;
using TikTokCalendar.DAL;
using TikTokCalendar.Models;

namespace TikTokCalendar.Controllers
{
	public class RoomsController : Controller
	{
		// GET: Rooms
		public ActionResult Index()
		{
			var dp = new DataParser();
			dp.ParseAllData();
			// Make a new ModelDataWrapper with the events based on the user, tags, and filters
			var user = new StudentUser("spillprog2", SchoolCourses.BacheloriIT, "second");
			var modelWrapper = CreateModelDataWrapper(DataWrapper.Instance.GetEventsWithName(user), user);

			// Send the model to the view
			Session["keywords"] = null;
			return View("Rooms", modelWrapper);
		}

		private ModelDataWrapper CreateModelDataWrapper(List<CustomEventMonth> months, StudentUser user)
		{
			var modelWrapper = new ModelDataWrapper();
			modelWrapper.Months = months;
			modelWrapper.User = user;

			// Set availible rooms
			var rooms = new List<Room>();
			foreach (var room in DataWrapper.Instance.Rooms)
			{
				rooms.Add(room.Value);
			}
			modelWrapper.Rooms = rooms;

			return modelWrapper;
		}
	}
}