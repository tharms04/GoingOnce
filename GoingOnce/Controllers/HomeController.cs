using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GoingOnce.Models;
using GoingOnce.Helpers;

namespace GoingOnce.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var orgId = User.Identity.GetUserOrgId();

            DashboardModel dash = new DashboardModel();
            dash.Auctions = new List<AuctionEventSummary>();

            var auctionList = db.AuctionEvents.Where(x => x.OrganizationId == orgId).OrderByDescending(x=> x.EventDate).ToList();

            foreach (var a in auctionList)
            {
                AuctionEventSummary summary = new AuctionEventSummary();
                summary.AuctionEvent = a;
                summary.AuctionItems = db.AuctionItem.Where(x => x.EventId == a.Id).ToList();
                summary.Bidders = db.Bidders.Where(x => x.EventId == a.Id).ToList();
                dash.Auctions.Add(summary);
            }

            return View(dash);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}