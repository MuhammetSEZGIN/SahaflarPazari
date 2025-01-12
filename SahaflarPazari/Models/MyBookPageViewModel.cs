using Domain.Entities;
using System;
using System.Collections.Generic;


namespace SahaflarPazari.Models
{
    public class MyBookPageViewModel
    {
        public List<Book> Books { get; set; }
        public List<BookCategory> Categories { get; set; }
        public List<Publisher> Publishers { get; set; }

    }
}