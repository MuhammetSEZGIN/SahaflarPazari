using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
namespace Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Infrastructure.Data.SahaflarPazari _context;
    

        public IBookRepository Books { get; }

        public IAddressRepository Addresses { get; }

        public IOrderRepository Orders { get; }

        public IComplaintRepository Complaints { get; }

        public IShoppingCartRepository ShoppingCarts { get; }

        public IWishlistRepository Wishlists { get; }
        public IBookImagesRepository BookImages { get; }


        public IPublisherRepository Publishers { get; }

        public IBookCategoryRepository BookCategories { get; }

        public UnitOfWork(Infrastructure.Data.SahaflarPazari context,
                        
                           IBookRepository bookRepository,
                           IAddressRepository addressRepository,
                           IOrderRepository orderRepository,
                           IComplaintRepository complaintRepository,
                           IShoppingCartRepository shoppingCartRepository,
                           IWishlistRepository wishlistRepository,
                           IBookImagesRepository bookImagesRepository,
                           IBookCategoryRepository bookCategoryRepository,
                            IPublisherRepository publisherRepository   
                           )
        {
            _context = context;
            BookImages = bookImagesRepository;
            Books = bookRepository;
            Addresses = addressRepository;
            Orders = orderRepository;
            Complaints = complaintRepository;
            ShoppingCarts = shoppingCartRepository;
            Wishlists = wishlistRepository;
            BookCategories= bookCategoryRepository;
            Publishers = publisherRepository;   
        }

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
