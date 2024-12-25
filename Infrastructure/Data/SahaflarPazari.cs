using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Domain.Entities;
using System.Linq;

namespace Domain.Data
{
    public partial class SahaflarPazari : DbContext
    {
        public SahaflarPazari()
            : base("name=SahaflarPazari")
        {
        }

        public virtual DbSet<Address> Addresses { get; set; }
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public virtual DbSet<Wishlist> Wishlists { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<BookCategory> BookCategories { get; set; }
        public virtual DbSet<BookImage> BookImages { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserDetails> UserDetails { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Complaint> Complaints { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Publisher> Publishers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>()
                .Property(e => e.AddressName)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.City)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.District)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.Neighborhood)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.PostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<Address>()
                .Property(e => e.AddressArea)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.BookName)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .Property(e => e.Author)
                .IsUnicode(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.ShoppingCarts)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.BookImages)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Complaints)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Book>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Book)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BookCategory>()
                .Property(e => e.CategoryName)
                .IsUnicode(false);

            modelBuilder.Entity<BookCategory>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.BookCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<BookImage>()
                .Property(e => e.ImagePath)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Addresses)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.ShoppingCarts)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Wishlists)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Books)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.SellerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.UserDetails)
                .WithRequired(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Complaints)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserDetails>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetails>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetails>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<UserDetails>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Role>()
                .Property(e => e.RoleName)
                .IsUnicode(false);

            modelBuilder.Entity<Complaint>()
                .Property(e => e.ComplaintContent)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.Address)
                .IsUnicode(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.OrderStatus)
                .IsUnicode(false);
        }
    }
}
