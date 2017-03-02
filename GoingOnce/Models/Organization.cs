using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GoingOnce.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Organization : BaseModel
    {
        // Your context has been configured to use a 'Organization' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'GoingOnce.Organization' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Organization' 
        // connection string in the application configuration file.
        public Organization()
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public String Name { get; set; }

        [Display(Name ="Street Address")]
        public String StreetAddress { get; set; }

        public String City { get; set; }

        public String State { get; set; }

        public String Country { get; set; }

        public String Zip { get; set; }

        public String Phone { get; set; }

        public virtual ICollection<AuctionEvent> AuctionEvents { get; set; }

    }

}