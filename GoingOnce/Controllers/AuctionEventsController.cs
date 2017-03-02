using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GoingOnce.Models;
using GoingOnce.Helpers;

namespace GoingOnce.Controllers
{
    public class AuctionEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AuctionEvents
        public ActionResult Index()
        {
            var auctionEvents = db.AuctionEvents.Include(a => a.Organization);
            return View(auctionEvents.ToList());
        }

        // GET: AuctionEvents/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionEvent auctionEvent = db.AuctionEvents.Find(id);
            if (auctionEvent == null)
            {
                return HttpNotFound();
            }
            return View(auctionEvent);
        }

        // GET: AuctionEvents/Create
        public ActionResult Create()
        {
            var orgId = User.Identity.GetUserOrgId();
            if (orgId == null)
                return RedirectToAction("Create", "Organizations");

            ViewBag.OrganizationId = new SelectList(db.Organizations, "Id", "Name");
            return View();
        }

        // POST: AuctionEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,EventName,EventDate,OrganizationId")] AuctionEvent auctionEvent)
        {
            if (ModelState.IsValid)
            {
                auctionEvent.Id = Guid.NewGuid();
                db.AuctionEvents.Add(auctionEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OrganizationId = new SelectList(db.Organizations, "Id", "Name", auctionEvent.OrganizationId);
            return View(auctionEvent);
        }

        // GET: AuctionEvents/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionEvent auctionEvent = db.AuctionEvents.Find(id);
            if (auctionEvent == null)
            {
                return HttpNotFound();
            }
            ViewBag.OrganizationId = new SelectList(db.Organizations, "Id", "Name", auctionEvent.OrganizationId);
            return View(auctionEvent);
        }

        // POST: AuctionEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,EventName,EventDate,OrganizationId")] AuctionEvent auctionEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auctionEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OrganizationId = new SelectList(db.Organizations, "Id", "Name", auctionEvent.OrganizationId);
            return View(auctionEvent);
        }

        // GET: AuctionEvents/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionEvent auctionEvent = db.AuctionEvents.Find(id);
            if (auctionEvent == null)
            {
                return HttpNotFound();
            }
            return View(auctionEvent);
        }

        // POST: AuctionEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AuctionEvent auctionEvent = db.AuctionEvents.Find(id);
            db.AuctionEvents.Remove(auctionEvent);
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
