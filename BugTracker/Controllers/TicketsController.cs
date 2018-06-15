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
            return View(tickets.OrderBy(t => t.ProjectID).ToList());
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Details(int? id)
        {
            var userId = User.Identity.GetUserId();
            if(db.tickets.Where(t => t.Id == id).Where(t => t.AssignedId == userId).FirstOrDefault() == null && User.IsInRole("Developer"))
            {
                return RedirectToAction("Index", "Profile");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var model = new TicketDetailsViewModel
            {
                tickets = db.tickets.Find(id),
                user = db.Users.Find(User.Identity.GetUserId()),
                ticketAttachments = db.ticketAttachments.Where(a => a.TicketId == id).ToList(),
                ticketComments = db.ticketComments.Where(c => c.TicketId == id).ToList(),
                ticketHistories = db.ticketHistories.Where(h => h.TicketId == id).ToList()
            };
            if (model.tickets == null || model.user == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter,Admin")]
        public ActionResult Create()
        {
            string UserId = User.Identity.GetUserId();

            ViewBag.AssignedId = new SelectList(db.Users, "Id", "firstName");
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "firstName");
            ViewBag.ProjectID = new SelectList(db.projects.Where(p => p.projectUsers.Select(u => u.Id).Contains(UserId)), "Id", "name");
            ViewBag.TicketPriorityId = new SelectList(db.ticketPriorities, "Id", "name");
            ViewBag.TicketStatusId = new SelectList(db.ticketStatuses, "Id", "name");
            ViewBag.TicketTypeId = new SelectList(db.ticketTypes, "Id", "name");

            if (db.projects.Where(p => p.projectUsers.Select(u => u.Id).Contains(UserId)).FirstOrDefault() == null)
            {
                return RedirectToAction("ErrorMessage", "Tickets");
            }

            return View();
        }

        public ActionResult ErrorMessage()
        {
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Submitter,Admin")]
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
            string UserId = User.Identity.GetUserId();

            ViewBag.AssignedId = new SelectList(db.Users, "Id", "firstName", tickets.AssignedId);
            ViewBag.OwnerId = new SelectList(db.Users, "Id", "firstName", tickets.OwnerId);
            ViewBag.ProjectID = new SelectList(db.projects.Where(p => p.projectUsers.Select(u => u.Id).Contains(UserId)), "Id", "name", tickets.ProjectID);
            ViewBag.TicketPriorityId = new SelectList(db.ticketPriorities, "Id", "name", tickets.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.ticketStatuses, "Id", "name", tickets.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.ticketTypes, "Id", "name", tickets.TicketTypeId);

            if(db.projects.Where(p => p.projectUsers.Select(u => u.Id).Contains(UserId)).FirstOrDefault() == null)
            {
                return RedirectToAction("ErrorMessage", "Tickets");
            }

            return View(tickets);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Edit(int? id)
        {

            var userId = User.Identity.GetUserId();
            if (db.tickets.Where(t => t.Id == id).Where(t => t.AssignedId == userId).FirstOrDefault() == null && User.IsInRole("Developer"))
            {
                return RedirectToAction("Index", "Profile");
            }

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
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,description,created,updated,ProjectID,TicketTypeId,TicketStatusId,TicketPriorityId,AssignedId")] Tickets tickets)
        {
            if (ModelState.IsValid)
            {

                if (User.IsInRole("Developer"))
                {
                    tickets.title = (string)TempData["Title"];
                    tickets.description = (string)TempData["Description"];
                    tickets.AssignedId = (string)TempData["AssignedId"];
                    tickets.TicketPriorityId = (int)TempData["TicketPriorityId"];
                }
                else if (User.IsInRole("Submitter"))
                {
                    tickets.TicketTypeId = (int)TempData["TicketTypeId"];
                    tickets.TicketStatusId = (int)TempData["TicketStatusId"];
                    tickets.AssignedId = (string)TempData["AssignedId"];
                    tickets.TicketPriorityId = (int)TempData["TicketPriorityId"];
                }

                if (User.IsInRole("ProjectManager") || User.IsInRole("Submitter"))
                {
                    if(tickets.AssignedId != (string)TempData["AssignedId"])
                    {
                        TicketNotifications notify = new TicketNotifications();
                        notify.TicketId = tickets.Id;
                        notify.UserId = tickets.AssignedId;
                        notify.seen = false;
                        notify.notification = "You have been assigned to ticket " + (string)TempData["Title"];
                        db.ticketNotifications.Add(notify);
                    }
                    else
                    {
                        TicketNotifications notify = new TicketNotifications();
                        notify.TicketId = tickets.Id;
                        notify.UserId = tickets.AssignedId;
                        notify.seen = false;
                        notify.notification = (string)TempData["Title"] + " has been modified";
                        db.ticketNotifications.Add(notify);
                    }
                }

                tickets.ModifierId = User.Identity.GetUserId();
                tickets.updated = DateTimeOffset.Now;
                tickets.created = (DateTimeOffset)TempData["CreatedDate"];
                tickets.ProjectID = (int)TempData["ProjectId"];
                tickets.OwnerId = (string)TempData["OwnerId"];
                db.Entry(tickets).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Personal");
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

        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult Personal()
        {
            var userId = User.Identity.GetUserId();
            var tickets = db.tickets.ToList();

            if (User.IsInRole("ProjectManager") || User.IsInRole("Admin"))
            {
                tickets = db.tickets.Where(t => t.Project.projectUsers.Select(p => p.Id).Contains(userId)).ToList();
            }
            else if (User.IsInRole("Developer"))
            {
                tickets = db.tickets.Where(t => t.AssignedId == userId).ToList();
            }
            else if (User.IsInRole("Submitter"))
            {
                tickets = db.tickets.Where(t => t.OwnerId == userId).ToList();
            }

            return View(tickets.OrderBy(t => t.ProjectID));
        }

        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        public ActionResult DevProjectTickets()
        {
            var userId = User.Identity.GetUserId();
            var tickets = db.tickets.Where(t => t.Project.projectUsers.Select(p => p.Id).Contains(userId)).ToList();

            return View(tickets);
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
