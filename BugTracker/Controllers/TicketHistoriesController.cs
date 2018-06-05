﻿using System;
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
    public class TicketHistoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketHistories
        public ActionResult Index()
        {
            var ticketHistories = db.ticketHistories.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketHistories.ToList());
        }

        // GET: TicketHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistories ticketHistories = db.ticketHistories.Find(id);
            if (ticketHistories == null)
            {
                return HttpNotFound();
            }
            return View(ticketHistories);
        }

        // GET: TicketHistories/Create
        public ActionResult Create()
        {
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title");
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName");
            return View();
        }

        // POST: TicketHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,property,oldValue,newValue,changed,UserId")] TicketHistories ticketHistories)
        {
            if (ModelState.IsValid)
            {
                db.ticketHistories.Add(ticketHistories);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketHistories.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketHistories.UserId);
            return View(ticketHistories);
        }

        // GET: TicketHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistories ticketHistories = db.ticketHistories.Find(id);
            if (ticketHistories == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketHistories.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketHistories.UserId);
            return View(ticketHistories);
        }

        // POST: TicketHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,property,oldValue,newValue,changed,UserId")] TicketHistories ticketHistories)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketHistories).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketHistories.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketHistories.UserId);
            return View(ticketHistories);
        }

        // GET: TicketHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketHistories ticketHistories = db.ticketHistories.Find(id);
            if (ticketHistories == null)
            {
                return HttpNotFound();
            }
            return View(ticketHistories);
        }

        // POST: TicketHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketHistories ticketHistories = db.ticketHistories.Find(id);
            db.ticketHistories.Remove(ticketHistories);
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
