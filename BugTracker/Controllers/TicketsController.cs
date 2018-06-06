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
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        [Authorize(Roles = "Submitter,Admin,ProjectManager,Developer")]
        public ActionResult Index()
        {
            var tickets = db.tickets.Include(t => t.Assigned).Include(t => t.Owner).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(tickets.ToList());
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter,Admin,ProjectManager,Developer")]
        public ActionResult Create()
        {
            ViewBag.AssignedId = new SelectList(db.Users, "Id", "firstName");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "firstName");
            ViewBag.ProjectID = new SelectList(db.projects, "Id", "name");
            ViewBag.TicketPriorityId = new SelectList(db.ticketPriorities, "Id", "name");
            ViewBag.TicketStatusId = new SelectList(db.ticketStatuses, "Id", "name");
            ViewBag.TicketTypeId = new SelectList(db.ticketTypes, "Id", "name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Submitter,Admin,ProjectManager,Developer")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,title,description,created,updated,ProjectID,TicketTypeId,TicketStatusId,TicketPriorityId,AssignedId,OwnerId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {

                tickets.OwnerId = User.Identity.GetUserId();
                tickets.created = DateTimeOffset.Now;
                tickets.TicketStatusId = 4;

                db.tickets.Add(tickets);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedId = new SelectList(db.Users, "Id", "firstName", tickets.AssignedId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "firstName", tickets.OwnerId);
            ViewBag.ProjectID = new SelectList(db.projects, "Id", "name", tickets.ProjectID);
            ViewBag.TicketPriorityId = new SelectList(db.ticketPriorities, "Id", "name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.ticketStatuses, "Id", "name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.ticketTypes, "Id", "name", tickets.TicketTypeId);
            return View(tickets);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedId = new SelectList(db.Users, "Id", "firstName", tickets.AssignedId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "firstName", tickets.OwnerId);
            ViewBag.ProjectID = new SelectList(db.projects, "Id", "name", tickets.ProjectID);
            ViewBag.TicketPriorityId = new SelectList(db.ticketPriorities, "Id", "name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.ticketStatuses, "Id", "name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.ticketTypes, "Id", "name", tickets.TicketTypeId);
            return View(tickets);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,description,created,updated,ProjectID,TicketTypeId,TicketStatusId,TicketPriorityId,AssignedId,OwnerId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssignedId = new SelectList(db.Users, "Id", "firstName", tickets.AssignedId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "firstName", tickets.OwnerId);
            ViewBag.ProjectID = new SelectList(db.projects, "Id", "name", tickets.ProjectID);
            ViewBag.TicketPriorityId = new SelectList(db.ticketPriorities, "Id", "name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.ticketStatuses, "Id", "name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.ticketTypes, "Id", "name", tickets.TicketTypeId);
            return View(tickets);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tickets tickets = db.tickets.Find(id);
            if (tickets == null)
            {
                return HttpNotFound();
            }
            return View(tickets);
        }

        public ActionResult Personal()
        {
            //var tickets = db.tickets.Where()

            return View();
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Tickets tickets = db.tickets.Find(id);
            db.tickets.Remove(tickets);
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
