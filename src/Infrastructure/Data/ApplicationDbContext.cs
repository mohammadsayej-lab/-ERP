using Microsoft.EntityFrameworkCore;
using Core.Entities;

namespace Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<PayrollProfile> PayrollProfiles { get; set; } = null!;
    public DbSet<SalarySlip> SalarySlips { get; set; } = null!;
    public DbSet<ChartOfAccounts> ChartOfAccounts { get; set; } = null!;
    public DbSet<JournalEntry> JournalEntries { get; set; } = null!;
    public DbSet<JournalEntryDetail> JournalEntryDetails { get; set; } = null!;
    public DbSet<PayrollAccountMapping> PayrollAccountMappings { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Employee Configuration
        modelBuilder.Entity<Employee>()
            .HasKey(e => e.Id);
        modelBuilder.Entity<Employee>()
            .Property(e => e.EmployeeNumber)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<Employee>()
            .Property(e => e.Email)
            .HasMaxLength(100);

        // PayrollProfile Configuration
        modelBuilder.Entity<PayrollProfile>()
            .HasKey(pp => pp.Id);
        modelBuilder.Entity<PayrollProfile>()
            .Property(pp => pp.BasicSalary)
            .HasPrecision(18, 2);

        // JournalEntry Configuration
        modelBuilder.Entity<JournalEntry>()
            .HasKey(je => je.Id);
        modelBuilder.Entity<JournalEntry>()
            .Property(je => je.EntryNumber)
            .IsRequired()
            .HasMaxLength(50);
        modelBuilder.Entity<JournalEntry>()
            .HasMany(je => je.Details)
            .WithOne(jed => jed.JournalEntry)
            .HasForeignKey(jed => jed.JournalEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        // ChartOfAccounts Configuration
        modelBuilder.Entity<ChartOfAccounts>()
            .HasKey(coa => coa.Id);
        modelBuilder.Entity<ChartOfAccounts>()
            .Property(coa => coa.AccountCode)
            .IsRequired()
            .HasMaxLength(20);
        modelBuilder.Entity<ChartOfAccounts>()
            .HasIndex(coa => coa.AccountCode)
            .IsUnique();
    }
}
