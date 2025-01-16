using Domain.Entities;
using Infrastructure.Identity;
using System.Collections.Generic;

namespace SahaflarPazari.Models
{
    public class ProfileViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Wishlist> Wishlist { get; set; }
    }
}