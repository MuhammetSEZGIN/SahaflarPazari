using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    internal interface IBookService
    {
        Task<Book> GetBookByIdAsync(int id);
        Task<IEnumerable<Book>> GetAllBooksAsync();
        Task AddBookAsync(Book book);
        Task UpdateBookAsync(Book book);
        Task DeleteBookAsync(int id);

        // Methods for managing book images
        Task AddBookImageAsync(BookImage bookImage);
        Task DeleteBookImageAsync(int imageId);
        Task<IEnumerable<BookImage>> GetBookImagesAsync(int bookId);
    }
}
