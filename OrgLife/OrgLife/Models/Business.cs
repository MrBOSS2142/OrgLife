namespace OrgLife.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Business")]
    public partial class Business
    {
        [Key]
        public int ID_Business { get; set; }

        [Required]
        public string TextWork { get; set; }

        [Required]
        [StringLength(10)]
        public string Date { get; set; }

        [StringLength(15)]
        public string State { get; set; }

        [StringLength(20)]
        public string Person { get; set; }

        public int? User { get; set; }

        public virtual User User1 { get; set; }
    }
}
