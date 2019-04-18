namespace OrgLife.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Event")]
    public partial class Event
    {
        [Key]
        public int ID_Event { get; set; }

        [Required]
        public string Text_Event { get; set; }

        [Required]
        [StringLength(15)]
        public string DateEvent { get; set; }

        public int? User { get; set; }

        public virtual User User1 { get; set; }
    }
}
