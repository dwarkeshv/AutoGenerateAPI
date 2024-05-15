using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AutoGenerateAPI.Database.Models;

public partial class TryAutomationContext : DbContext
{
    public TryAutomationContext()
    {
    }

    public TryAutomationContext(DbContextOptions<TryAutomationContext> options)
        : base(options)
    {
    }

   

    public virtual DbSet<Application> Applications { get; set; }


    public virtual DbSet<HeroTable> HeroTables { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.AppId);

            entity.ToTable("Application");

            entity.Property(e => e.AppId).HasColumnName("appId");
            entity.Property(e => e.AdditionalInfoTableName)
                .HasMaxLength(50)
                .HasColumnName("additionalInfoTableName");
            entity.Property(e => e.AddtionalInfoId).HasColumnName("addtionalInfoID");
            entity.Property(e => e.HeroId).HasColumnName("heroId");
            entity.Property(e => e.UserId).HasColumnName("userId");
            entity.Property(e => e.UserTableName)
                .HasMaxLength(50)
                .HasColumnName("userTableName");
        });

      
        modelBuilder.Entity<HeroTable>(entity =>
        {
            entity.HasKey(e => e.HeroId);

            entity.ToTable("heroTable");

            entity.Property(e => e.HeroId).HasColumnName("heroId");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Title).HasColumnName("title");
        });

      

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
