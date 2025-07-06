using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Domain.Entities;
using MiniiPaaS.Domain.Exceptions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniiPaaS.Application.Commands.Auth
{
    public class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;

        public ConfirmEmailHandler(IApplicationDbContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.Email }, cancellationToken);
            if (user == null || user.EmailConfirmationToken != request.Token ||
                user.EmailConfirmationTokenExpiry < DateTime.UtcNow)
            {
                throw new AuthException("Invalid email confirmation attempt");
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, request.NewPassword);
            user.EmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationTokenExpiry = null;
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}