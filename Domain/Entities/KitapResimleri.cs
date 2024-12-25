namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KitapResimleri")]
    public partial class KitapResimleri
    {
        [Key]
        public int ResimId { get; set; }

        public int KitapId { get; set; }

        [Required]
        [StringLength(255)]
        public string DataYolu { get; set; }

        public virtual Kitap Kitap { get; set; }
    }
}
