using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DummyDB.Controllers
{
    public class DummyDataController : Controller
    {
        // GET: DummyData
        public ActionResult Index()
        {
            return View();
        }

		public string DummyData()
		{
			InsertDummyData();
			return "Dummy data here";
		}

		void InsertDummyData()
		{

		}
	}
}