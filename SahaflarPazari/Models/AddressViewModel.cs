using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SahaflarPazari.Models
{
    public class AddressViewModel
    {
        public int AddressId { get; set; }
        public string AddressName { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neighborhood { get; set; }
        public string PostalCode { get; set; }
        public string AddressArea { get; set; }


        public override string ToString()
        {
            return $"{AddressName} {City} {District} {Neighborhood} {PostalCode} {AddressArea}";
        }
    }

  

    }