using MediatR;
using Microsoft.AspNetCore.Identity;
using MiniiPaaS.Application.Interfaces;
using MiniiPaaS.Application.Commands.Responses;
using MiniiPaaS.Domain.Entities;
using MiniiPaaS.Domain.Enums;
using MiniiPaaS.Domain.Exceptions;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MiniiPaaS.Application.Commands.UserManagement
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly IJwtService _jwtService;
        private readonly IApplicationDbContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IEmailService _emailService;

        public RegisterUserHandler(
            IJwtService jwtService,
            IApplicationDbContext context,
            IPasswordHasher<User> passwordHasher,
            IEmailService emailService)
        {
            _jwtService = jwtService;
            _context = context;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            // ❌ SuperAdmin manuel oluşturulmalı
            if (request.Role == Role.SuperAdmin)
                throw new AuthException("SuperAdmin can only be created manually.");

            // ❌ Kullanıcı zaten varsa hata ver
            var existing = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (existing is not null)
                throw new AuthException("User already exists.");

            // ✅ Yeni kullanıcı nesnesi
            var user = new User
            {
                Email = request.Email,
                Role = request.Role,
                CompanyId = request.CompanyId,
                EmailConfirmed = false,
                EmailConfirmationToken = Guid.NewGuid().ToString(),
                EmailConfirmationTokenExpiry = DateTime.UtcNow.AddDays(1)
            };

            // ⚠ Geçici şifre ataması
            user.PasswordHash = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString());

            _context.Users.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            // 📧 E-posta gönderimi
            var confirmationLink = $"https://yourapp.com/confirm-email?email={user.Email}&token={user.EmailConfirmationToken}";
            await _emailService.SendEmailAsync(
                user.Email,
                "Confirm Your Email",
                $"Welcome! Please confirm your email and set your password by clicking <a href='{confirmationLink}'>here</a>");

            // 🎟 Opsiyonel: JWT token döndür (email onayından sonra da yapabilirsin)
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
