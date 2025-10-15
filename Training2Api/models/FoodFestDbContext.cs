using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Training2Api.models;

public partial class FoodFestDbContext : DbContext
{
    public FoodFestDbContext()
    {
    }

    public FoodFestDbContext(DbContextOptions<FoodFestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Attendee> Attendees { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=CHUAHMX\\MSSQLSERVER2022;Initial Catalog=FoodFestDB;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin");
        });

        modelBuilder.Entity<Attendee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendee__3214EC075D3299D3");

            entity.HasIndex(e => e.Email, "UQ__Attendee__A9D10534BB3564A4").IsUnique();

            entity.Property(e => e.City).HasMaxLength(120);
            entity.Property(e => e.Email).HasMaxLength(160);
            entity.Property(e => e.FullName).HasMaxLength(120);
            entity.Property(e => e.Phone).HasMaxLength(40);
            entity.Property(e => e.Region).HasMaxLength(60);
            entity.Property(e => e.RegisteredAtUtc)
                .HasPrecision(0)
                .HasDefaultValueSql("(sysutcdatetime())");
            entity.Property(e => e.TicketType)
                .HasMaxLength(40)
                .HasDefaultValue("General");

            entity.HasOne(d => d.Category).WithMany(p => p.Attendees)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Attendees_Category");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B407E0782");

            entity.ToTable("Category");

            entity.HasIndex(e => e.Name, "UQ__Category__737584F64ED3F7AE").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(80);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
