// MiniiPaaS.Application/Commands/Auth/LoginHandler.cs
using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniiPaaS.Application.Commands.Responses;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Domain.Entities;
using MiniiPaaS.Domain.Exceptions;

namespace MiniiPaaS.Application.Commands.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IJwtService _jwtService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginHandler(IApplicationDbContext context, IJwtService jwtService, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.Email }, cancellationToken);
            if (user == null)
            {
                throw new AuthException("Invalid login attempt");
            }

            // Check if account is locked
            if (user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow)
            {
                var remainingTime = user.LockoutEnd.Value - DateTime.UtcNow;
                throw new AuthException($"Account locked. Try again in {remainingTime.Minutes} minutes.");
            }

            // Verify password
            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result != PasswordVerificationResult.Success)
            {
                user.AccessFailedCount++;

                // Lock account after 5 failed attempts
                if (user.AccessFailedCount >= 5)
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                }

                await _context.SaveChangesAsync(cancellationToken);
                throw new AuthException("Invalid login attempt");
            }

            // Check if email is confirmed
            if (!user.EmailConfirmed)
            {
                throw new AuthException("Please confirm your email address first");
            }

            // Reset access failed count on successful login
            user.AccessFailedCount = 0;
            user.LockoutEnabled = false;
            user.LockoutEnd = null;
            await _context.SaveChangesAsync(cancellationToken);

            var token = _jwtService.GenerateToken(user);
            return new AuthResponse(
    user.Email,
    token,
    user.Role,
    user.CompanyId
);
            //return new AuthResponse { Token = token, Email = user.Email, Role = user.Role.ToString() };
        }
    }
}