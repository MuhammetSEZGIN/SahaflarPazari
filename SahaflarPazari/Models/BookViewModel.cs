using Domain.Entities;
using System.Collections.Generic;
namespace SahaflarPazari.Models
{
    public class BookViewModel
    {
        public List<BookImage> BookImages { get; set; }
        public string BookName { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int BookId { get; set; }

        public string PublisherName { get; set; }
        public string UserName { get; set; }
        public string AuthorName { get; set; }
        public string AddedDate { get; set; }

    }
}