using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Domain.Entities;
using MiniiPaaS.Application.Commands.Responses;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace MiniiPaaS.Application.Commands.UserManagement
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IJwtService _jwtService;
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterUserHandler(
            IJwtService jwtService,
            IApplicationDbContext context,
            IPasswordHasher<User> passwordHasher)
        {
            _jwtService = jwtService;
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existing = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (existing is not null)
                throw new Exception("User already exists.");

            var user = new User
            {
                Email = request.Email,
                Role = request.Role,
                CompanyId = request.CompanyId
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            var token = _jwtService.GenerateToken(user);

            return new AuthResponse(
                Email: user.Email,
                Token: token,
                Role: user.Role,
                CompanyId: user.CompanyId
            );
        }
    }
}
