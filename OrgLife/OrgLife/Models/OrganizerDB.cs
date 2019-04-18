namespace OrgLife.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OrganizerDB : DbContext
    {
        public OrganizerDB()
            : base("name=OrganizerDB")
        {
        }

        public virtual DbSet<Business> Business { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Notebook> Notebook { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                .Property(e => e.Gender)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.UserName)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Business)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Event)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Notebook)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.User);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Person)
                .WithOptional(e => e.User1)
                .HasForeignKey(e => e.User);
        }
    }
}
