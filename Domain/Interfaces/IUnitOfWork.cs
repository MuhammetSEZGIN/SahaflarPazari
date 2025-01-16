using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
      
        IBookRepository Books { get; }
        IAddressRepository Addresses { get; }
        IOrderRepository Orders { get; }
        IComplaintRepository Complaints { get; }
        IShoppingCartRepository ShoppingCarts { get; }
        IWishlistRepository Wishlists { get; }
        IBookImagesRepository BookImages { get; }
        IPublisherRepository Publishers { get; }
        IBookCategoryRepository BookCategories { get; }
        Task<int> CommitAsync();
        

    }
}
