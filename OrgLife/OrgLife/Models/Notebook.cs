namespace OrgLife.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Notebook")]
    public partial class Notebook
    {
        [Key]
        public int ID_NB { get; set; }

        [Required]
        public string HeaderNB { get; set; }

        [Required]
        public string TextNotebook { get; set; }

        [Required]
        [StringLength(15)]
        public string DateTime { get; set; }

        public int? User { get; set; }

        public virtual User User1 { get; set; }
    }
}
