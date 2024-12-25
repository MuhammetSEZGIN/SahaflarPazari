namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ShoppingCart")]
    public partial class ShoppingCart
    {
        [Key]
        public int CartId { get; set; }

        public int UserId { get; set; }

        public int BookId { get; set; }

        public virtual Book Book { get; set; }

        public virtual User User { get; set; }
    }
}
