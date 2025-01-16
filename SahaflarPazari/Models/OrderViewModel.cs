using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SahaflarPazari.Models
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public string Status { get; set; }

        public string OrderDate { get; set; }

        public BookViewModel BookViewModel { get; set; }
        public string Address { get; set; }

    }
}