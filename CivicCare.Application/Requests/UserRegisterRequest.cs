using BCrypt.Net;
using CivicCare.Application.Contracts;
using CivicCare.Application.Dtos;
using CivicCare.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CivicCare.Application.Requests
{
    public class UserRegisterRequest : IRequest<CommonResponseDto<int>>
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }

        // Default role = Citizen
        public int RoleId { get; set; } = 2;

        public int? DepartmentId { get; set; }
    }

    public class UserRegisterHandler
        : IRequestHandler<UserRegisterRequest, CommonResponseDto<int>>
    {
        private readonly IApplicationDbContext _db;

        public UserRegisterHandler(IApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<CommonResponseDto<int>> Handle(
            UserRegisterRequest request,
            CancellationToken cancellationToken)
        {
            try
            {
                // 1️⃣ Check duplicate email
                var existingUser = await _db.Users
                    .AnyAsync(u => u.Email == request.Email,
                              cancellationToken);

                if (existingUser)
                {
                    return new CommonResponseDto<int>
                    {
                        Status = "error",
                        Message = "Email already exists",
                        Data = 0
                    };
                }

                // 2️⃣ Validate Role exists
                var roleExists = await _db.Roles
                    .AnyAsync(r => r.Id == request.RoleId,
                              cancellationToken);

                if (!roleExists)
                {
                    return new CommonResponseDto<int>
                    {
                        Status = "error",
                        Message = "Invalid RoleId",
                        Data = 0
                    };
                }

                // 3️⃣ Validate Department (only for Officer)
                if (request.DepartmentId.HasValue)
                {
                    var deptExists = await _db.Departments
                        .AnyAsync(d => d.Id == request.DepartmentId.Value,
                                  cancellationToken);

                    if (!deptExists)
                    {
                        return new CommonResponseDto<int>
                        {
                            Status = "error",
                            Message = "Invalid DepartmentId",
                            Data = 0
                        };
                    }
                }

                // 4️⃣ Create user
                var user = new User
                {
                    FullName = request.FullName,
                    Email = request.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    RoleId = request.RoleId,
                    DepartmentId = request.DepartmentId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _db.Users.Add(user);
                await _db.SaveChangesAsync(cancellationToken);

                return new CommonResponseDto<int>
                {
                    Status = "success",
                    Message = "User registered successfully",
                    Data = user.Id
                };
            }
            catch (Exception ex)
            {
                return new CommonResponseDto<int>
                {
                    Status = "error",
                    Message = "Failed to register user",
                    Error = ex.InnerException?.Message ?? ex.Message
                };
            }
        }
    }
}