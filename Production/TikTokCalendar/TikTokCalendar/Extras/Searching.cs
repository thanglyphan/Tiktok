using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TikTokCalendar
{
    public class Searching : Controller
    {
        public JsonResult AutoComplete(string search)
        {
            var data = new[] {"Programmering","Spillprogrammering","Intelligente Systemer","Mobil Apputvikling",
                "Prosjekt software engineering","Matematikk og Fysikk","C++ Programmering","",
                "Game AI","Embedded systems","Mobil utvikling","Ruby on Rails",
                "Avansert Javaprogrammering","Undersøkelsesmetoder","Enterprise programmering 2","PJ3100",
                "RF3100" };
            /*ModelDataWrapper model = new ModelDataWrapper();
            foreach (var month in model.calEvents)
            {
                foreach (var item in month.Events)
                {
                }
            }*/
            var result = data.Where(x => x.ToLower().StartsWith(search.ToLower())).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}