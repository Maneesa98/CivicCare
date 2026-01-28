using CivicCare.Api.Data;
using CivicCare.Api.Models;
using MediatR;
using BCrypt.Net;

namespace CivicCare.Api.Requests
{
    public class UserRegisterRequest : IRequest<string>
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public int DepartmentId { get; set; }
    }

    public class UserRegisterHandler : IRequestHandler<UserRegisterRequest, string>
    {
        private readonly CivicCareDbContext _db;

        public UserRegisterHandler(CivicCareDbContext db)
        {
            _db = db;
        }

        public async Task<string> Handle(UserRegisterRequest request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                FullName = request.FullName,
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash),
                RoleId = request.RoleId,
                DepartmentId = request.DepartmentId
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync(cancellationToken);

            return "User registered";
        }
    }
}