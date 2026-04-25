using Microsoft.EntityFrameworkCore;
using CivicCare.Domain.Models;
using System;

namespace CivicCare.Application.Contracts
{
    public interface IApplicationDbContext
    {
        public DbSet<User>Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<ServiceRequest> ServiceRequests { get; set; }
        public DbSet<RequestStatusHistory> StatusHistory { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        
    }
}
