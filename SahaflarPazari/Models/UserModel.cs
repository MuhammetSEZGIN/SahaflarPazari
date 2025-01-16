using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SahaflarPazari.Models
{
    public class UserModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone {  get; set; }
        public virtual List<Address> Addresses { get; set; }
        public virtual List<ShoppingCart> ShoppingCarts { get; set; }
        public virtual List<Wishlist> Wishlists { get; set; }

    }
}