namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sikayet")]
    public partial class Sikayet
    {
        public int SikayetId { get; set; }

        public int KitapId { get; set; }

        public int KullaniciId { get; set; }

        [Required]
        [StringLength(255)]
        public string SikayetIcerigi { get; set; }

        public DateTime? Tarih { get; set; }

        public virtual Kitap Kitap { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
