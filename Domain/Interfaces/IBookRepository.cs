using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author);
        Task<IEnumerable<Book>> GetBooksByCategoryAsync(int category);
        Task<IEnumerable<Book>> GetBooksByTitleAsync(string title);
        Task<IEnumerable<Book>> GetBooksByUserIdAsync(string id);


    }
}
