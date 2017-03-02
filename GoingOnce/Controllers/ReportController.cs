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
    public class ReportController : Controller
    {
        private static string ReportVmTempKey = "ReportVmTemp";
        private static string OrgIdTempKey = "OrgIdTemp";

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Report
        public ActionResult Index()
        {
            var orgId = User.Identity.GetUserOrgId();
            if (orgId == null)
                return RedirectToAction("Create", "Organizations");

            var viewmodel = new ReportSelectionViewModel();
            viewmodel.AvailableEvents = db.AuctionEvents.Where(x => x.OrganizationId == orgId).OrderByDescending(x => x.EventDate).ToList();

            return View(viewmodel);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HtmlReport(ReportSelectionViewModel reportSelectorVm)
        {
            var orgId = User.Identity.GetUserOrgId();
            if (orgId == null)
                return new HttpNotFoundResult();

            reportSelectorVm.AvailableEvents = db.AuctionEvents.Where(x => x.OrganizationId == orgId).OrderByDescending(x => x.EventDate).ToList();

            if (ModelState.IsValid)
            {
                reportSelectorVm.OutputFormat = ReportOutputFormat.Html;
                TempData[ReportVmTempKey] = reportSelectorVm;
                TempData[OrgIdTempKey] = orgId;

                return RedirectToReport(orgId.Value, reportSelectorVm);
            }

            return View("Index", reportSelectorVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PdfReport(ReportSelectionViewModel reportVm)
        {
            var orgId = User.Identity.GetUserOrgId();
            if (orgId == null)
                return new HttpNotFoundResult();

            //Required for PDF printing
            reportVm.AvailableEvents = db.AuctionEvents.Where(x => x.OrganizationId == orgId).OrderByDescending(x => x.EventDate).ToList();

            if (ModelState.IsValid)
            {
                reportVm.OutputFormat = ReportOutputFormat.Pdf;
                TempData[ReportVmTempKey] = reportVm;
                TempData[OrgIdTempKey] = orgId;

                return RedirectToReport(orgId.Value, reportVm);
            }

            return View(reportVm);
        }

        private ActionResult RedirectToReport(Guid orgId, ReportSelectionViewModel reportSelectorVm)
        {
            var auctionEventId = reportSelectorVm.SelectedEventId;
            
            switch (reportSelectorVm.TypeOfReport)
            {
                case ReportType.BidSheets:
                    return BidSheets(orgId, reportSelectorVm, auctionEventId);
                case ReportType.ItemCatalog:
                    return ItemCatalog(orgId, reportSelectorVm, auctionEventId);
                case ReportType.Invoices:
                    return Invoices(orgId, reportSelectorVm, auctionEventId);
                case ReportType.WinningBidsPrivate:
                    return WinningBiddersPrivate(orgId, reportSelectorVm, auctionEventId);
                case ReportType.WinningBidsPublic:
                    return WinningBiddersPublic(orgId, reportSelectorVm, auctionEventId);
                case ReportType.BidderTotalsPrivate:
                    return BidderTotalsPrivate(orgId, reportSelectorVm, auctionEventId);
                case ReportType.BidderTotalsPublic:
                    return BidderTotalsPublic(orgId, reportSelectorVm, auctionEventId);
                default:
                    return RedirectToAction(nameof(Index));

            }
        }

        ActionResult BidSheets(Guid orgId, ReportSelectionViewModel reportVm, Guid eventId)
        {
            var result = new BidSheetsReportModel();
                
            result.AuctionItems = db.AuctionItem.Where(a => a.EventId == eventId && a.AuctionType == AuctionCategory.Silent).ToList();

            return View(nameof(BidSheets), result);
        }

        ActionResult ItemCatalog(Guid orgId, ReportSelectionViewModel reportVm, Guid eventId)
        {
            var result = new ItemCatalogModel();

            result.AuctionItems = db.AuctionItem.Where(a => a.EventId == eventId).OrderByDescending(a => a.AuctionType).ThenBy(a => a.ItemNumber).ToList();
            result.AuctionEvent = db.AuctionEvents.First(a => a.Id == eventId);
            result.IsPublic = true;

            return View(nameof(ItemCatalog), result);
        }


        ActionResult WinningBiddersPublic(Guid orgId, ReportSelectionViewModel reportVm, Guid eventId)
        {
            var result = new ItemCatalogModel();

            result.AuctionItems = db.AuctionItem.Where(a => a.EventId == eventId).OrderBy(a => a.ItemNumber).ToList();
            result.AuctionEvent = db.AuctionEvents.First(a => a.Id == eventId);
            result.IsPublic = true;

            return View("WinningBids", result);
        }

        ActionResult WinningBiddersPrivate(Guid orgId, ReportSelectionViewModel reportVm, Guid eventId)
        {
            var result = new ItemCatalogModel();

            result.AuctionItems = db.AuctionItem.Where(a => a.EventId == eventId).OrderBy(a => a.ItemNumber).ToList();
            result.AuctionEvent = db.AuctionEvents.First(a => a.Id == eventId);
            result.IsPublic = false;

            return View("WinningBids", result);
        }

        ActionResult Invoices(Guid orgId, ReportSelectionViewModel reportVm, Guid eventId)
        {
            InvoiceModel result = GenerateInvoiceModel(orgId, eventId, false);
            result.IsPublic = false;

            return View(nameof(Invoices), result);
        }

        ActionResult BidderTotalsPublic(Guid orgId, ReportSelectionViewModel reportVm, Guid eventId)
        {
            InvoiceModel result = GenerateInvoiceModel(orgId, eventId, true);
            result.IsPublic = true;

            return View("PaddleTotals", result);
        }

        ActionResult BidderTotalsPrivate(Guid orgId, ReportSelectionViewModel reportVm, Guid eventId)
        {
            InvoiceModel result = GenerateInvoiceModel(orgId, eventId, true);
            result.IsPublic = false;

            return View("PaddleTotals", result);
        }

        InvoiceModel GenerateInvoiceModel(Guid orgId, Guid eventId, bool bIncludeZeroBalances)
        {
            var result = new InvoiceModel();

            result.AuctionEvent = db.AuctionEvents.First(a => a.Id == eventId);
            result.WinningBidders = new List<BidderItemsModel>();

            var auctionBidders = db.Bidders.Where(a => a.EventId == eventId).ToList();
            foreach(var bidder in auctionBidders)
            {
                var wonItems = db.AuctionItem.Where(a => a.EventId == eventId && a.BidderId == bidder.Id);
                if (wonItems.Count() > 0 || bIncludeZeroBalances)
                {
                    var winningBidder = new BidderItemsModel()
                    {
                        AuctionItems = wonItems.ToList(),
                        Bidder = bidder
                    };
                    result.WinningBidders.Add(winningBidder);
                }
            }

            return result;
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
