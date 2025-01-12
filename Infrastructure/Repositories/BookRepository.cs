using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(Data.SahaflarPazari context) : base(context)
        {
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorAsync(string author)
        {
            return await _context.Books.Where(x=>x.Author.Equals(author)).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int category)
        {
           return await _context.Books.Where(x=>x.CategoryId.Equals(category)).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByTitleAsync(string title)
        {
            return await _context.Books.Where(x=>x.BookName.Equals(title)).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByUserIdAsync(string id)
        {
            return await _context.Books.Where(x=>x.BookId.Equals(id)).ToListAsync();
        }
       
    }
}
