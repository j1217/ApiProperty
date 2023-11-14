using System;
using System.Collections.Generic;
using ApiProperty.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace ApiProperty.DataAccess
{
    public partial class DbpropertyJfazContext : DbContext
    {
        public DbpropertyJfazContext(DbContextOptions<DbpropertyJfazContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Property> Properties { get; set; }
        public virtual DbSet<PropertyImage> PropertyImages { get; set; }
        public virtual DbSet<PropertyTrace> PropertyTraces { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Owner>(entity =>
            {
                entity.HasKey(e => e.IdOwner).HasName("PK__Owner__D3261816A7D0FC0B");

                entity.ToTable("Owner");

                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.Birthday).HasColumnType("date");
                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<Property>(entity =>
            {
                entity.HasKey(e => e.IdProperty).HasName("PK__Property__842B6AA7D22503A8");

                entity.ToTable("Property");

                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.CodeInternal).HasMaxLength(50);
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdOwnerNavigation).WithMany(p => p.Properties)
                    .HasForeignKey(d => d.IdOwner)
                    .HasConstraintName("FK__Property__IdOwne__398D8EEE");
            });

            modelBuilder.Entity<PropertyImage>(entity =>
            {
                entity.HasKey(e => e.IdPropertyImage).HasName("PK__Property__018BACD564526092");

                entity.ToTable("PropertyImage");

                entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.PropertyImages)
                    .HasForeignKey(d => d.IdProperty)
                    .HasConstraintName("FK__PropertyI__IdPro__3F466844");
            });

            modelBuilder.Entity<PropertyTrace>(entity =>
            {
                entity.HasKey(e => e.IdPropertyTrace).HasName("PK__Property__373407C9DE60BDC9");

                entity.ToTable("PropertyTrace");

                entity.Property(e => e.DataSale).HasColumnType("datetime");
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Tax).HasColumnType("decimal(18, 2)");
                entity.Property(e => e.Value).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.IdPropertyNavigation).WithMany(p => p.PropertyTraces)
                    .HasForeignKey(d => d.IdProperty)
                    .HasConstraintName("FK__PropertyT__IdPro__3C69FB99");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
