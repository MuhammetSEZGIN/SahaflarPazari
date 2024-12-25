using Domain.Data;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(SahaflarPazari context) : base(context)
        {
        }

        // Implement any additional methods specific to the Book entity if needed
    }
}
