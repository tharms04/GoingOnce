using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using GoingOnce.Helpers;

namespace GoingOnce.Models
{

    public class BidSheetsReportModel
    {
        public List<AuctionItem> AuctionItems { get; set; }
    }

    public class ItemCatalogModel
    {
        public List<AuctionItem> AuctionItems { get; set; }

        public AuctionEvent AuctionEvent { get; set; }

        public bool IsPublic { get; set; }
    }

    public class BidderItemsModel
    {
        public Bidder Bidder { get; set; }

        [Display(Name = "Total Amount")]
        public decimal TotalAmount {
            get
            {
                return (decimal)AuctionItems.Sum(a => a.AmountBid);
            }
       }

        public List<AuctionItem> AuctionItems { get;  set;}
    }

    public class AuctionItemReportModel
    {
        public List<AuctionItem> AuctionItems { get; set; }
    }

    public class InvoiceModel
    {
        public AuctionEvent AuctionEvent { get; set; }

        public List<BidderItemsModel> WinningBidders { get; set; }

        public bool IsPublic { get; set; }
    }

    public class InvoiceReportModel
    {
        public List<InvoiceModel> Invoices { get; set; }
    }

    public class BidderReportModel
    {
        public List<Bidder> Bidders { get; set; }
    }

    public class ReportSelectionViewModel : IValidatableObject
    {
        public Guid Id { get; set; }

        public ReportSelectionViewModel()
        {
            AvailableEvents = new List<AuctionEvent>();
        }


        [Display(Name = "Event")]
        public Guid SelectedEventId { get; set; }

        public List<AuctionEvent> AvailableEvents { get; set; }

        public ReportType TypeOfReport { get; set; }

        public ReportOutputFormat OutputFormat { get; set; }

        [NotMapped]
        public List<SelectListItem> AvailableEventsSelectList
        {
            get
            {
                return AvailableEvents.Select(c => new SelectListItem { Text = c.EventName, Value = c.Id.ToString() }).ToList();
            }
        }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (SelectedEventId == Guid.Empty)
            {
                yield return
                  new ValidationResult(errorMessage: "Must select an event",
                                       memberNames: new[] { nameof(SelectedEventId) });
            }

        }

        public List<ReportType> AvailableReportTypes
        {
            get {
                return Enum.GetValues(typeof(ReportType)).Cast<ReportType>().ToList();
            }
        }

    }

    public enum ReportOutputFormat
    {
        Html = 1,
        Pdf,
    }

    public enum ReportType
    {
        [Display(Name = "Bid Sheets")]
        BidSheets = 1,

        [Display(Name = "Item Catalog")]
        ItemCatalog,

        [Display(Name = "Invoices")]
        Invoices,

        [Display(Name = "Winning Bids - Private")]
        WinningBidsPrivate,

        [Display(Name = "Winning Bids - Public")]
        WinningBidsPublic,

        [Display(Name = "Bidder Totals - Private")]
        BidderTotalsPrivate,

        [Display(Name = "Bidder Totals - Public")]
        BidderTotalsPublic


    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}