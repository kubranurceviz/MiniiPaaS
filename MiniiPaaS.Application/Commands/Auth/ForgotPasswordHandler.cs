using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MiniiPaaS.Application.Commands.Auth
{
    public class ForgotPasswordHandler : IRequestHandler<ForgotPasswordCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public ForgotPasswordHandler(IApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<bool> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FindAsync(new object[] { request.Email }, cancellationToken);
            if (user == null || !user.EmailConfirmed)
            {
                // Don't reveal that the user does not exist or is not confirmed
                return true;
            }

            // Generate password reset token
            var token = Guid.NewGuid().ToString();
            user.EmailConfirmationToken = token;
            user.EmailConfirmationTokenExpiry = DateTime.UtcNow.AddHours(1);
            await _context.SaveChangesAsync(cancellationToken);

            // Send email
            var resetLink = $"https://yourapp.com/reset-password?email={user.Email}&token={token}";
            await _emailService.SendEmailAsync(user.Email, "Reset Password",
                $"Please reset your password by clicking <a href='{resetLink}'>here</a>");

            return true;
        }
    }
}
