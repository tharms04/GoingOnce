namespace GoingOnce.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuctionEvents",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        EventName = c.String(),
                        EventDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        OrganizationId = c.Guid(),
                        DateCreatedUtc = c.DateTime(),
                        UserCreated = c.String(),
                        DateModifiedUtc = c.DateTime(),
                        UserModified = c.String(),
                        ReportSelectionViewModel_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId)
                .ForeignKey("dbo.ReportSelectionViewModels", t => t.ReportSelectionViewModel_Id)
                .Index(t => t.OrganizationId)
                .Index(t => t.ReportSelectionViewModel_Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        StreetAddress = c.String(),
                        City = c.String(),
                        State = c.String(),
                        Country = c.String(),
                        Zip = c.String(),
                        Phone = c.String(),
                        DateCreatedUtc = c.DateTime(),
                        UserCreated = c.String(),
                        DateModifiedUtc = c.DateTime(),
                        UserModified = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AuctionItems",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        ItemNumber = c.Int(),
                        ItemName = c.String(),
                        ItemDescription = c.String(),
                        ItemValue = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StartBid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        BidIncrement = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountBid = c.Decimal(precision: 18, scale: 2),
                        NumBids = c.Int(),
                        AuctionType = c.Int(),
                        EventId = c.Guid(),
                        BidderId = c.Guid(),
                        DateCreatedUtc = c.DateTime(),
                        UserCreated = c.String(),
                        DateModifiedUtc = c.DateTime(),
                        UserModified = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuctionEvents", t => t.EventId)
                .ForeignKey("dbo.Bidders", t => t.BidderId)
                .Index(t => t.EventId)
                .Index(t => t.BidderId);
            
            CreateTable(
                "dbo.Bidders",
                c => new
                    {
                        Id = c.Guid(nullable: false, identity: true),
                        Name = c.String(),
                        Phone = c.String(),
                        Paddle = c.Int(nullable: false),
                        EventId = c.Guid(),
                        DateCreatedUtc = c.DateTime(),
                        UserCreated = c.String(),
                        DateModifiedUtc = c.DateTime(),
                        UserModified = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AuctionEvents", t => t.EventId)
                .Index(t => t.EventId);
            
            CreateTable(
                "dbo.ReportSelectionViewModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        SelectedEventId = c.Guid(nullable: false),
                        TypeOfReport = c.Int(nullable: false),
                        OutputFormat = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        OrganizationId = c.Guid(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.OrganizationId)
                .Index(t => t.OrganizationId)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "OrganizationId", "dbo.Organizations");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AuctionEvents", "ReportSelectionViewModel_Id", "dbo.ReportSelectionViewModels");
            DropForeignKey("dbo.AuctionItems", "BidderId", "dbo.Bidders");
            DropForeignKey("dbo.Bidders", "EventId", "dbo.AuctionEvents");
            DropForeignKey("dbo.AuctionItems", "EventId", "dbo.AuctionEvents");
            DropForeignKey("dbo.AuctionEvents", "OrganizationId", "dbo.Organizations");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUsers", new[] { "OrganizationId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Bidders", new[] { "EventId" });
            DropIndex("dbo.AuctionItems", new[] { "BidderId" });
            DropIndex("dbo.AuctionItems", new[] { "EventId" });
            DropIndex("dbo.AuctionEvents", new[] { "ReportSelectionViewModel_Id" });
            DropIndex("dbo.AuctionEvents", new[] { "OrganizationId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ReportSelectionViewModels");
            DropTable("dbo.Bidders");
            DropTable("dbo.AuctionItems");
            DropTable("dbo.Organizations");
            DropTable("dbo.AuctionEvents");
        }
    }
}
