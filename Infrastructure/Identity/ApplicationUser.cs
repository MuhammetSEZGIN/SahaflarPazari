using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<ShoppingCart> ShoppingCarts { get; set; }
        public virtual ICollection<Wishlist> Wishlists { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Complaint> Complaints { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public ApplicationUser()
        {
            Addresses = new HashSet<Address>();
            ShoppingCarts = new HashSet<ShoppingCart>();
            Wishlists = new HashSet<Wishlist>();
            Books = new HashSet<Book>();
            Complaints = new HashSet<Complaint>();
            Orders = new HashSet<Order>();
        }
    }
}

