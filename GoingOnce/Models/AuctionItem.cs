using System;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoingOnce.Models
{
    public class AuctionItem : BaseModel
    {
        // Your context has been configured to use a 'AuctionItemModel' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'GoingOnce.AuctionItemModel' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AuctionItemModel' 
        // connection string in the application configuration file.
        public AuctionItem()
        {
            
        }

        [Display(Name = "Item #")]
        public int? ItemNumber { get; set; }

        [Display(Name = "Item Name")]
        public String ItemName { get; set; }

        [Display(Name = "Description")]
        public String ItemDescription { get; set; }
        
        [Display(Name = "Value")]
        public decimal ItemValue { get; set; }

        [Display(Name = "Starting Bid")]
        public decimal StartBid { get; set; }

        [Display(Name = "Minimum Bid Increment")]
        public decimal BidIncrement { get; set; }

        [Display(Name = "Amount Bid")]
        public decimal? AmountBid { get; set; }

        [Display(Name = "# Bids")]
        public int? NumBids { get; set; }

        [Display(Name = "Type")]
        public AuctionCategory? AuctionType { get; set; }
        
        public Guid? EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual AuctionEvent AuctionEvent { get; set; }

        public Guid? BidderId { get; set; }

        [ForeignKey("BidderId")]
        public virtual Bidder WinningBidder { get; set; }

        
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    public enum AuctionCategory
    {
        Silent = 0,
        Live
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}