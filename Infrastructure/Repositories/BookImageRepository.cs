using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BookImagesRepository : Repository<BookImage>, IBookImagesRepository
    {
        public BookImagesRepository(Infrastructure.Data.SahaflarPazari context) : base(context)
        {
        }

        public async Task<IEnumerable<BookImage>> GetBookImagesByBookIdAsync(int bookId)
        {
            return await _context.BookImages.Where(x => x.BookId.Equals(bookId)).ToListAsync();
        }
         public void AddBookImages(BookImage image)
        {
           _context.BookImages.Add(image);
           
        }

        public void DeleteBookImagesAsync(List<BookImage> bookImages)
        {
            _context.BookImages.RemoveRange(bookImages);
        }
        public  void AddBoookImages(BookImage image)
        {
            _context.BookImages.Add(image);
            
        }

    }
}
