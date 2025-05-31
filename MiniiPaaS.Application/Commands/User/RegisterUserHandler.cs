using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniiPaaS.Infrastructure.Data;
using MiniiPaaS.Infrastructure.Services;
using MiniiPaaS.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Docker.DotNet.Models;

namespace MiniiPaaS.Application.Commands.User
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IJwtService _jwtService;
        private readonly MiniIPaaSDbContext _context;
        private readonly IPasswordHasher<Domain.Entities.User> _passwordHasher;

        public RegisterUserHandler(
            IJwtService jwtService, 
            MiniIPaaSDbContext context, 
            IPasswordHasher<Domain.Entities.User> hasher)
        {
            _jwtService = jwtService;
            _context = context;
            _passwordHasher = hasher;
        }

        public async Task<AuthResponse> Handle(
            RegisterUserCommand request, 
            CancellationToken cancellationToken)
        {
            var existing = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (existing is not null)
                throw new Exception("User already exists.");

            var user = new Domain.Entities.User { Email = request.Email };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            var token = _jwtService.GenerateToken(user);
            
         return new AuthResponse { Token = token, Email = user.Email };
        }
    }
}