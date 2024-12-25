namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Roller")]
    public partial class Roller
    {
        [Key]
        public int RolId { get; set; }

        public int KullaniciId { get; set; }

        [Required]
        [StringLength(50)]
        public string RolAdi { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
