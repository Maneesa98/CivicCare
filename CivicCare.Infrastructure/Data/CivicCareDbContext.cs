using CivicCare.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CivicCare.Application.Contracts;
using System;

namespace CivicCare.Infrastructure.Data
{
    public partial class CivicCareDbContext : DbContext, IApplicationDbContext
    {
        public CivicCareDbContext()
        {
        }
        public CivicCareDbContext(DbContextOptions<CivicCareDbContext> options)
        : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.\\SQLEXPRESS;Database=CivicCareDB;Trusted_Connection=True;TrustServerCertificate=True"
                );
            }
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<RequestStatusHistory> StatusHistory { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships and constraints
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Name).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<ServiceRequest>(entity =>
            {
                entity.HasKey(sr => sr.Id);
                entity.Property(sr => sr.Title).IsRequired().HasMaxLength(200);
                entity.Property(sr => sr.Status).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<RequestStatusHistory>(entity =>
            {
                entity.HasKey(rsh => rsh.Id);
                entity.Property(rsh => rsh.Status).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Name).IsRequired().HasMaxLength(100);
            });
        }
    }
}
