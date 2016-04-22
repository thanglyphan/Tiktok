using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TikTokCalendar.Models;
using TikTokCalendar.DAL;

namespace TikTokCalendar.Controllers
{
    public class RoomsController : Controller
    {
        // GET: Rooms
        public ActionResult Index()
        {
			DataParser dp = new DataParser();
			dp.ParseAllData();
			// Make a new ModelDataWrapper with the events based on the user, tags, and filters
			StudentUser user = new StudentUser("spillprog2", SchoolCourses.BacheloriIT, "second");
			ModelDataWrapper modelWrapper = CreateModelDataWrapper(DataWrapper.Instance.GetEventsWithName(user), user);

			// Send the model to the view
			Session["keywords"] = null;
			return View("Rooms", modelWrapper);
		}

		private ModelDataWrapper CreateModelDataWrapper(List<CustomEventMonth> months, StudentUser user) {
			ModelDataWrapper modelWrapper = new ModelDataWrapper();
			modelWrapper.Months = months;
			modelWrapper.User = user;

			// Set availible rooms
			List<Room> rooms = new List<Room>();
			foreach (var room in DataWrapper.Instance.Rooms) {
				rooms.Add(room.Value);
			}
			modelWrapper.Rooms = rooms;

			return modelWrapper;
		}
	}
}