namespace Infrastructure.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        AddressName = c.String(maxLength: 255, unicode: false),
                        City = c.String(maxLength: 255, unicode: false),
                        District = c.String(maxLength: 255, unicode: false),
                        Neighborhood = c.String(maxLength: 255, unicode: false),
                        PostalCode = c.String(maxLength: 10, unicode: false),
                        AddressArea = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.BookCategory",
                c => new
                    {
                        CategoryId = c.Int(nullable: false, identity: true),
                        CategoryName = c.String(nullable: false, maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.CategoryId);
            
            CreateTable(
                "dbo.Book",
                c => new
                    {
                        BookId = c.Int(nullable: false, identity: true),
                        SellerId = c.String(nullable: false, maxLength: 128),
                        CategoryId = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        BookName = c.String(nullable: false, maxLength: 255, unicode: false),
                        Description = c.String(maxLength: 1000, unicode: false),
                        AddedDate = c.DateTime(),
                        PublisherId = c.Int(),
                        Author = c.String(maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.BookId)
                .ForeignKey("dbo.Publisher", t => t.PublisherId)
                .ForeignKey("dbo.BookCategory", t => t.CategoryId)
                .ForeignKey("dbo.AspNetUsers", t => t.SellerId)
                .Index(t => t.SellerId)
                .Index(t => t.CategoryId)
                .Index(t => t.PublisherId);
            
            CreateTable(
                "dbo.BookImage",
                c => new
                    {
                        ImageId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        ImagePath = c.String(nullable: false, maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.ImageId)
                .ForeignKey("dbo.Book", t => t.BookId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Complaint",
                c => new
                    {
                        ComplaintId = c.Int(nullable: false, identity: true),
                        BookId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ComplaintContent = c.String(nullable: false, maxLength: 255, unicode: false),
                        Date = c.DateTime(),
                    })
                .PrimaryKey(t => t.ComplaintId)
                .ForeignKey("dbo.Book", t => t.BookId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.BookId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        BookId = c.Int(nullable: false),
                        Address = c.String(nullable: false, maxLength: 255, unicode: false),
                        OrderDate = c.DateTime(nullable: false),
                        OrderStatus = c.String(nullable: false, maxLength: 255, unicode: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Book", t => t.BookId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Publisher",
                c => new
                    {
                        PublisherId = c.Int(nullable: false, identity: true),
                        PublisherName = c.String(nullable: false, maxLength: 255),
                    })
                .PrimaryKey(t => t.PublisherId);
            
            CreateTable(
                "dbo.ShoppingCart",
                c => new
                    {
                        CartId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        BookId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CartId)
                .ForeignKey("dbo.Book", t => t.BookId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.Wishlist",
                c => new
                    {
                        ListId = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        BookId = c.Int(),
                    })
                .PrimaryKey(t => t.ListId)
                .ForeignKey("dbo.Book", t => t.BookId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId)
                .Index(t => t.BookId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
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
            DropForeignKey("dbo.Wishlist", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ShoppingCart", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Order", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Complaint", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Book", "SellerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Addresses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Book", "CategoryId", "dbo.BookCategory");
            DropForeignKey("dbo.Wishlist", "BookId", "dbo.Book");
            DropForeignKey("dbo.ShoppingCart", "BookId", "dbo.Book");
            DropForeignKey("dbo.Book", "PublisherId", "dbo.Publisher");
            DropForeignKey("dbo.Order", "BookId", "dbo.Book");
            DropForeignKey("dbo.Complaint", "BookId", "dbo.Book");
            DropForeignKey("dbo.BookImage", "BookId", "dbo.Book");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Wishlist", new[] { "BookId" });
            DropIndex("dbo.Wishlist", new[] { "UserId" });
            DropIndex("dbo.ShoppingCart", new[] { "BookId" });
            DropIndex("dbo.ShoppingCart", new[] { "UserId" });
            DropIndex("dbo.Order", new[] { "BookId" });
            DropIndex("dbo.Order", new[] { "UserId" });
            DropIndex("dbo.Complaint", new[] { "UserId" });
            DropIndex("dbo.Complaint", new[] { "BookId" });
            DropIndex("dbo.BookImage", new[] { "BookId" });
            DropIndex("dbo.Book", new[] { "PublisherId" });
            DropIndex("dbo.Book", new[] { "CategoryId" });
            DropIndex("dbo.Book", new[] { "SellerId" });
            DropIndex("dbo.Addresses", new[] { "UserId" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Wishlist");
            DropTable("dbo.ShoppingCart");
            DropTable("dbo.Publisher");
            DropTable("dbo.Order");
            DropTable("dbo.Complaint");
            DropTable("dbo.BookImage");
            DropTable("dbo.Book");
            DropTable("dbo.BookCategory");
            DropTable("dbo.Addresses");
        }
    }
}
