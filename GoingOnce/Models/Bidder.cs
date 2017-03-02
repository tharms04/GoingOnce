using System;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoingOnce.Models
{

    public class Bidder : BaseModel
    {
        // Your context has been configured to use a 'Bidder' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'GoingOnce.Bidder' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Bidder' 
        // connection string in the application configuration file.
        public Bidder()
        {
        }

        public String Name { get; set; }

        [Display(Name = "Phone #")]
        public String Phone { get; set; }

        [Display(Name = "Paddle #")]
        public int Paddle { get; set; }

        public Guid? EventId { get; set; }

        [ForeignKey("EventId")]
        public virtual AuctionEvent AuctionEvent { get; set; }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}