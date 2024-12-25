using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

namespace Domain.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        public async Task AddBookImageAsync(BookImage bookImage)
        {
            // Implement logic to add book image
        }

        public async Task DeleteBookImageAsync(int imageId)
        {
            // Implement logic to delete book image
        } 

        public async Task<IEnumerable<BookImage>> GetBookImagesAsync(int bookId)
        {
            // Implement logic to get book images
            return null;
        }
    }
}
