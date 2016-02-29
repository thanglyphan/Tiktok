using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TikTokCalendar.DAL;
using TikTokCalendar.Models;

namespace TikTokCalendar.Controllers
{
    public class CalendarEventController : Controller
    {
        private CalendarEventContext db = new CalendarEventContext();

        // GET: CalendarEvent
        public ActionResult Index()
        {
			return View();
			Account acc = db.Accounts.Find(new Random().Next(2, 4)); // TODO Replace with the currently logged in account
			ViewBag.Title = "User: " + acc.ID.ToString();

			var calendarEvents = new List<CalendarEvent>();// = db.CalendarEvents.Include(c => c.Subject);
			var events = db.CalendarEvents.ToList();

			// Go through all course subjects
			foreach (var item in db.CourseSubject)
			{
				// Check if the coursesubject has the same ID and semester as the user
				if (item.CourseID == acc.CourseID && item.Semester == acc.SemesterID)
				{
					// Go through all events
					foreach (var calEvent in events)
					{
						// Add the event if the events subjectID mathces the courseubjects subjectID
						if (calEvent.SubjectID == item.SubjectID)
						{
							calendarEvents.Add(calEvent);
						}
					}
				}
			}
            return View(calendarEvents.ToList());
			/*
			 * Make a class for the calendar view?
			 *		With a List of months that each have a list with events for that month (List<List<CalendarEvents>> events)
			 * foreach (month)
			 *		display month header
			 *			foreach(events in month)
			 *				display event
			 */
		}

		/*// GET: CalendarEvent/Details/5
		public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarEvent calendarEvent = db.CalendarEvents.Find(id);
            if (calendarEvent == null)
            {
                return HttpNotFound();
            }
            return View(calendarEvent);
        }

        // GET: CalendarEvent/Create
        public ActionResult Create()
        {
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Name");
            return View();
        }

        // POST: CalendarEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,SubjectID,StartTime,EndTime,RoomName,EventName,Attendees,Teacher,Comment")] CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                db.CalendarEvents.Add(calendarEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Name", calendarEvent.SubjectID);
            return View(calendarEvent);
        }

        // GET: CalendarEvent/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarEvent calendarEvent = db.CalendarEvents.Find(id);
            if (calendarEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Name", calendarEvent.SubjectID);
            return View(calendarEvent);
        }

        // POST: CalendarEvent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,SubjectID,StartTime,EndTime,RoomName,EventName,Attendees,Teacher,Comment")] CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendarEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Name", calendarEvent.SubjectID);
            return View(calendarEvent);
        }

        // GET: CalendarEvent/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarEvent calendarEvent = db.CalendarEvents.Find(id);
            if (calendarEvent == null)
            {
                return HttpNotFound();
            }
            return View(calendarEvent);
        }

        // POST: CalendarEvent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CalendarEvent calendarEvent = db.CalendarEvents.Find(id);
            db.CalendarEvents.Remove(calendarEvent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/
    }
}
