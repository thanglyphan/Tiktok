using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DB_EF_Test.DAL;
using DB_EF_Test.Models;

namespace DB_EF_Test.Controllers
{
    public class CalendarEventController : Controller
    {
        private CalendarEventContext db = new CalendarEventContext();

        // GET: CalendarEvent
        public ActionResult Index()
        {
            var calendarEvents = db.CalendarEvents.Include(c => c.Room).Include(c => c.Subject);
            return View(calendarEvents.ToList());
        }

        // GET: CalendarEvent/Details/5
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
            ViewBag.RoomID = new SelectList(db.Rooms, "ID", "Name");
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Name");
            return View();
        }

        // POST: CalendarEvent/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,StartTime,EndTime,SubjectID,RoomID,Attendees,Type,Comment")] CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                db.CalendarEvents.Add(calendarEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.RoomID = new SelectList(db.Rooms, "ID", "Name", calendarEvent.RoomID);
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
            ViewBag.RoomID = new SelectList(db.Rooms, "ID", "Name", calendarEvent.RoomID);
            ViewBag.SubjectID = new SelectList(db.Subjects, "ID", "Name", calendarEvent.SubjectID);
            return View(calendarEvent);
        }

        // POST: CalendarEvent/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,StartTime,EndTime,SubjectID,RoomID,Attendees,Type,Comment")] CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendarEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomID = new SelectList(db.Rooms, "ID", "Name", calendarEvent.RoomID);
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
        }
    }
}
