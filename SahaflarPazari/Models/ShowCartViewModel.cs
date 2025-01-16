using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SahaflarPazari.Models
{
    public class ShopCartViewModel
    {
        public int CartId { get; set; } 
        public int BookId { get; set; }
        public string BookName { get; set; }
        public string SellerName { get; set; }
        public int BookPrice { get; set; }
        public string BookImagePath { get; set; }

    }
}