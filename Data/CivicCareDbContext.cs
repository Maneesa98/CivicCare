using CivicCare.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CivicCare.Api.Data
{
    public class CivicCareDbContext : DbContext
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
            if(!optionsBuilder.IsConfigured)
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
    }
}
