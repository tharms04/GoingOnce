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

namespace GoingOnce
{
    public class BiddersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Bidders
        public ActionResult Index()
        {
            Guid? eventId = User.Identity.GetEventId();
            if (eventId == null)
            {
                return RedirectToAction("Create", "AuctionEvents");
            }

            var bidders = db.Bidders.Where(x => x.EventId == eventId).OrderBy(x => x.Paddle);
            return View(bidders.ToList());
        }

        // GET: Bidders/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bidder bidder = db.Bidders.Find(id);
            if (bidder == null)
            {
                return HttpNotFound();
            }
            return View(bidder);
        }

        // GET: Bidders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Bidders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Phone,Paddle")] Bidder bidder)
        {
            if (ModelState.IsValid)
            {
                var eventId = User.Identity.GetEventId();
                bidder.Id = Guid.NewGuid();
                bidder.EventId = eventId;
                db.Bidders.Add(bidder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventId = new SelectList(db.AuctionEvents, "Id", "EventName", bidder.EventId);
            return View(bidder);
        }

        // GET: Bidders/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bidder bidder = db.Bidders.Find(id);
            if (bidder == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(db.AuctionEvents, "Id", "EventName", bidder.EventId);
            return View(bidder);
        }

        // POST: Bidders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Phone,Paddle,EventId")] Bidder bidder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bidder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bidder);
        }

        // GET: Bidders/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bidder bidder = db.Bidders.Find(id);
            if (bidder == null)
            {
                return HttpNotFound();
            }
            return View(bidder);
        }

        // POST: Bidders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Bidder bidder = db.Bidders.Find(id);
            db.Bidders.Remove(bidder);
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
