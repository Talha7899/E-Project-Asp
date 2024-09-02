using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Health_Insurance_Management_System.Models;

public partial class HealthInsuranceContext : DbContext
{
    public HealthInsuranceContext()
    {
    }

    public HealthInsuranceContext(DbContextOptions<HealthInsuranceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Claim> Claims { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<InsuranceCompany> InsuranceCompanies { get; set; }

    public virtual DbSet<Policy> Policies { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=healthInsurance;User ID=sa;Password=aptech; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(e => e.ClaimId).HasName("PK__Claim__EF2E13BB83751424");

            entity.ToTable("Claim");

            entity.Property(e => e.ClaimId).HasColumnName("ClaimID");
            entity.Property(e => e.ClaimAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClaimDate).HasColumnType("date");
            entity.Property(e => e.ClaimStatus).HasMaxLength(50);
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");

            entity.HasOne(d => d.Employee).WithMany(p => p.Claims)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Claim__EmployeeI__32E0915F");

            entity.HasOne(d => d.Policy).WithMany(p => p.Claims)
                .HasForeignKey(d => d.PolicyId)
                .HasConstraintName("FK__Claim__PolicyID__31EC6D26");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Employee__7AD04FF1CDD9D79E");

            entity.ToTable("Employee");

            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.Department).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Policy).WithMany(p => p.Employees)
                .HasForeignKey(d => d.PolicyId)
                .HasConstraintName("FK__Employee__Policy__33D4B598");

            entity.HasOne(d => d.User).WithMany(p => p.Employees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Employee__UserID__34C8D9D1");
        });

        modelBuilder.Entity<InsuranceCompany>(entity =>
        {
            entity.HasKey(e => e.InsuranceCompanyId).HasName("PK__Insuranc__CE9C9444BD09F7F1");

            entity.ToTable("InsuranceCompany");

            entity.Property(e => e.InsuranceCompanyId).HasColumnName("InsuranceCompanyID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContactNumber).HasMaxLength(20);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Policy>(entity =>
        {
            entity.HasKey(e => e.PolicyId).HasName("PK__Policies__2E1339440CA8FD31");

            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.InsuranceCompanyId).HasColumnName("InsuranceCompanyID");
            entity.Property(e => e.PolicyName).HasMaxLength(100);

            entity.HasOne(d => d.InsuranceCompany).WithMany(p => p.Policies)
                .HasForeignKey(d => d.InsuranceCompanyId)
                .HasConstraintName("FK__Policies__Insura__35BCFE0A");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("PK__Report__D5BD48E570626CE9");

            entity.ToTable("Report");

            entity.Property(e => e.ReportId).HasColumnName("ReportID");
            entity.Property(e => e.ReportDate).HasColumnType("date");
            entity.Property(e => e.ReportType).HasMaxLength(50);

            entity.HasOne(d => d.GeneratedByNavigation).WithMany(p => p.Reports)
                .HasForeignKey(d => d.GeneratedBy)
                .HasConstraintName("FK__Report__Generate__36B12243");
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Request__33A8519AA12AA8D3");

            entity.ToTable("Request");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");
            entity.Property(e => e.InsuranceCompanyId).HasColumnName("InsuranceCompanyID");
            entity.Property(e => e.PolicyId).HasColumnName("PolicyID");
            entity.Property(e => e.RequestDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("date");
            entity.Property(e => e.RequestType).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValueSql("('Pending')");

            entity.HasOne(d => d.Employee).WithMany(p => p.Requests)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK__Request__Employe__37A5467C");

            entity.HasOne(d => d.InsuranceCompany).WithMany(p => p.Requests)
                .HasForeignKey(d => d.InsuranceCompanyId)
                .HasConstraintName("FK__Request__Insuran__38996AB5");

            entity.HasOne(d => d.Policy).WithMany(p => p.Requests)
                .HasForeignKey(d => d.PolicyId)
                .HasConstraintName("FK__Request__PolicyI__398D8EEE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC27EF392C0E");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E4EFE1B69B").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534FC89C3B9").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Role).HasDefaultValueSql("((2))");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
