using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SPDemo.Models
{
    public partial class DbCarContext : DbContext
    {
        public DbCarContext()
        {
        }

        public DbCarContext(DbContextOptions<DbCarContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblCar> TblCar { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DSK-992\\SQL2019;Database=DbCar;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblCar>(entity =>
            {
                entity.Property(e => e.Brand).HasMaxLength(50);

                entity.Property(e => e.Model).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
