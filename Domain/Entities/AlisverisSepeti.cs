namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AlisverisSepeti")]
    public partial class AlisverisSepeti
    {
        [Key]
        public int SepetId { get; set; }

        public int KullaniciId { get; set; }

        public int KitapId { get; set; }

        public virtual Kitap Kitap { get; set; }

        public virtual Kullanici Kullanici { get; set; }
    }
}
