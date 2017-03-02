using System;
using System.Data.Entity;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoingOnce.Models
{
    
    public class AuctionEvent : BaseModel
    {
        // Your context has been configured to use a 'AuctionEvent' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'GoingOnce.AuctionEvent' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'AuctionEvent' 
        // connection string in the application configuration file.
        public AuctionEvent()
        {
           
        }

        [Display(Name = "Event Name")]
        public String EventName { get; set; }

        [Display(Name = "Event Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EventDate { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }
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