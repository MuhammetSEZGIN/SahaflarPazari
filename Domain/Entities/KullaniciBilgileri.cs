namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KullaniciBilgileri")]
    public partial class KullaniciBilgileri
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int KullaniciId { get; set; }

        [Required]
        [StringLength(255)]
        public string Ad { get; set; }

        [Required]
        [StringLength(255)]
        public string Soyad { get; set; }

        [Required]
        [StringLength(11)]
        public string Telefon { get; set; }

        [Required]
        [StringLength(255)]
        public string Eposta { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
