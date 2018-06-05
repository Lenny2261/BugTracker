using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;

namespace BugTracker.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        public ActionResult Index()
        {
            var ticketAttachments = db.ticketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.ticketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,filePath,description,created,UserId,fileURL")] TicketAttachments ticketAttachments)
        {
            if (ModelState.IsValid)
            {
                db.ticketAttachments.Add(ticketAttachments);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.ticketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,filePath,description,created,UserId,fileURL")] TicketAttachments ticketAttachments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachments ticketAttachments = db.ticketAttachments.Find(id);
            if (ticketAttachments == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachments);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachments ticketAttachments = db.ticketAttachments.Find(id);
            db.ticketAttachments.Remove(ticketAttachments);
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
