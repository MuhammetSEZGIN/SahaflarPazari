namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Wishlist")]
    public partial class Wishlist
    {
        public string UserId { get; set; }

        public int? BookId { get; set; }

        [Key]
        public int ListId { get; set; }

        public virtual Book Book { get; set; }

    }
}
