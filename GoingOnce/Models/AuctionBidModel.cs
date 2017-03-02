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
using GoingOnce.Helpers;

namespace GoingOnce.Models
{

    [NotMapped]
    public class AuctionBidModel : IValidatableObject
    {
        public Guid Id { get; set; }

        public AuctionBidModel() {
            AuctionItemList = new List<AuctionItem>();
            BidderList = new List<Bidder>();
        }
        
        public int ItemNumber { get; set; }

        public int NumBids { get; set; }

        public decimal AmountBid { get; set; }
        
        public int PaddleNumber { get; set; }

        public List<AuctionItem> AuctionItemList { get; set; }

        public List<Bidder> BidderList { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ItemNumber < 0)
            {
                yield return
                  new ValidationResult(errorMessage: "Item number must be greater than zero",
                                       memberNames: new[] { nameof(ItemNumber) });
            }

            if (AmountBid < 0)
            {
                yield return
                  new ValidationResult(errorMessage: "Amount bid must be greater than zero",
                                       memberNames: new[] { nameof(AmountBid) });
            }

            if (PaddleNumber < 0)
            {
                yield return
                    new ValidationResult(errorMessage: "Paddle number must be greater than zero",
                                        memberNames: new[] { nameof(PaddleNumber ) });
            }
        }
    }

}