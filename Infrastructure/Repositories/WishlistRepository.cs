using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class WishlistRepository : Repository<Wishlist>, IWishlistRepository
    {
        public WishlistRepository(SahaflarPazari context) : base(context)
        {
        }

        public async Task<IEnumerable<Wishlist>> GetWishlistsByUserIdAsync(string id)
        {
            return await _context.Wishlists.Where(w => w.UserId == id).ToListAsync();
        }
    }
}
