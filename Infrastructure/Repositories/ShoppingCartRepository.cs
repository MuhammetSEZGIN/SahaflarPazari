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
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(SahaflarPazari context) : base(context)
        {
        }

        public async Task<IEnumerable<ShoppingCart>> GetShoppingCartsByUserIdAsync(string id)
        {
            return await _context.ShoppingCarts.Where(sc => sc.UserId == id).ToListAsync();
        }
    }
}
