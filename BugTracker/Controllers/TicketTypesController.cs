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
    [Authorize(Roles = "Admin")]
    public class TicketTypesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketTypes
        public ActionResult Index()
        {
            return View(db.ticketTypes.ToList());
        }

        // GET: TicketTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketTypes ticketTypes = db.ticketTypes.Find(id);
            if (ticketTypes == null)
            {
                return HttpNotFound();
            }
            return View(ticketTypes);
        }

        // GET: TicketTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TicketTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name")] TicketTypes ticketTypes)
        {
            if (ModelState.IsValid)
            {
                db.ticketTypes.Add(ticketTypes);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ticketTypes);
        }

        // GET: TicketTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketTypes ticketTypes = db.ticketTypes.Find(id);
            if (ticketTypes == null)
            {
                return HttpNotFound();
            }
            return View(ticketTypes);
        }

        // POST: TicketTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name")] TicketTypes ticketTypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketTypes).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticketTypes);
        }

        // GET: TicketTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketTypes ticketTypes = db.ticketTypes.Find(id);
            if (ticketTypes == null)
            {
                return HttpNotFound();
            }
            return View(ticketTypes);
        }

        // POST: TicketTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketTypes ticketTypes = db.ticketTypes.Find(id);
            db.ticketTypes.Remove(ticketTypes);
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
