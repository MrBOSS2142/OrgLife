namespace OrgLife.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Person")]
    public partial class Person
    {
        [Key]
        public int ID_Person { get; set; }

        [StringLength(50)]
        public string NickName { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Lastname { get; set; }

        [StringLength(50)]
        public string Patronymic { get; set; }

        [StringLength(10)]
        public string DOB { get; set; }

        [StringLength(1)]
        public string Gender { get; set; }

        [StringLength(13)]
        public string Phone_number { get; set; }

        public string Position { get; set; }

        public string Email { get; set; }

        public string AdressPerson { get; set; }

        [Column(TypeName = "image")]
        public byte[] PersonPhoto { get; set; }

        public int? User { get; set; }

        public virtual User User1 { get; set; }
    }
}
