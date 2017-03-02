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

    public class DashboardModel
    {
        public List<AuctionEventSummary> Auctions { get; set; }
    }

    public class AuctionEventSummary
    {
        public AuctionEvent AuctionEvent { get; set; }

        public List<AuctionItem> AuctionItems { get; set; }

        public List<Bidder> Bidders { get; set; }

        [Display(Name ="# of Bidders")]
        public int BidderCount
        {
            get
            {
                return Bidders.Count();
            }
        }

        [Display(Name = "Avg Spend")]
        public decimal AvgSpend
        {
            get
            {
                if (BidderCount == 0)
                {
                    return 0;
                }

                return AuctionTotal / BidderCount;
            }
        }

        [Display (Name = "Live Auction Total")]
        public decimal LiveItemTotal
        {
            get
            {
                return (decimal)AuctionItems.Where(a => a.AuctionType == AuctionCategory.Live).Sum(b => b.AmountBid);
            }
        }

        [Display (Name = "Silent Auction Total")]
        public decimal SilentItemTotal
        {
            get
            {
                return (decimal)AuctionItems.Where(a => a.AuctionType == AuctionCategory.Silent).Sum(b => b.AmountBid);
            }
        }

        [Display(Name = "Auction Total")]
        public decimal AuctionTotal
        {
            get
            {
                return SilentItemTotal + LiveItemTotal;
            }
        }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}