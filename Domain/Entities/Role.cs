namespace Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        [Key]
        public int RoleId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(255)]
        public string RoleName { get; set; }

        public virtual User User { get; set; }
    }
}
