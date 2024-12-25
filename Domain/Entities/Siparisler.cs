namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Siparisler")]
    public partial class Siparisler
    {
        [Key]
        public int SiparisId { get; set; }

        public int KullaniciId { get; set; }

        public int KitapId { get; set; }

        [Required]
        [StringLength(255)]
        public string Adres { get; set; }

        public DateTime SiparisTarihi { get; set; }

        [Required]
        [StringLength(255)]
        public string SiparisDurumu { get; set; }

        public virtual Kitap Kitap { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
