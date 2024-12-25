namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Complaint")]
    public partial class Complaint
    {
        public int ComplaintId { get; set; }

        public int BookId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string ComplaintContent { get; set; }

        public DateTime? Date { get; set; }

        public virtual Book Book { get; set; }

        public virtual User User { get; set; }
    }
}
