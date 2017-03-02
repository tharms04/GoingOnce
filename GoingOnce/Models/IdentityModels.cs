using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoingOnce.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            LockoutEnabled = true;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("OrganizationId", OrganizationId.ToString()));
            if (Organization != null)
            {
                userIdentity.AddClaim(new Claim("OrganizationName", Organization.Name));
                foreach (AuctionEvent AE in Organization.AuctionEvents)
                {
                    if (AE.IsActive)
                    {
                        userIdentity.AddClaim(new Claim("EventId", AE.Id.ToString()));
                    }
                }
            }

            return userIdentity;
        }

        [Display(Name = "Name")]
        public string Name { get; set; }

        public Guid? OrganizationId { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<GoingOnce.Models.AuctionItem> AuctionItem { get; set; }

        public System.Data.Entity.DbSet<GoingOnce.Models.Organization> Organizations { get; set; }

        public System.Data.Entity.DbSet<GoingOnce.Models.AuctionEvent> AuctionEvents { get; set; }

        public System.Data.Entity.DbSet<GoingOnce.Models.Bidder> Bidders { get; set; }

        public System.Data.Entity.DbSet<GoingOnce.Models.ReportSelectionViewModel> ReportSelectionViewModels { get; set; }
    }
}