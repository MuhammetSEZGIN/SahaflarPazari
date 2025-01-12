namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Order")]
    public partial class Order
    {
        [Key]
        public int OrderId { get; set; }

        public string UserId { get; set; }

        public int BookId { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(255)]
        public string OrderStatus { get; set; }

        public virtual Book Book { get; set; }

    }
}
