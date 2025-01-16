namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BookImage")]
    public partial class BookImage
    {
        [Key]
        public int ImageId { get; set; }

        public int BookId { get; set; }

        [Required]
        [StringLength(255)]
        public string ImagePath { get; set; }

        public virtual Book Book { get; set; }
    }
}
