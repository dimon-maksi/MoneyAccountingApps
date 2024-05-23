using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MoneyAccountingAppEF.Models;

namespace MoneyAccountingAppEF.Data;

public partial class MoneyAccountingDbContext : DbContext
{
    public MoneyAccountingDbContext()
    {
    }

    public MoneyAccountingDbContext(DbContextOptions<MoneyAccountingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<Income> Incomes { get; set; }

    public virtual DbSet<Saving> Savings { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.;Database=MoneyAccountingDB;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Accounts__3214EC07209E2D0C");

            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC074FFD1D82");

            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Expenses__3214EC0793317E3B");

            entity.HasIndex(e => e.Date, "IX_Expenses_Date");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Type).HasMaxLength(255);

            entity.HasOne(d => d.Account).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Expenses__Accoun__412EB0B6");

            entity.HasOne(d => d.Category).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Expenses__Catego__4222D4EF");
        });

        modelBuilder.Entity<Income>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Income__3214EC0766B3A839");

            entity.ToTable("Income");

            entity.HasIndex(e => e.Date, "IX_Income_Date");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Source).HasMaxLength(255);
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Account).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Income__AccountI__3F466844");

            entity.HasOne(d => d.Category).WithMany(p => p.Incomes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Income__Category__403A8C7D");
        });

        modelBuilder.Entity<Saving>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Savings__3214EC07B783A8CB");

            entity.HasIndex(e => e.Date, "IX_Savings_Date");

            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Goal).HasMaxLength(255);
            entity.Property(e => e.Sum).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Account).WithMany(p => p.Savings)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Savings__Account__4316F928");

            entity.HasOne(d => d.Category).WithMany(p => p.Savings)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Savings__Categor__440B1D61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
