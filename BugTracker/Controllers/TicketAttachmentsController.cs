using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Helpers;
using BugTracker.Models;
using Microsoft.AspNet.Identity;

namespace BugTracker.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var ticketAttachments = db.ticketAttachments.Include(t => t.Ticket).Include(t => t.User);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
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
        [Authorize(Roles = "Admin,ProjectManager,Developer,Submitter")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,description")] TicketAttachments ticketAttachments, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                FileUploadValidator helper = new FileUploadValidator();

                if (helper.IsWebpageFrendlyFile(file))
                {
                    var extention = Path.GetExtension(file.FileName);
                    var fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath("~/UploadedFiles/"), fileName));
                    ticketAttachments.fileURL = "/UploadedFiles/" + fileName;
                    ticketAttachments.filePath = Path.GetFileNameWithoutExtension(file.FileName);

                    if (extention == ".doc" || extention == ".docx")
                        ticketAttachments.icon = "/Attachments/word.png";
                    else if (extention == ".xls" || extention == ".xlsx")
                        ticketAttachments.icon = "/Attachments/excel.png";
                    else if (extention == ".pdf")
                        ticketAttachments.icon = "/Attachments/pdf.png";
                    else if (extention == ".png" || extention == ".jpg" || extention == ".jpeg")
                        ticketAttachments.icon = "/Attachments/image.png";
                    else
                        ticketAttachments.icon = "/Attachments/default.png";

                }
                else
                {
                    TempData["attachCheck"] = "Failure";
                    return RedirectToAction("Details", "Tickets", new { id = ticketAttachments.TicketId });
                }

                if(User.IsInRole("ProjectManager") || User.IsInRole("Submitter"))
                {
                    TicketNotifications notify = new TicketNotifications();
                    notify.TicketId = ticketAttachments.TicketId;
                    notify.UserId = (string)TempData["assignedId"];
                    notify.seen = false;
                    notify.notification = (string)TempData["Title"] + " has a new attachment";
                    db.ticketNotifications.Add(notify);
                }

                ticketAttachments.UserId = User.Identity.GetUserId();
                ticketAttachments.created = DateTimeOffset.Now;
                db.ticketAttachments.Add(ticketAttachments);
                db.SaveChanges();
                TempData["attachCheck"] = "Success";
                return RedirectToAction("Details", "Tickets", new { id = ticketAttachments.TicketId });
            }

            TempData["attachCheck"] = "Failure";
            ViewBag.TicketId = new SelectList(db.tickets, "Id", "title", ticketAttachments.TicketId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "firstName", ticketAttachments.UserId);
            return View(ticketAttachments);
        }

        // GET: TicketAttachments/Edit/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin, ProjectManager")]
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
        [HttpPost, ActionName("Delete, ProjectManager")]
        [Authorize(Roles = "Admin")]
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
