namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kitap")]
    public partial class Kitap
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kitap()
        {
            AlisverisSepetis = new HashSet<AlisverisSepeti>();
            IstekListesis = new HashSet<IstekListesi>();
            KitapResimleris = new HashSet<KitapResimleri>();
            Sikayets = new HashSet<Sikayet>();
            Siparislers = new HashSet<Siparisler>();
        }

        public int KitapId { get; set; }

        public int SaticiId { get; set; }

        public int KategoriId { get; set; }

        public int Fiyat { get; set; }

        [Required]
        [StringLength(255)]
        public string KitapAdi { get; set; }

        [StringLength(1000)]
        public string Detay { get; set; }

        public DateTime? EklenmeTarihi { get; set; }

        public int? YayineviId { get; set; }

        [StringLength(255)]
        public string Yazar { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AlisverisSepeti> AlisverisSepetis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IstekListesi> IstekListesis { get; set; }

        public virtual KitapKategorileri KitapKategorileri { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Yayinevi Yayinevi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KitapResimleri> KitapResimleris { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sikayet> Sikayets { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Siparisler> Siparislers { get; set; }
    }
}
