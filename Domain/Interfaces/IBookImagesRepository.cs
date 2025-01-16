using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBookImagesRepository : IRepository<BookImage>
    {
        Task<IEnumerable<BookImage>> GetBookImagesByBookIdAsync(int bookId);
        void AddBookImages(BookImage image);
        void DeleteBookImagesAsync(List<BookImage> bookImages);
        
    }

}
