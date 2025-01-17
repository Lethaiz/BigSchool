using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BigSchool.Models
{
    public partial class BigSchoolContext : DbContext
    {
        public BigSchoolContext()
            : base("name=BigSchoolContext2")
        {
        }
        public virtual DbSet<Following> Following { get; set; }
        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Course> Course { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
                .HasMany(e => e.Attendance)
                .WithRequired(e => e.Course)
                .WillCascadeOnDelete(false);
        }
    }
}
