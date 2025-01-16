using Domain.Entities;
using System.Collections.Generic;

namespace SahaflarPazari.Models
{
    public class MainPageModel
    {
        public List<Book> books;
        public List<Book> lastAddedBooks;
        public List<int> wishListBooks;
    }
}