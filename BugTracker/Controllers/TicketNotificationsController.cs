using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class TicketNotificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketNotifications
        public ActionResult Index()
        {
            var ticketNotifications = db.ticketNotifications.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketNotifications.ToList());
        }

        // GET: TicketNotifications/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.ticketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName");
            return View();
        }

        // POST: TicketNotifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,UserId,seen")] TicketNotifications ticketNotifications)
        {
            if (ModelState.IsValid)
            {
                db.ticketNotifications.Add(ticketNotifications);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketNotifications.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketNotifications.UserId);
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.ticketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketNotifications.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketNotifications.UserId);
            return View(ticketNotifications);
        }

        // POST: TicketNotifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,UserId,seen")] TicketNotifications ticketNotifications)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketNotifications).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketNotifications.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketNotifications.UserId);
            return View(ticketNotifications);
        }

        // GET: TicketNotifications/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketNotifications ticketNotifications = db.ticketNotifications.Find(id);
            if (ticketNotifications == null)
            {
                return HttpNotFound();
            }
            return View(ticketNotifications);
        }

        // POST: TicketNotifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketNotifications ticketNotifications = db.ticketNotifications.Find(id);
            db.ticketNotifications.Remove(ticketNotifications);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult _UserNotifications()
        {
            string userId = User.Identity.GetUserId();

            var model = new NotificationViewModel
            {
                notifications = db.ticketNotifications.Where(n => n.UserId == userId).Where(n => n.seen == false).OrderByDescending(n => n.Ticket.created).ToList(),
                currentUser = db.Users.Find(userId)
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult _UserNotifications(int TicketId)
        {
            //TicketNotifications notifications = new TicketNotifications();

            foreach(var item in db.ticketNotifications.Where(n => n.seen == false))
            {
                if(item.TicketId == TicketId)
                {
                    item.seen = true;
                }
            }
            //notifications.seen = true;
            //notifications.Id = Id;
            //notifications.UserId = UserId;
            //notifications.TicketId = TicketId;


            //db.Entry(notifications).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", "Tickets", new { id = TicketId});
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
