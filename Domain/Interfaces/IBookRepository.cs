using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        // Add any additional methods specific to the Book entity if needed
    }
}
