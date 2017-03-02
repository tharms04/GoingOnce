using AutoMapper;
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
    [Authorize]
    public class AuctionItemController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AuctionItem
        public ActionResult Index(string sortOrder)
        {
            Guid? eventId = User.Identity.GetEventId();
            var auctionItems = db.AuctionItem.Where(x => x.EventId == eventId).OrderBy(x => x.ItemNumber);
            return View(auctionItems.ToList());
        }

        // GET: AuctionItem/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionItem auctionItemModel = db.AuctionItem.Find(id);
            if (auctionItemModel == null)
            {
                return HttpNotFound();
            }
            return View(auctionItemModel);
        }

        // GET: AuctionItem/Create
        public ActionResult Create()
        {
            ViewBag.EventId = new SelectList(db.AuctionEvents, "Id", "EventName");
            return View();
        }

        // POST: AuctionItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemNumber,ItemName,ItemDescription,ItemValue,StartBid,AmountBid,NumBids,BidIncrement,EventId,AuctionType")] AuctionItem auctionItemModel)
        {
            if (ModelState.IsValid)
            {
                var eventId = User.Identity.GetEventId();
                auctionItemModel.EventId = eventId;
                auctionItemModel.Id = Guid.NewGuid();
                db.AuctionItem.Add(auctionItemModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EventId = new SelectList(db.AuctionEvents, "Id", "EventName", auctionItemModel.EventId);
            return View(auctionItemModel);
        }

        // GET: AuctionItem/EnterBidInfo
        public ActionResult EnterBidInfo()
        {
            Guid? eventId = User.Identity.GetEventId();
            var BidModel = CreateAuctionBidModel(db, eventId);
            return View(BidModel);
        }

        // POST: AuctionItem/EnterBidInfo
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EnterBidInfo([Bind(Include = "Id,ItemNumber,AmountBid,NumBids,PaddleNumber")] AuctionBidModel auctionBid)
        {
            if (ModelState.IsValid)
            {
                var eventId = User.Identity.GetEventId();
                var bidder = db.Bidders.First(a => a.Paddle == auctionBid.PaddleNumber && a.EventId == eventId);
                var auctionItem = db.AuctionItem.First(a => a.ItemNumber == auctionBid.ItemNumber && a.EventId == eventId);
                
                auctionItem.NumBids = auctionBid.NumBids;
                auctionItem.AmountBid = auctionBid.AmountBid;
                auctionItem.WinningBidder = bidder;
                db.SaveChanges();
                return RedirectToAction("EnterBidInfo");
            }

            return View(auctionBid);
        }


        // GET: AuctionItem/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionItem auctionItemModel = db.AuctionItem.Find(id);
            if (auctionItemModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.EventId = new SelectList(db.AuctionEvents, "Id", "EventName", auctionItemModel.EventId);
            return View(auctionItemModel);
        }

        // POST: AuctionItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemNumber,ItemName,ItemDescription,ItemValue,StartBid,AmountBid,NumBids,BidIncrement,EventId,AuctionType")] AuctionItem auctionItemModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auctionItemModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EventId = new SelectList(db.AuctionEvents, "Id", "Name", auctionItemModel.EventId);
            return View(auctionItemModel);
        }

        // GET: AuctionItem/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AuctionItem auctionItemModel = db.AuctionItem.Find(id);
            if (auctionItemModel == null)
            {
                return HttpNotFound();
            }
            return View(auctionItemModel);
        }

        // POST: AuctionItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            AuctionItem auctionItemModel = db.AuctionItem.Find(id);
            db.AuctionItem.Remove(auctionItemModel);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        AuctionBidModel CreateAuctionBidModel(ApplicationDbContext DbContext, Guid? EventId)
        {
            AuctionBidModel abm = new GoingOnce.Models.AuctionBidModel();
            abm.AuctionItemList = DbContext.AuctionItem.Where(a => a.EventId == EventId).ToList();
            abm.BidderList = DbContext.Bidders.Where(a => a.EventId == EventId).ToList();
            return abm;
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
