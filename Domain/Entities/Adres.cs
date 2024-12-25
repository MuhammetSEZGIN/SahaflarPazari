namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Adres
    {
        [Key]
        public int AdresId { get; set; }

        public int KullaniciId { get; set; }

        [StringLength(255)]
        public string AdresAdi { get; set; }

        [StringLength(255)]
        public string Sehir { get; set; }

        [StringLength(255)]
        public string Ilce { get; set; }

        [StringLength(255)]
        public string Mahalle { get; set; }

        [StringLength(10)]
        public string PostaKodu { get; set; }

        [StringLength(255)]
        public string AdresAlani { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
