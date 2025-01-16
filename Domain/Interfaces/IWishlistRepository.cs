using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IWishlistRepository : IRepository<Wishlist>
    {
        Task<IEnumerable<Wishlist>> GetWishlistsByUserIdAsync(string id);
    }
}
