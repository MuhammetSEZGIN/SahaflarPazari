namespace Domain.Entities
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yayinevi")]
    public partial class Yayinevi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Yayinevi()
        {
            Kitaps = new HashSet<Kitap>();
        }

        public int YayineviID { get; set; }

        [Required]
        [StringLength(255)]
        public string YayineviAdi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kitap> Kitaps { get; set; }
    }
}
