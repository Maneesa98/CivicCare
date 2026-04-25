using BCrypt.Net;
using CivicCare.Api.CivicCare.Application.Helpers;
using CivicCare.Application.Contracts;
using CivicCare.Application.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CivicCare.Application.Requests
{
    public class UserLoginRequest : IRequest<CommonResponseDto<string>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserLoginHandler : IRequestHandler<UserLoginRequest, CommonResponseDto<string>>
    {
        private readonly IApplicationDbContext _db;
        private readonly IConfiguration _config;

        public UserLoginHandler(IApplicationDbContext db, IConfiguration config)
        {
            _db = db;
            _config = config;
        }

        public async Task<CommonResponseDto<string>> Handle(UserLoginRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _db.Users.Include(u => u.Role)
                                          .FirstOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return new CommonResponseDto<string>
                    {
                        Message = "Invalid email or password",
                        Status = "error"
                    };
                }

                var token = JwtHelper.GenerateToken(user, _config);

                return new CommonResponseDto<string>
                {
                    Message = "Login successful",
                    Status = "success",
                    Data = token
                };
            }
            catch (Exception ex)
            {
                return new CommonResponseDto<string>
                {
                    Message = "Failed to login",
                    Status = "error",
                    Error = ex.Message
                };
            }
        }
    }
}