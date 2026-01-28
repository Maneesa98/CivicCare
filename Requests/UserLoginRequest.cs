using CivicCare.Api.Data;
using CivicCare.Api.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CivicCare.Api.Requests
{
    public class UserLoginRequest : IRequest<string?>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginHandler : IRequestHandler<UserLoginRequest, string?>
    {
        private readonly CivicCareDbContext _db;
        private readonly IConfiguration _config;

        public UserLoginHandler(CivicCareDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<string?> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.Include(u => u.Role)
                                      .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return null;

            return JwtHelper.GenerateToken(user, _config);
        }
    }
}