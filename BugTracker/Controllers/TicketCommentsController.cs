﻿using System;
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
    public class TicketCommentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketComments
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var ticketComments = db.ticketComments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketComments.ToList());
        }

        // GET: TicketComments/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.ticketComments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            return View(ticketComments);
        }

        // GET: TicketComments/Create
        [Authorize(Roles = "Admin,ProjectManager,Submitter,Developer")]
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName");
            return View();
        }

        // POST: TicketComments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager,Submitter,Developer")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,body")] TicketComments ticketComments)
        {
            if (ModelState.IsValid)
            {
                ticketComments.UserId = User.Identity.GetUserId();
                ticketComments.created = DateTimeOffset.Now;

                db.ticketComments.Add(ticketComments);
                db.SaveChanges();
                return RedirectToAction("Details", "Tickets", new { id = ticketComments.TicketId });
            }

            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketComments.UserId);
            return View(ticketComments);
        }

        // GET: TicketComments/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.ticketComments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketComments.UserId);
            return View(ticketComments);
        }

        // POST: TicketComments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,body,created,UserId")] TicketComments ticketComments)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketComments).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketComments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketComments.UserId);
            return View(ticketComments);
        }

        // GET: TicketComments/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketComments ticketComments = db.ticketComments.Find(id);
            if (ticketComments == null)
            {
                return HttpNotFound();
            }
            return View(ticketComments);
        }

        // POST: TicketComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketComments ticketComments = db.ticketComments.Find(id);
            db.ticketComments.Remove(ticketComments);
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
