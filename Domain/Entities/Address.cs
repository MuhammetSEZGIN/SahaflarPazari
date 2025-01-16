namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class Address
    {
        [Key]
        public int AddressId { get; set; }

        public string UserId { get; set; }

        [StringLength(255)]
        public string AddressName { get; set; }

        [StringLength(255)]
        public string City { get; set; }

        [StringLength(255)]
        public string District { get; set; }

        [StringLength(255)]
        public string Neighborhood { get; set; }

        [StringLength(10)]
        public string PostalCode { get; set; }

        [StringLength(255)]
        public string AddressArea { get; set; }

    }
}
